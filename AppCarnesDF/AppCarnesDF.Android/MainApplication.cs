using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AppCarnesDF.Droid.Servicios;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;

namespace AppCarnesDF.Droid
{
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private AndroidMethods methods = new AndroidMethods();

        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            #region Funcionalidad no utilizada
            ////Set the default notification channel for your app when running Android Oreo
            //if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            //{
            //    //Change for your default notification channel id here
            //    FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

            //    //Change for your default notification channel name here
            //    FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            //}

            //If debug you should reset the token each time.
#if DEBUG
            //FirebasePushNotificationManager.Initialize(this, true);
#else
              //FirebasePushNotificationManager.Initialize(this,false);
#endif
            #endregion

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                var model = new ConsultNative();

                bool Activated = model.ConsultValor();

                if (Activated)
                {
                    string messageBody = string.Empty;

                    foreach (var data in p.Data)
                    {
                        if (data.Key.Equals("message"))
                        {
                            messageBody = data.Value.ToString();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(messageBody))
                    {
                        methods.PushNotification(messageBody);
                    }
                }
            };
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

    }
}