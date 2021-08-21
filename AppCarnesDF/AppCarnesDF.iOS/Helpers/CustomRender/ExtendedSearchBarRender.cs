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

[assembly: ExportRenderer(typeof(ExtenderSearchBar), typeof(ExtendedSearchBarRender))]
namespace AppCarnesDF.iOS.Helpers.CustomRender
{
    public class ExtendedSearchBarRender: SearchBarRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            var searchbar = (UISearchBar)Control;
            if (e.NewElement != null)
            {
                searchbar.Layer.CornerRadius = 8;
                searchbar.Layer.BorderWidth = (float)0;
                //searchbar.Layer.BorderColor = UIColor.FromRGB(159, 160, 159).CGColor;
                searchbar.Layer.BackgroundColor = UIColor.FromRGB(249, 249, 249).CGColor;
                searchbar.SetImageforSearchBarIcon(new UIImage("ic_search"), UISearchBarIcon.Search , UIControlState.Normal);

                searchbar.BackgroundColor = UIColor.FromRGB(249, 249, 249);

                // This is all for styling purposes, so you might have to play around with these values to make them fit your scenario. Or better yet, you could make them configurable properties in the Xamarin.Forms page.
                searchbar.SearchBarStyle = UISearchBarStyle.Minimal;
                searchbar.Translucent = true;
                searchbar.BarStyle = UIBarStyle.BlackTranslucent;
            }
        }
    }
}