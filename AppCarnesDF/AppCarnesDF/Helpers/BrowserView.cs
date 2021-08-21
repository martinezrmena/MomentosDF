using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers
{
    public class BrowserView
    {
        public const int TimeOutLimit = 10;

        public async Task OpenBrowser(Uri uri)
        {
            await Browser.OpenAsync(uri, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#872125"),
                PreferredControlColor = Color.FromHex("#872125")
            });
        }
    }
}
