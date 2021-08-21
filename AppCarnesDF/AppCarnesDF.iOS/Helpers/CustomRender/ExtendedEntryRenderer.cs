using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using AppCarnesDF.Helpers.CustomRender;
using AppCarnesDF.iOS.Helpers.CustomRender;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace AppCarnesDF.iOS.Helpers.CustomRender
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || this.Element == null) return;

            this.Control.BackgroundColor = UIColor.FromRGB(249, 249, 249);

            this.Control.Layer.BackgroundColor = UIColor.FromRGB(249, 249, 249).CGColor;

            if (e.PropertyName == ExtendedEntry.IsBorderErrorVisibleProperty.PropertyName)
            {
                if (((ExtendedEntry)this.Element).IsBorderErrorVisible)
                {
                    this.Control.Layer.BorderColor = ((ExtendedEntry)this.Element).BorderErrorColor.ToCGColor();
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