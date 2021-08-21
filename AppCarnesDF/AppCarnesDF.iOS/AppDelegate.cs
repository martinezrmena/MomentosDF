using System;
using System.Diagnostics;
using System.Linq;
using AppCarnesDF.Helpers.Azure;
using AppCarnesDF.iOS.Servicios;
using AppCarnesDF.iOS.Servicios.Notifications;
using Foundation;
using Plugin.DeviceInfo;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;

namespace AppCarnesDF.iOS
{
    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    /// User Interface of the application, as well as listening (and optionally responding) to 
    /// application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        #region Properties
        private SBNotificationHub Hub { get; set; }
        private NSDictionary _launchOptions { get; set; }
        public bool Activated { get; set; } = true;
        #endregion

        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run. In this 
        /// method you should instantiate the window, load the UI into it and then make the window
        /// visible.
        /// You have 17 seconds to return from this method, or iOS will terminate your application.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            try
            {
                #region Inicializacion de plugins
                Rg.Plugins.Popup.Popup.Init();

                global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");

                global::Xamarin.Forms.Forms.Init();
                //Plugin para controles nuevos
                Plugin.InputKit.Platforms.iOS.Config.Init();
                #endregion

                LoadApplication(new App());

                _launchOptions = options;

                base.FinishedLaunching(app, options);

                ConsultNative servicio = new ConsultNative();
                Activated = servicio.ConsultValor();

                if (Activated)
                {
                    #region Notificaciones
                    //For banner notification
                    UNUserNotificationCenter.Current.Delegate = new iOSBannerNotification();
                    //Ask user permission to display notification . its support for iOS 8

                    var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
                    UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);
                    #endregion

                    RegisterForRemoteNotifications();
                }

                AppTrackingTransparency.ATTrackingManager.RequestTrackingAuthorization((result) =>
                {
                    ConsultAllowRegister servicioRegister = new ConsultAllowRegister();

                    switch (result)
                    {
                        case AppTrackingTransparency.ATTrackingManagerAuthorizationStatus.NotDetermined:
                            break;
                        case AppTrackingTransparency.ATTrackingManagerAuthorizationStatus.Restricted:
                            break;
                        case AppTrackingTransparency.ATTrackingManagerAuthorizationStatus.Denied:
                            //Si no tendra acceso sera activado
                            servicioRegister.SaveItemAsync(false);
                            break;
                        case AppTrackingTransparency.ATTrackingManagerAuthorizationStatus.Authorized:
                            servicioRegister.SaveItemAsync(true);
                            break;
                        default:
                            break;
                    }
                });
            

            }
            catch (Exception ex)
            {

            }
            

            return true;
        }

        //Se dispara cuando la aplicacion termina su deploy
        public override void FinishedLaunching(UIApplication application)
        {
            base.FinishedLaunching(application);

            try
            {

                
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Si la aplicacion no estaba habilitada tomamos el notification badge y lo entregamos por las opciones.
        /// </summary>
        /// <param name="uiApplication"></param>
        public async override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);

            try
            {
                //Es necesario validar si en las configuraciones el usuario ha decidido recibir
                //notificaciones.
                ConsultNative servicio = new ConsultNative();
                Activated = servicio.ConsultValor();

                if (Activated)
                {
                    //If app was not running and we come from a notificatio badge, the notification is delivered via the options.
                    if (_launchOptions != null && _launchOptions.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                    {
                        var notification = _launchOptions[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
                        ProcessNotification(_launchOptions, false);
                    }
                    _launchOptions = null;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Registro a Azure
        /// <summary>
        /// Metodo que permite registrarse en Azure para recibir notificaciones remotas
        /// </summary>
        async void RegisterForRemoteNotifications()
        {
            try
            {
                // register for remote notifications based on system version
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                        UNAuthorizationOptions.Sound |
                        UNAuthorizationOptions.Sound,
                        (granted, error) =>
                        {
                            if (granted)
                                InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                        });
                }
                else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                    UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                }
                else
                {
                    UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                    UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Cuando el dispositivo se haya registrado correctamente para notificaciones remotas durante el
        /// FinishedLaunchingmétodo, iOS llamará a este metodo
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        public async override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            try
            {
                Hub = new SBNotificationHub(AzureConstants.ListenConnectionString, AzureConstants.NotificationHubName);

                // actualizar registro con Azure Notification Hub
                Hub.UnregisterAllAsync(deviceToken, async (error) =>
                {
                    if (error != null)
                    {
                        Debug.WriteLine($"Unable to call unregister {error}");
                        return;
                    }

                    var tags = new NSSet(AzureConstants.SubscriptionTags.ToArray());
                    Hub.RegisterNativeAsync(deviceToken, tags, async (errorCallback) =>
                    {
                        if (errorCallback != null)
                        {
                            Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                        }
                    });

                    var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                    Hub.RegisterTemplateAsync(deviceToken, "defaultTemplate", AzureConstants.APNTemplateBody, templateExpiration, tags, async (errorCallback) =>
                    {
                        if (errorCallback != null)
                        {
                            if (errorCallback != null)
                            {
                                Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                            }
                        }
                    });
                });
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Manejo de Notificación

        public async override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            try
            {
                ProcessNotification(userInfo, false);
            }
            catch (Exception ex)
            {
            }
        }

        async void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            try
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

                // make sure we have a payload
                if (options != null && options.ContainsKey(new NSString("aps")))
                {
                    // get the APS dictionary and extract message payload. Message JSON will be converted
                    // into a NSDictionary so more complex payloads may require more processing
                    NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                    string payload = string.Empty;
                    string title = string.Empty;
                    NSString payloadKey = new NSString("alert");
                    NSString payloadTitle = new NSString("title");

                    if (aps.ContainsKey(payloadKey))
                    {
                        payload = aps[payloadKey].ToString();
                    }

                    if (aps.ContainsKey(payloadTitle))
                    {
                        title = aps[payloadTitle].ToString();
                    }

                    //Necesario mostrar la notificacion
                    //CreateLocalNotification(payload, title);

                }
            }
            catch (Exception ex)
            {                
            }

        }

        //Call notification to create from your class
        public void CreateLocalNotification(string body, string title)
        {
            iOSNotificationManager oiOSNotify = new iOSNotificationManager();
            oiOSNotify.CreateDefaultNotification(body, title);
        }

        #endregion
    }
}