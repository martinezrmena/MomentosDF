using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Share;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppCarnesDF.Services.CompartirCodigo
{
    public class ShareDataBase
    {
        readonly SQLiteConnection database;

        public ShareDataBase()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(dbPath);
            database.CreateTable<ShareItem>();
        }

        public List<ShareItem> GetItemsAsync()
        {
            return database.Table<ShareItem>().ToList();
        }

        public ShareItem GetItemAsync(string id)
        {
            return database.Table<ShareItem>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItemAsync(ShareItem item)
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

        public int UpdateItemAsync(ShareItem item)
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

        public int DeleteItemAsync(ShareItem item)
        {
            return database.Delete(item);
        }
    }
}
