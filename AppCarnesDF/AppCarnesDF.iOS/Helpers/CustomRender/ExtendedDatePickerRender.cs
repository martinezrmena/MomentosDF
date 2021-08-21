using System;
using AppCarnesDF.Helpers.CustomRender;
using AppCarnesDF.iOS.Helpers.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRender))]
namespace AppCarnesDF.iOS.Helpers.CustomRender
{
    public class ExtendedDatePickerRender : DatePickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || this.Element == null) return;

            this.Control.Layer.BackgroundColor = UIColor.FromRGB(249, 249, 249).CGColor;

            if (e.PropertyName == ExtendedDatePicker.IsBorderErrorVisibleProperty.PropertyName)
            {
                if (((ExtendedDatePicker)this.Element).IsBorderErrorVisible)
                {
                    this.Control.Layer.BorderColor = ((ExtendedDatePicker)this.Element).BorderErrorColor.ToCGColor();
                    this.Control.Layer.BorderWidth = new nfloat(0.8);
                    this.Control.Layer.CornerRadius = 5;
                }
                else
                {
                    this.Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                    this.Control.Layer.CornerRadius = 5;
                    this.Control.Layer.BorderWidth = new nfloat(0.8);
                }

            }

            DateTime? placeholder = ((ExtendedDatePicker)this.Element)?.Date;
            DateTime value = DateTime.Now;

            if (placeholder == null || placeholder.GetValueOrDefault().Date == value.Date)
            {
                Control.TextColor = UIColor.LightGray;
            }
            else
            {
                Control.TextColor = UIColor.Black;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (this.Control == null)
                return;
            var element = e.NewElement as ExtendedDatePicker;
            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                Control.Text = element.Placeholder;
                Control.TextColor = UIColor.LightGray;
            }
            else
            {
                Control.TextColor = UIColor.Black;
            }
            Control.BorderStyle = UITextBorderStyle.RoundedRect;
            Control.Layer.BorderColor = UIColor.LightGray.CGColor;
            Control.Layer.CornerRadius = 5;
            Control.Layer.BorderWidth = 1f;
            Control.AdjustsFontSizeToFitWidth = true;

           Control.ShouldEndEditing += (textField) => {
               var seletedDate = (UITextField)textField;
               var text = seletedDate.Text;
               if (text == element.Placeholder)
               {
                   Control.Text = DateTime.Now.ToString("dd/MM/yyyy");
               }
               
               return true;
           };
        }

        private void OnCanceled(object sender, EventArgs e)
        {
            Control.ResignFirstResponder();
        }

        private void OnDone(object sender, EventArgs e)
        {
            Control.ResignFirstResponder();
        }
    }
}