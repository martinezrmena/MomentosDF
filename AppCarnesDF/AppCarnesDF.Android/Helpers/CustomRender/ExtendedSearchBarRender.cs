using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Droid.Helpers.CustomRender;
using AppCarnesDF.Helpers.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtenderSearchBar), typeof(ExtendedSearchBarRender))]
namespace AppCarnesDF.Droid.Helpers.CustomRender
{
    public class ExtendedSearchBarRender : SearchBarRenderer
    {
        public ExtendedSearchBarRender(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null) {
                Control.Background = ContextCompat.GetDrawable(Android.App.Application.Context, Resource.Drawable.custom_search_view);
                var searchView = (Control as SearchView);
                var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                if (searchIconId > 0)
                {
                    var searchPlateIcon = searchView.FindViewById(searchIconId);
                    (searchPlateIcon as ImageView).SetColorFilter(Xamarin.Forms.Color.FromHex("#872125").ToAndroid(), PorterDuff.Mode.SrcIn);
                }
            }
        }
    }
}