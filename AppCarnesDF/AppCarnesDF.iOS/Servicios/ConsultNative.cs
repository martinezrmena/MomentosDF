using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Notificaciones;
using Foundation;
using SQLite;
using UIKit;

namespace AppCarnesDF.iOS.Servicios
{
    public class ConsultNative
    {
        readonly SQLiteConnection database;
        public ObservableCollection<NotificacionModel> n_config { get; set; }

        public ConsultNative()
        {
            string path;
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            path = Path.Combine(libFolder, DataBaseConstants.DatabaseName);

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