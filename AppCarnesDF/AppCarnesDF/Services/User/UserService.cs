using AppCarnesDF.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AppCarnesDF.Services.User
{
    public class UserService
    {
        public ObservableCollection<UserModel> usuarios { get; set; }
        public UserDataBase db { get; set; }

        public UserService()
        {
            if (db == null)
            {
                db = new UserDataBase();
            };

            if (usuarios == null)
            {
                usuarios = new ObservableCollection<UserModel>();
                TraerLista();
            }
        }

        public ObservableCollection<UserModel> Consultar()
        {
            return usuarios;
        }

        private UserModel ConvertirDataBaseAModelo(UserItem data)
        {
            UserModel modelo = new UserModel()
            {
                //Id = data.Id,
                Nombre = data.Nombre,
                Apellido = data.Apellido
            };

            return modelo;
        }

        private UserItem ConvertirModeloADataBase(UserModel model)
        {
            UserItem data = new UserItem()
            {
                //Id = model.Id,
                Nombre = model.Nombre,
                Apellido = model.Apellido
            };

            return data;
        }

        private void TraerLista()
        {
            usuarios.Clear();
            List<UserItem> Lista = db.GetItemsAsync();
            foreach (var item in Lista)
            {
                usuarios.Add(ConvertirDataBaseAModelo(item));
            }
        }

        public int Guardar(UserModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.SaveItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Modificar(UserModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.UpdateItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Eliminar(string IdPersona)
        {
            var Data = db.GetItemAsync(IdPersona);
            int Resultados = db.DeleteItemAsync(Data);
            TraerLista();
            return Resultados;
        }
    }
}