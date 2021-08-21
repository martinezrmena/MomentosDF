using AppCarnesDF.iOS.Helpers.CustomRender;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.Forms.TabbedPage), typeof(ExtendedTabbedPageRender))]
namespace AppCarnesDF.iOS.Helpers.CustomRender
{
    public class ExtendedTabbedPageRender: TabbedRenderer
    {
        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            var newHeight = TabBar.Frame.Height + 40;
            CoreGraphics.CGRect tabFrame = TabBar.Frame; //self.TabBar is IBOutlet of your TabBar
            tabFrame.Height = newHeight;
            tabFrame.Y = View.Frame.Size.Height - newHeight;
            TabBar.Frame = tabFrame;
            //TabBar.Layer.BackgroundColor

        }
    }
}