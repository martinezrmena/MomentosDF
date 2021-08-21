using AppCarnesDF.Models.Notificaciones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AppCarnesDF.Services.Notificaciones
{
    public class NotificacionesService
    {
        public ObservableCollection<NotificacionModel> n_config { get; set; }
        public NotificacionesDataBase db { get; set; }

        public NotificacionesService()
        {
            if (db == null)
            {
                db = new NotificacionesDataBase();
            };

            if (n_config == null)
            {
                n_config = new ObservableCollection<NotificacionModel>();
                TraerLista();
            }
        }

        public ObservableCollection<NotificacionModel> Consultar()
        {
            return n_config;
        }

        public bool ConsultarConfig()
        {
            TraerLista();
            var modelo = n_config.FirstOrDefault();
            bool value = modelo == null ? true : modelo.Activated;

            return value;
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

        public void TraerLista()
        {
            n_config.Clear();
            List<NotificacionItem> Lista = db.GetItemsAsync();
            foreach (var item in Lista)
            {
                n_config.Add(ConvertirDataBaseAModelo(item));
            }
        }

        public int Guardar(NotificacionModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.SaveItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Modificar(NotificacionModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.UpdateItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Eliminar(string IdConfig)
        {
            var Data = db.GetItemAsync(IdConfig);
            int Resultados = db.DeleteItemAsync(Data);
            TraerLista();
            return Resultados;
        }
    }
}
