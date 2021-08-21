using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using AppCarnesDF.Helpers.Azure;

namespace AppCarnesDF.Droid.Servicios
{
    [Service]
    public class AndroidMethods
    {
        public void PushNotification(string Message)
        {
            var intent = new Intent(Application.Context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", Message);
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.OneShot);
            string Title = "Nueva Promoción - Momentos Don Fernando";

            var notificationBuilder = new NotificationCompat.Builder(Application.Context, AzureConstants.NotificationChannelName)
                .SetContentTitle(Title)
                .SetSmallIcon(Resource.Drawable.logo)
                .SetContentText(Message)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId("FirebasePushNotificationChannel");
            }

            var notificationManager = NotificationManager.FromContext(Application.Context);
            notificationManager.Notify(0, notificationBuilder.Build());

        }
    }
}