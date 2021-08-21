using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    class EditorEmptyBehavior : Behavior<ExtendedEditor>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EditorEmptyBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        string _placeHolder;

        protected override void OnAttachedTo(ExtendedEditor editor)
        {
            editor.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(editor);

            _placeHolder = editor.Placeholder;
        }

        protected override void OnDetachingFrom(ExtendedEditor editor)
        {
            editor.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(editor);
        }

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var editor = (ExtendedEditor)sender;

            IsValid = string.IsNullOrEmpty(editor.Text);

            RenderControls(IsValid, editor);
        }

        void RenderControls(bool Validate, ExtendedEditor campo)
        {
            //campo.TextColor = IsValid ? Color.Red : Color.Black;

            campo.Placeholder = IsValid ? campo.ErrorText : _placeHolder;

            campo.IsBorderErrorVisible = IsValid ? true : false;

            campo.PlaceholderColor = IsValid ? Color.Red : Color.Black;
        }

    }
}