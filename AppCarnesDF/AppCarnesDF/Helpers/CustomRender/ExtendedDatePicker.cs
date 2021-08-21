using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.CustomRender
{
    public class ExtendedDatePicker : DatePicker
    {

        public static readonly BindableProperty IsBorderErrorVisibleProperty =
            BindableProperty.Create(nameof(IsBorderErrorVisible), typeof(bool), typeof(ExtendedEntry), false, BindingMode.TwoWay);

        public bool IsBorderErrorVisible
        {
            get { return (bool)GetValue(IsBorderErrorVisibleProperty); }
            set
            {
                SetValue(IsBorderErrorVisibleProperty, value);
            }
        }

        public static readonly BindableProperty BorderErrorColorProperty =
            BindableProperty.Create(nameof(BorderErrorColor), typeof(Xamarin.Forms.Color), typeof(ExtendedEntry), Xamarin.Forms.Color.Transparent, BindingMode.TwoWay);

        public Xamarin.Forms.Color BorderErrorColor
        {
            get { return (Xamarin.Forms.Color)GetValue(BorderErrorColorProperty); }
            set
            {
                SetValue(BorderErrorColorProperty, value);
            }
        }

        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(ExtendedDatePicker), defaultValue: default(string));
        public string Placeholder
        {
            get;
            set;
        }

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(propertyName: "IsSelected", returnType: typeof(bool), declaringType: typeof(ExtendedDatePicker), defaultValue: default(bool));
        public bool IsSelected
        {
            get;
            set;
        }
    }
}