using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de aplicar un behavior a un DatePicker
    /// utilizado para poder marcar como error el contenido del mismo
    /// segun su validación
    /// </summary>
    public class DatePickerCanceledBehavior : Behavior<ExtendedDatePicker>
    {
        protected override void OnAttachedTo(ExtendedDatePicker bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.Focused += OnFocus;
        }

        protected override void OnDetachingFrom(ExtendedDatePicker bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.Focused -= OnFocus;
        }

        private void OnFocus(object sender, FocusEventArgs e)
        {
            var datepicker = (ExtendedDatePicker)sender;

            if (!datepicker.IsSelected)
            {
                //datepicker.TextColor = Color.Red;

                datepicker.IsBorderErrorVisible = true;
            }
        }

    }
}