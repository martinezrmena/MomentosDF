using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Permissions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Services.Permissions
{
    public class AllowRegisterDataBase
    {
        readonly SQLiteConnection database;

        public AllowRegisterDataBase()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(dbPath);
            database.CreateTable<AllowRegisterItem>();
        }

        public List<AllowRegisterItem> GetItemsAsync()
        {
            return database.Table<AllowRegisterItem>().ToList();
        }

        public AllowRegisterItem GetItemAsync(string id)
        {
            return database.Table<AllowRegisterItem>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItemAsync(AllowRegisterItem item)
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

        public int UpdateItemAsync(AllowRegisterItem item)
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

        public int DeleteItemAsync(AllowRegisterItem item)
        {
            return database.Delete(item);
        }
    }
}
