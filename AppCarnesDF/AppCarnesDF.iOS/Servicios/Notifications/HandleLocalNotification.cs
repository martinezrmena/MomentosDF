using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCarnesDF.iOS.Servicios.Notifications;
using AppCarnesDF.Services.Notificaciones;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(HandleLocalNotification))]
namespace AppCarnesDF.iOS.Servicios.Notifications
{
    public class HandleLocalNotification: IHandleLocalNotification
    {
        public void ReceiveNotification() 
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
                            UIApplication.SharedApplication.RegisterForRemoteNotifications();
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

        public void NotReceiveNotification() 
        {
            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
            UIApplication.SharedApplication.UnregisterForRemoteNotifications();
        }
    }
}