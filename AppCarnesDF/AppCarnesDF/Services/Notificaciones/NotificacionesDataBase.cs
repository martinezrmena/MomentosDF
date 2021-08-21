using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Notificaciones;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Services.Notificaciones
{
    public class NotificacionesDataBase
    {
        readonly SQLiteConnection database;

        public NotificacionesDataBase()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(dbPath);
            database.CreateTable<NotificacionItem>();
        }

        public List<NotificacionItem> GetItemsAsync()
        {
            return database.Table<NotificacionItem>().ToList();
        }

        public NotificacionItem GetItemAsync(string id)
        {
            return database.Table<NotificacionItem>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItemAsync(NotificacionItem item)
        {
            try
            {
                return database.Insert(item);
            }
            catch (Exception ex)
            {
            }

            return 0;

        }

        public int UpdateItemAsync(NotificacionItem item)
        {
            try
            {

                return database.Update(item);
            }
            catch (Exception ex)
            {

            }

            return 0;

        }

        public int DeleteItemAsync(NotificacionItem item)
        {
            return database.Delete(item);
        }
    }
}
