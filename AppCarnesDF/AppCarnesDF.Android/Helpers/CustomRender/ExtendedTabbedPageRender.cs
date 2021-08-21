using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Droid.Helpers.CustomRender;
using AppCarnesDF.Helpers.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(ExtendedTabbedPage), typeof(ExtendedTabbedPageRender))]
namespace AppCarnesDF.Droid.Helpers.CustomRender
{
    public class ExtendedTabbedPageRender : TabbedPageRenderer
    {
        public ExtendedTabbedPageRender(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsHidden")
            {
                BottomNavigationView TabsLayout = null;
                for (int i = 0; i < ChildCount; ++i)
                {
                    Android.Views.View view = (Android.Views.View)GetChildAt(i);
                    if (view is BottomNavigationView)
                        TabsLayout = (BottomNavigationView)view;
                }
                if ((Element as ExtendedTabbedPage).IsHidden)
                {
                    TabsLayout.Visibility = ViewStates.Invisible;
                }
                else
                {
                    TabsLayout.Visibility = ViewStates.Visible;
                }
            }
        }
    }
}