using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCarnesDF.Helpers.CustomRender;
using AppCarnesDF.iOS.Helpers.CustomRender;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRender))]
namespace AppCarnesDF.iOS.Helpers.CustomRender
{
    public class ExtendedEditorRender : EditorRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || this.Element == null) return;

            this.Control.Layer.BackgroundColor = UIColor.FromRGB(249, 249, 249).CGColor;

            if (e.PropertyName == ExtendedEditor.IsBorderErrorVisibleProperty.PropertyName)
            {
                if (((ExtendedEditor)this.Element).IsBorderErrorVisible)
                {
                    this.Control.Layer.BorderColor = ((ExtendedEditor)this.Element).BorderErrorColor.ToCGColor();
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
        }

    }
}