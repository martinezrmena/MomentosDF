using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Notificaciones;
using AppCarnesDF.Services.Notificaciones;
using SQLite;

namespace AppCarnesDF.Droid.Servicios
{
    public class ConsultNative
    {
        readonly SQLiteConnection database;
        public ObservableCollection<NotificacionModel> n_config { get; set; }

        public ConsultNative()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = Path.Combine(path, DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(path);
            database.CreateTable<NotificacionItem>();
        }


        public bool ConsultValor() 
        {
            if (n_config == null)
            {
                n_config = new ObservableCollection<NotificacionModel>();
                TraerLista();
            }

            var item = n_config.FirstOrDefault();

            if (item == null)
            {
                return true;
            }

            return item.Activated;

        }

        public void TraerLista()
        {
            n_config.Clear();
            List<NotificacionItem> Lista = GetItemsAsync();
            foreach (var item in Lista)
            {
                n_config.Add(ConvertirDataBaseAModelo(item));
            }
        }

        public List<NotificacionItem> GetItemsAsync()
        {
            return database.Table<NotificacionItem>().ToList();
        }

        private NotificacionModel ConvertirDataBaseAModelo(NotificacionItem data)
        {
            NotificacionModel modelo = new NotificacionModel()
            {
                Id = data.Id,
                Activated = data.Activated
            };

            return modelo;
        }

        private NotificacionItem ConvertirModeloADataBase(NotificacionModel model)
        {
            NotificacionItem data = new NotificacionItem()
            {
                Id = model.Id,
                Activated = model.Activated
            };

            return data;
        }

    }
}