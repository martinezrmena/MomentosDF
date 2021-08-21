using AppCarnesDF.Services.FontSize;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers
{
    public class ConvertFontSizeBD
    {
        public double GetDefaultFontSize()
        {
            double value = Device.GetNamedSize(NamedSize.Medium, typeof(Label)); ;

            return value;
        }

        public double GetFontSize(int? index)
        {
            double value;

            switch (index)
            {
                case 3:
                    value = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    break;
                case 1:
                    value = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    break;
                default:
                    value = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    break;
            }

            return value;
        }

        public double GetFontSizeCookie(int? index)
        {
            double value;

            switch (index)
            {
                case 3:
                    value = FontSizesValues.LargeCookieSize;
                    break;
                case 1:
                    value = FontSizesValues.SmallCookieSize;
                    break;
                default:
                    value = FontSizesValues.MediumCookieSize;
                    break;
            }

            return value;
        }

        public double GetFontSizeOptima(int? index)
        {
            double value;

            switch (index)
            {
                case 3:
                    value = FontSizesValues.LargeOptimaSize;
                    break;
                case 1:
                    value = FontSizesValues.SmallOptimaSize;
                    break;
                default:
                    value = FontSizesValues.MediumOptimaSize;
                    break;
            }

            return value;
        }

        public double GetFontSize(string Font)
        {
            double value;
            string FontSize = FontSizesValues.MediumSize;

            if (!string.IsNullOrEmpty(Font))
            {
                FontSize = Font;
            }

            switch (FontSize)
            {
                case FontSizesValues.LargeSize:
                    value = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    break;
                case FontSizesValues.SmallSize:
                    value = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    break;
                default:
                    value = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    break;
            }

            return value;
        }

        public int GetFontSizeIndex(string Font) {

            int value;
            string FontSize = FontSizesValues.MediumSize;

            if (!string.IsNullOrEmpty(Font))
            {
                FontSize = Font;
            }

            switch (FontSize)
            {
                case FontSizesValues.LargeSize:
                    value = 3;
                    break;
                case FontSizesValues.SmallSize:
                    value = 1;
                    break;
                default:
                    value = 2;
                    break;
            }

            return value;
        }

        public string GetFontLabel(int index)
        {
            string value = FontSizesValues.MediumSizeE;

            switch (index)
            {
                case 3:
                    value = FontSizesValues.LargeSizeE;
                    break;
                case 1:
                    value = FontSizesValues.SmallSizeE;
                    break;
                default:
                    value = FontSizesValues.MediumSizeE;
                    break;
            }

            return value;
        }
    }
}
