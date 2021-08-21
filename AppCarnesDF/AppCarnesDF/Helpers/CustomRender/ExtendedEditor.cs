using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCarnesDF.Helpers.CustomRender
{
    public class ExtendedEditor : Editor
    {
        public static readonly BindableProperty IsBorderErrorVisibleProperty =
            BindableProperty.Create(nameof(IsBorderErrorVisible), typeof(bool), typeof(ExtendedEditor), false, BindingMode.TwoWay);

        public bool IsBorderErrorVisible
        {
            get { return (bool)GetValue(IsBorderErrorVisibleProperty); }
            set
            {
                SetValue(IsBorderErrorVisibleProperty, value);
            }
        }

        public static readonly BindableProperty BorderErrorColorProperty =
            BindableProperty.Create(nameof(BorderErrorColor), typeof(Color), typeof(ExtendedEditor), Xamarin.Forms.Color.Transparent, BindingMode.TwoWay);

        public Color BorderErrorColor
        {
            get { return (Color)GetValue(BorderErrorColorProperty); }
            set
            {
                SetValue(BorderErrorColorProperty, value);
            }
        }

        public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(ExtendedEditor), string.Empty);

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set
            {
                SetValue(ErrorTextProperty, value);
            }
        }

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(propertyName: "IsSelected", returnType: typeof(bool), declaringType: typeof(ExtendedEditor), defaultValue: default(bool));
        public bool IsSelected
        {
            get;
            set;
        }
    }
}
