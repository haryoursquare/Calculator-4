using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Text;
using Java.Util.Regex;

namespace Calculator
{
    [Activity(Label = "Calculator", MainLauncher = true, Icon = "@drawable/calc_Icon", Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {
        private TextView calculatorText;
        private string display = "";
        private string[] numbers = new string[2];
        private string @operator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetStatusBarColor(Android.Graphics.Color.Purple);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.layout2);

            calculatorText = FindViewById<TextView>(Resource.Id.textView);
            calculatorText.SetText(display.ToString(), null);

        }
        private void updateScreen()
        {
            calculatorText.SetText(display.ToString(), null);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
                AddDigitOrDecimalPoint(button.Text);
            else if ("÷×+-".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else
                Erase();
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;
            if (value == "." && numbers[index].Contains("."))
                return;

            numbers[index] += value;
            UpdateCalculatorText();
        }
        private void UpdateCalculatorText() => calculatorText.Text = $"{numbers[0]} {@operator} {numbers[1]}";

        private void AddOperator(string value)
        {
            if(numbers[1] != null)
            {
                Calculate(value);
                return;
            }
            @operator = value;

            UpdateCalculatorText();
        }
        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);
            
            switch (@operator)
            {
                case "÷":             
                    result = first / second;
                    
                    break;
                case "×":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
            }
            if (result != null)
            {
                //numbers[0] = result.ToString(); return result to string
                numbers[0] = Math.Round(double.Parse(result.ToString()), 2).ToString(); // return result to 2 decimal places
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalculatorText();
            }
           
        }

        private void Erase()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }
    }
}

