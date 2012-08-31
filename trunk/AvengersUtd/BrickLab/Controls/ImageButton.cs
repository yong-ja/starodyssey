using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace AvengersUtd.BrickLab.Controls
{
    public class ImageButton : ToggleButton
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton),
            new PropertyMetadata(null));

        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value);}
        }

        public static readonly DependencyProperty IsSelectedProperty =
                DependencyProperty.Register("IsSelected", typeof(bool), typeof(ImageButton),
                 new PropertyMetadata(false));

        //public bool IsSelected
        //{
        //    get { return (bool)GetValue(IsSelectedProperty); }
        //    set { SetValue(IsSelectedProperty, value); }
        //}

        //protected override void OnClick()
        //{
        //    base.OnClick();
        //    IsSelected
        //}

    }
}
