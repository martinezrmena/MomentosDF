using AppCarnesDF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Models
{
    public class MessageAttributes: BaseViewModel
    {
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        private string buttontext;
        public string ButtonText
        {
            get { return buttontext; }
            set
            {
                SetProperty(ref buttontext, value);
            }
        }

        private string cancelbuttontext;
        public string CancelButtonText
        {
            get { return cancelbuttontext; }
            set
            {
                SetProperty(ref cancelbuttontext, value);
            }
        }

    }
}
