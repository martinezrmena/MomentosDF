using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Services.Notificaciones
{
    public interface IHandleLocalNotification
    {
        void ReceiveNotification();

        void NotReceiveNotification();

    }
}
