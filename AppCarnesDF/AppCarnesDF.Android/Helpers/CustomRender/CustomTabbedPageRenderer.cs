using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Internal;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AppCarnesDF.Droid.Helpers.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace AppCarnesDF.Droid.Helpers.CustomRender
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        public CustomTabbedPageRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (ViewGroup != null && ViewGroup.ChildCount > 0)
            {
                BottomNavigationMenuView bottomNavigationMenuView = FindChildOfType<BottomNavigationMenuView>(ViewGroup);

                if (bottomNavigationMenuView != null)
                {
                    bottomNavigationMenuView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityLabeled;
                    if (bottomNavigationMenuView.ChildCount > 0) bottomNavigationMenuView.UpdateMenuView();
                }
            }
        }

        private T FindChildOfType<T>(ViewGroup viewGroup) where T : Android.Views.View
        {
            if (viewGroup == null || viewGroup.ChildCount == 0) return null;

            for (var i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);

                var typedChild = child as T;
                if (typedChild != null) return typedChild;

                if (!(child is ViewGroup)) continue;

                var result = FindChildOfType<T>(child as ViewGroup);

                if (result != null) return result;
            }

            return null;
        }
    }
}