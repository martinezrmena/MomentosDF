using Android.Content;
using AppCarnesDF.Droid.Helpers.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButton))]
namespace AppCarnesDF.Droid.Helpers.CustomRender
{
    public class CustomButton : ButtonRenderer
    {
        public CustomButton(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {

            }
        }
    }
}