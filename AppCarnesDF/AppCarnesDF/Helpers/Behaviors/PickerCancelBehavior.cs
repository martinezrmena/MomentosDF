using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de aplicar un behavior a un Picker
    /// utilizado para poder marcar como error el contenido del mismo
    /// segun su validación
    /// </summary>
    public class PickerCancelBehavior : Behavior<ExtendedPicker>
    {
        protected override void OnAttachedTo(ExtendedPicker bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.Focused += OnFocus;
        }

        protected override void OnDetachingFrom(ExtendedPicker bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.Focused -= OnFocus;
        }

        private void OnFocus(object sender, FocusEventArgs e)
        {
            var picker = (ExtendedPicker)sender;

            if (!picker.IsSelected)
            {
                //picker.TitleColor = Color.Red;

                picker.IsBorderErrorVisible = true;
            }
        }

    }
}