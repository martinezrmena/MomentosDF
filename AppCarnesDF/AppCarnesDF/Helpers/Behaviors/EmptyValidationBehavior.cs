using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de validar que el Entry no se encuentre vacio
    /// </summary>
    public class EmptyValidationBehavior : Behavior<ExtendedEntry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EmptyValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        string _placeHolder;

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

            IsValid = string.IsNullOrEmpty(entry.Text);

            RenderControls(IsValid, entry);
        }

        void RenderControls(bool Validate, ExtendedEntry campo)
        {
            campo.TextColor = IsValid ? Color.Red : Color.Black;

            campo.Placeholder = IsValid ? campo.ErrorText : _placeHolder;

            campo.IsBorderErrorVisible = IsValid ? true : false;

            campo.PlaceholderColor = IsValid ? Color.Red : Color.Black;
        }

    }
}