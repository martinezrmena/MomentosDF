using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Services.FontSize;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace AppCarnesDF.iOS.Servicios.Notifications
{
    public class iOSNotificationManager
    {
        private LogMessageAttention Message = new LogMessageAttention();
        private FontSizeService servicio = new FontSizeService();

        public iOSNotificationManager()
        {
            Message.SizeFonts = servicio.ConsultarFont();
            Message.SizeFontsCookie = servicio.ConsultarFontCookie();
            Message.SizeFontsOptima = servicio.ConsultarFontOptima();
        }

        public Task CreateDefaultNotification(string body, string title)
        {
            return Task.Run(async () =>
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            title = "Nueva Promoción - Momentos Don Fernando";
                            //Schedule local notification
                            UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
                            UILocalNotification oNotification = new UILocalNotification();
                            oNotification.AlertTitle = title;
                            oNotification.AlertBody = body;
                            //you can set fire time of notification as well
                            // oNotification.FireDate = NSDate.FromTimeIntervalSinceNow(1);
                            //Will push notification after 15 sec
                            oNotification.SoundName = UILocalNotification.DefaultSoundName;
                            //Push notification
                            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 1;
                            UIApplication.SharedApplication.ScheduleLocalNotification(oNotification);
                        }
                        catch (Exception oExp)
                        {
                            await Message.Failed("Error detectado: " + oExp.Message);
                            Debug.WriteLine("Error detectado: " + oExp.Message);
                        }
                    });

                }
                catch (Exception oExp)
                {
                    await Message.Failed("Error detectado: " + oExp.Message);
                    Debug.WriteLine("Error detectado: " + oExp.Message);
                }
            });
        }
    }
}