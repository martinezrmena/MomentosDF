using AppCarnesDF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models.Permissions
{
    public class AllowRegisterModel : BaseViewModel
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

        private bool activated = true;
        public bool Activated
        {
            get { return activated; }
            set
            {
                activated = value;
                OnPropertyChanged();
            }
        }
    }
}