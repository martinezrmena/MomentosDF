using AppCarnesDF.Helpers;
using AppCarnesDF.Models.User;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Services.User
{
    /// <summary>
    /// Clase encargada de manejar la base de datos
    /// en especifico la tabla de usuario del sistema
    /// </summary>
    public class UserDataBase
    {
        readonly SQLiteConnection database;

        public UserDataBase()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(dbPath);
            database.CreateTable<UserItem>();
        }

        public List<UserItem> GetItemsAsync()
        {
            return database.Table<UserItem>().ToList();
        }

        public UserItem GetItemAsync(string id)
        {
            return database.Table<UserItem>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItemAsync(UserItem item)
        {
            try
            {
                return database.Insert(item);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int UpdateItemAsync(UserItem item)
        {
            try
            {

                return database.Update(item);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int DeleteItemAsync(UserItem item)
        {
            return database.Delete(item);
        }
    }
}