 

using System.Diagnostics;
using Microsoft.Maui.Graphics.Text;

namespace AppoMobi.Mobile.Views.Controls.WheelPicker
{

 

    public class WheelPickerCell : SkiaLayout, IInsideWheelStack
    {

        private readonly SkiaLabel _label;

        public WheelPickerCell()
        {
            HorizontalOptions = LayoutOptions.Fill; ;
            Padding = 0;
            UseCache = SkiaCacheType.Operations;

            Children = new List<SkiaControl>()
            {
                new SkiaLayer() //will be "filled" by picker
                {
                    Children =
                    {
                        new SkiaLabel()
                        {
                            HeightRequest = 100,
                            //FontAttributes = FontAttributes.Italic,
                            FontSize = 18,
                            MonoForDigits="8",
                            HorizontalOptions = LayoutOptions.Center,
                            TextColor = Colors.White,
                            VerticalOptions = LayoutOptions.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = DrawTextAlignment.Center
                        }.Assign(out _label)
                    }
                }
            };

            ApplyContext();
        }

        void ApplyContext()
        {
            if (BindingContext is string value)
            {
                _label.Text = value;
            }
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            ApplyContext();
        }

 
        public void OnPositionChanged(float offsetRatio, bool isSelected)
        {
            if (isSelected)
            {
                _label.TextColor = Colors.Orange;
                _label.FontAttributes = FontAttributes.Bold;
            }
            else
            {
                _label.TextColor = Colors.White;
                _label.FontAttributes = FontAttributes.None;
            }
        }
    }
}
