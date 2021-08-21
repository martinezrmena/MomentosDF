using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de validar que el Entry posea el formato correcto para la contraseña
    /// </summary>
    public class PasswordValidationBehavior : Behavior<ExtendedEntry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(PasswordValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

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

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = (ExtendedEntry)sender;

            ValidatePassword(entry);

            IsValid = string.IsNullOrEmpty(entry.Text);

            RenderControls(IsValid, entry);
        }

        public bool ValidatePassword(ExtendedEntry entry) {

            string Password = entry.Text;

            //Valida si se ingresan caracteres especiales
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{6,50}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrEmpty(Password))
            {
                entry.ErrorText = "Este es un campo requerido";
                return false;
            }

            if (!hasSymbols.IsMatch(Password))
            {
                entry.ErrorText = "Debe contener como minimo un caracter especial";
                return false;
            }

            //la contraseña debe contener entre 6 y 50 caracteres
            if (!hasMiniMaxChars.IsMatch(Password))
            {
                entry.ErrorText = "La contraseña debe contener entre 6 y 50 caracteres";
                return false;
            }

            //Valida que alguno de los caracteres sea digito
            if (!hasNumber.IsMatch(Password))
            {
                entry.ErrorText = "La contraseña debe contener al menos un número";
                return false;
            }

            //Valida que alguno de los caracteres sea letra
            if (!Password.ToCharArray().Any(char.IsLetter))
            {
                entry.ErrorText = "La contraseña debe contener letras";
                return false;
            }

            return true;
        }

        void RenderControls(bool Validate, ExtendedEntry campo)
        {
            //campo.TextColor = IsValid ? Color.Red : Color.Black;

            campo.IsBorderErrorVisible = IsValid ? true : false;

            campo.Text = IsValid ? string.Empty : campo.Text;

            campo.Placeholder = campo.ErrorText;

            //campo.PlaceholderColor = IsValid ? Color.Red : Color.Black;
        }


    }
}