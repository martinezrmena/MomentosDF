using AppCarnesDF.Helpers.CustomRender;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.Behaviors
{
    /// <summary>
    /// Clase encargada de aplicar un behavior a un Picker
    /// utilizado para poder marcar desmarcar posibles errores,
    /// si se ha seleccionado un valor permitido
    /// </summary>
    public class PickerSetBehavior : Behavior<ExtendedPicker>
    {
        protected override void OnAttachedTo(ExtendedPicker bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.SelectedIndexChanged += OnSelected;
        }

        protected override void OnDetachingFrom(ExtendedPicker bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.SelectedIndexChanged -= OnSelected;
        }

        private void OnSelected(object sender, EventArgs e)
        {
            var picker = (ExtendedPicker)sender;

            //picker.TitleColor = Color.Black;

            picker.IsBorderErrorVisible = false;

            picker.IsSelected = true;

        }

    }
}