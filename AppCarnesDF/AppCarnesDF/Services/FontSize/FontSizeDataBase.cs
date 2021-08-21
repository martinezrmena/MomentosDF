using AppCarnesDF.Helpers;
using AppCarnesDF.Models.FontSizes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Services.FontSize
{
    public class FontSizeDataBase
    {
        readonly SQLiteConnection database;

        public FontSizeDataBase()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(dbPath);
            database.CreateTable<FontSizeItem>();
        }

        public List<FontSizeItem> GetItemsAsync()
        {
            return database.Table<FontSizeItem>().ToList();
        }

        public FontSizeItem GetItemAsync(string id)
        {
            return database.Table<FontSizeItem>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItemAsync(FontSizeItem item)
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

        public int UpdateItemAsync(FontSizeItem item)
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

        public int DeleteItemAsync(FontSizeItem item)
        {
            return database.Delete(item);
        }
    }
}
