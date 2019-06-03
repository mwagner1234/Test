using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestProject
{
    public partial class SliderPage : ContentPage
    {
        public SliderPage()
        {
            InitializeComponent();
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            int value = Convert.ToInt32(Math.Round(args.NewValue));
            currentValue.Text = "N = " + value.ToString();
            if (value == 12)
            {
                fibonacciNumber.Text = "Fib = 100";
            } else
            {
                fibonacciNumber.Text = "Fib = " + calculateFibo(value).ToString();
            }



            //rotatingLabel.Rotation = value;displayLabel.Text = String.Format("The Slider value is {0}", value);
        }

        int calculateFibo(int number)
        {
            if ((number == 0) || (number == 1))
            {
                return number;
            }
            else
                return calculateFibo(number - 1) + calculateFibo(number - 2);
        }
    }
}
