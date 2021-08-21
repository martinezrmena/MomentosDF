using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Helpers;
using AppCarnesDF.Models.Permissions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AppCarnesDF.Droid.Servicios
{
    public class ConsultAllowRegister
    {
        readonly SQLiteConnection database;
        public ObservableCollection<AllowRegisterModel> n_config { get; set; }

        public ConsultAllowRegister()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = Path.Combine(path, DataBaseConstants.DatabaseName);
            database = new SQLiteConnection(path);
            database.CreateTable<AllowRegisterItem>();
        }

        public bool ConsultValor()
        {
            if (n_config == null)
            {
                n_config = new ObservableCollection<AllowRegisterModel>();
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
            List<AllowRegisterItem> Lista = GetItemsAsync();
            foreach (var item in Lista)
            {
                n_config.Add(ConvertirDataBaseAModelo(item));
            }
        }

        public List<AllowRegisterItem> GetItemsAsync()
        {
            return database.Table<AllowRegisterItem>().ToList();
        }

        private AllowRegisterModel ConvertirDataBaseAModelo(AllowRegisterItem data)
        {
            AllowRegisterModel modelo = new AllowRegisterModel()
            {
                Id = data.Id,
                Activated = data.Activated
            };

            return modelo;
        }

        private AllowRegisterItem ConvertirModeloADataBase(AllowRegisterModel model)
        {
            AllowRegisterItem data = new AllowRegisterItem()
            {
                Id = model.Id,
                Activated = model.Activated
            };

            return data;
        }

        public int SaveItemAsync(bool Activate)
        {
            try
            {
                var values = GetItemsAsync();

                if (values != null && values.Any())
                {
                    //Si ya existe algun elemento guardado
                    var item = values.FirstOrDefault();
                    item.Activated = Activate;
                    return UpdateItemAsync(item);
                }
                else
                {
                    //Si no hay ninguno quiere decir que guardaremos por primera vez
                    AllowRegisterItem allowRegisterItem = new AllowRegisterItem();
                    allowRegisterItem.Id = Guid.NewGuid().ToString();
                    allowRegisterItem.Activated = Activate;
                    return database.Insert(allowRegisterItem);
                }
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

    }
}