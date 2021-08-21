using AppCarnesDF.Helpers;
using AppCarnesDF.Models.FontSizes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Services.FontSize
{
    public class FontSizeService
    {
        public ObservableCollection<FontSizeModel> fontsizes { get; set; }
        public FontSizeDataBase db { get; set; }

        public FontSizeService()
        {
            if (db == null)
            {
                db = new FontSizeDataBase();
            };

            if (fontsizes == null)
            {
                fontsizes = new ObservableCollection<FontSizeModel>();
                TraerLista();
            }
        }

        public ObservableCollection<FontSizeModel> Consultar()
        {
            return fontsizes;
        }

        public double GetFontSize(int? index)
        {
            double value;

            switch (index)
            {
                case 3:
                    value = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    break;
                case 1:
                    value = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    break;
                default:
                    value = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    break;
            }

            return value;
        }

        public double GetFontSizeCookie(int? index)
        {
            double value;

            switch (index)
            {
                case 3:
                    value = FontSizesValues.LargeCookieSize;
                    break;
                case 1:
                    value = FontSizesValues.SmallCookieSize;
                    break;
                default:
                    value = FontSizesValues.MediumCookieSize;
                    break;
            }

            return value;
        }

        public double GetFontSizeOptima(int? index)
        {
            double value;

            switch (index)
            {
                case 3:
                    value = FontSizesValues.LargeOptimaSize;
                    break;
                case 1:
                    value = FontSizesValues.SmallOptimaSize;
                    break;
                default:
                    value = FontSizesValues.MediumOptimaSize;
                    break;
            }

            return value;
        }

        public double ConsultarFont()
        {
            TraerLista();
            var modelo = fontsizes.FirstOrDefault();
            int value = modelo == null ? 2 : modelo.Font;

            return GetFontSize(value);
        }

        public int ConsultarSize()
        {
            TraerLista();
            var modelo = fontsizes.FirstOrDefault();
            int value = modelo == null ? 2 : modelo.Font;

            return value;
        }

        public double ConsultarFontCookie()
        {
            TraerLista();
            var modelo = fontsizes.FirstOrDefault();
            int value = modelo == null ? 2 : modelo.Font;

            return GetFontSizeCookie(value);
        }

        public double ConsultarFontOptima()
        {
            TraerLista();
            var modelo = fontsizes.FirstOrDefault();
            int value = modelo == null ? 2 : modelo.Font;

            return GetFontSizeOptima(value);
        }


        private FontSizeModel ConvertirDataBaseAModelo(FontSizeItem data)
        {
            FontSizeModel modelo = new FontSizeModel()
            {
                Id = data.Id,
                Font = data.Font
            };

            return modelo;
        }

        private FontSizeItem ConvertirModeloADataBase(FontSizeModel model)
        {
            FontSizeItem data = new FontSizeItem()
            {
                Id = model.Id,
                Font = model.Font
            };

            return data;
        }

        public void TraerLista()
        {
            fontsizes.Clear();
            List<FontSizeItem> Lista = db.GetItemsAsync();
            foreach (var item in Lista)
            {
                fontsizes.Add(ConvertirDataBaseAModelo(item));
            }
        }

        public int Guardar(FontSizeModel modelo)
        {
            var Data = ConvertirModeloADataBase(modelo);
            int resultados = db.SaveItemAsync(Data);
            TraerLista();
            return resultados;
        }

        public int Modificar(FontSizeModel modelo)
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
