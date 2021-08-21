using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de aplicar un behavior a un DatePicker
    /// utilizado para poder marcar desmarcar posibles errores,
    /// si se ha seleccionado un valor permitido
    /// </summary>
    public class DatePickerSetDateBehavior: Behavior<ExtendedDatePicker>
    {
        protected override void OnAttachedTo(ExtendedDatePicker bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.DateSelected += OnDateSelected;
        }

        protected override void OnDetachingFrom(ExtendedDatePicker bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.DateSelected -= OnDateSelected;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e) {

            var datepicker = (ExtendedDatePicker)sender;

            //datepicker.TextColor = Color.Black;

            datepicker.IsBorderErrorVisible = false;

            datepicker.IsSelected = true;

        }

    }
}
