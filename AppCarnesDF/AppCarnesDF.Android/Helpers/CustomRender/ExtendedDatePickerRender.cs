using System;
using Android.Content;
using Android.Graphics.Drawables;
using AppCarnesDF.Droid.Helpers.CustomRender;
using AppCarnesDF.Helpers.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRender))]
namespace AppCarnesDF.Droid.Helpers.CustomRender
{
    public class ExtendedDatePickerRender : DatePickerRenderer
    {
        public ExtendedDatePickerRender(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            Control.SetTextColor(Android.Graphics.Color.Gray);
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control.SetPadding(20, 0, 0, 0);

            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(25); //increase or decrease to changes the corner look
            gd.SetStroke(3, Android.Graphics.Color.LightGray);

            Control.SetBackground(gd);

            ExtendedDatePicker element = Element as ExtendedDatePicker;
            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                Control.Text = element.Placeholder;
                Control.SetTextColor(Android.Graphics.Color.Gray);
            }
            else
            {
                Control.SetTextColor(Android.Graphics.Color.Black);
            }

            this.Control.TextChanged += (sender, arg) => {
                var selectedDate = arg.Text.ToString();
                if (selectedDate == element.Placeholder)
                {
                    Control.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            };

            UpdateBorders();
        }


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            try
            {
                ExtendedDatePicker element = sender as ExtendedDatePicker;
                DateTime? placeholder = element?.Date;
                DateTime value = DateTime.Now;

                if (placeholder == null || placeholder.GetValueOrDefault().Date == value.Date)
                {
                    Control.SetTextColor(Android.Graphics.Color.Gray);
                }
                else
                {
                    Control.SetTextColor(Android.Graphics.Color.Black);
                }
            }
            catch (Exception)
            {
            }

            if (e.PropertyName == ExtendedEntry.IsBorderErrorVisibleProperty.PropertyName)
                UpdateBorders();
        }

        void UpdateBorders()
        {
            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            shape.SetCornerRadius(25);
            shape.SetColor(Android.Graphics.Color.Rgb(249, 249, 249));

            if (((ExtendedDatePicker)this.Element).IsBorderErrorVisible)
            {
                shape.SetStroke(3, ((ExtendedDatePicker)this.Element).BorderErrorColor.ToAndroid());
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