using AppCarnesDF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.FontSizes
{
    public class FontSizeModel: BaseViewModel
    {
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private int font = 2;
        public int Font
        {
            get { return font; }
            set
            {
                font = value;
                OnPropertyChanged();
            }
        }

    }
}
