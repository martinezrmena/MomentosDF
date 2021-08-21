using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Droid.Helpers.CustomRender;
using AppCarnesDF.Helpers.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRender))]
namespace AppCarnesDF.Droid.Helpers.CustomRender
{
    public class ExtendedPickerRender : PickerRenderer
    {

        public ExtendedPickerRender(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            UpdateBorders();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            if (e.PropertyName == ExtendedEntry.IsBorderErrorVisibleProperty.PropertyName)
                UpdateBorders();
        }

        void UpdateBorders()
        {
            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            shape.SetCornerRadius(25);
            shape.SetColor(Android.Graphics.Color.Rgb(249, 249, 249));

            if (((ExtendedPicker)this.Element).IsBorderErrorVisible)
            {
                shape.SetStroke(3, ((ExtendedPicker)this.Element).BorderErrorColor.ToAndroid());
            }
            else
            {
                shape.SetStroke(3, Android.Graphics.Color.LightGray);
                this.Control.SetBackground(shape);
            }

            this.Control.SetBackground(shape);
        }

    }
}