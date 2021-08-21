using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Droid.Servicios.Notifications;
using AppCarnesDF.Services.Notificaciones;
using Xamarin.Forms;

[assembly: Dependency(typeof(HandleLocalNotification))]
namespace AppCarnesDF.Droid.Servicios.Notifications
{
    public class HandleLocalNotification: IHandleLocalNotification
    {

        public void ReceiveNotification()
        { }

        public void NotReceiveNotification()
        { }
    }
}