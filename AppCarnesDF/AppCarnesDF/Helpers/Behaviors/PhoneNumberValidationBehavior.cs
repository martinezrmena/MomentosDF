using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de validar que el Entry posea un numero de telefono con
    /// formato correcto
    /// </summary>
    public class PhoneNumberValidationBehavior : Behavior<ExtendedEntry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(PhoneNumberValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(ExtendedEntry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(ExtendedEntry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var campo = (ExtendedEntry)sender;

            IsValid = IsPhoneNumber(campo.Text);

            //campo.TextColor = IsValid ? Color.Black : Color.Red;

            campo.Placeholder = campo.ErrorText;

            campo.IsBorderErrorVisible = IsValid ? false : true;

            //campo.PlaceholderColor = IsValid ? Color.Black : Color.Red;
        }

        public static bool IsPhoneNumber(string number)
        {
            if (number.Length != 8)
            {
                return false;
            }

            return true;
        }
    }
}
