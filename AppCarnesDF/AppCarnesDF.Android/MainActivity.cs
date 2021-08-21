using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using Android.Views;
using Android.Content;
using Plugin.FirebasePushNotification;
using WindowsAzure.Messaging;
using AppCarnesDF.Helpers.Azure;
using AppCarnesDF.Droid.Servicios;
using System.Linq;

namespace AppCarnesDF.Droid
{
    [Activity(Label = "Momentos Don Fernando",
              Icon = "@drawable/logo",
              Theme = "@style/splashscreen", 
              MainLauncher = true, 
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              ScreenOrientation = ScreenOrientation.Portrait,
              LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public bool Activated { get; set; }
        ConsultNative model;
        NotificationHub hub;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window.RequestFeature(WindowFeatures.ActionBar);

            // For global use "global::" prefix - global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            #region Inicializacion de plugins
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //Inicializamos plugin para mensajes
            UserDialogs.Init(this);
            //Inicializamos plugin para utilizar la camara
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //This forces the custom renderers to be used
            //Android.Glide.Forms.Init();
            #endregion

            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            CreateNotificationChannel();
            ConsultAllowRegister servicioRegister = new ConsultAllowRegister();
            var values = servicioRegister.GetItemsAsync();

            if (values == null && !values.Any())
            {
                servicioRegister.SaveItemAsync(true);
            }

        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = "FirebasePushNotificationChannel";
            var channelDescription = string.Empty;
            var channel = new NotificationChannel("FirebasePushNotificationChannel", channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Metodo para controlar el boton de retorno del dispositivo al aparecer un pop up
        /// </summary>
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

    }
}