using System;
using UserNotifications;


namespace AppCarnesDF.iOS.Servicios.Notifications
{
    public class iOSBannerNotification : UNUserNotificationCenterDelegate
    {
        public iOSBannerNotification()
        {

        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}