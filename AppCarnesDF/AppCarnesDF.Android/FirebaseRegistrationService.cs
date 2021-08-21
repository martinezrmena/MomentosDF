using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Util;
using AppCarnesDF.Droid.Servicios;
using AppCarnesDF.Helpers.Azure;
using AppCarnesDF.Services.Notificaciones;
using Firebase.Iid;
using WindowsAzure.Messaging;

namespace AppCarnesDF.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseRegistrationService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseMsgService";
        NotificationHub hub;

        public override void OnTokenRefresh()
        {
            string token = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "FCM token: " + token);
            SendRegistrationToServer(token);
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                hub = new NotificationHub(AzureConstants.NotificationHubName,
                                            AzureConstants.ListenConnectionString, this);

                // Register with Notification Hubs
                var tags = new List<string>() { };
                var regID = hub.Register(token, tags.ToArray()).RegistrationId;

                Log.Debug(TAG, $"Successful registration of ID {regID}");
            }
            catch (System.Exception)
            {

            }
        }
    }
}