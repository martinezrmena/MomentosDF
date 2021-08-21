using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    public class CedulaValidationBehavior : Behavior<ExtendedEntry>
    {
        #region Properties
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(CedulaValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        string _placeHolder;
        #endregion

        protected override void OnAttachedTo(ExtendedEntry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);

            _placeHolder = entry.Placeholder;
        }

        protected override void OnDetachingFrom(ExtendedEntry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = (ExtendedEntry)sender;

            string texto = entry.Text;

            //Validando que sea digito
            texto = CheckDigit(texto);

            entry.Text = texto;

            if (entry.Text.Length != 9)
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }

            RenderControls(IsValid, entry);
        }

        private string CheckDigit(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                {
                    texto = texto.Remove(texto.Length - 1);
                }
            }

            return texto;
        }

        void RenderControls(bool Validate, ExtendedEntry campo)
        {
            //campo.TextColor = IsValid ? Color.Red : Color.Black;

            campo.Placeholder = IsValid ? campo.ErrorText : _placeHolder;

            campo.IsBorderErrorVisible = IsValid ? true : false;

            //campo.PlaceholderColor = IsValid ? Color.Red : Color.Black;
        }

    }
}