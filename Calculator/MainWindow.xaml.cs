using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _customNotation = 10;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalcButtonOnClick(object sender, RoutedEventArgs e)
        {
            
            var content = (sender as Button)?.Content as string;
            switch (content)
            {
                case "Clear":
                    Expression.Text = "";
                    break;
                case "=":
                    try
                    {
                        Result2.Text = Calculator.Get(Expression.Text, 2);
                        Result8.Text = Calculator.Get(Expression.Text, 2);
                        Result10.Text = Calculator.Get(Expression.Text, 2);
                        Result16.Text = Calculator.Get(Expression.Text, 2);
                        ResultCustom.Text = Calculator.Get( Expression.Text, Convert.ToInt32(CustomNotation.Text));
                    }
                    catch (Exception _)
                    {
                        // ignored
                    }
                    break;
                 default:
                     Expression.Text += content;
                     break;
            }

        }

        private void ChangeNotation(object sender, RoutedEventArgs e)
        {
            var maxDigit = (sender as RadioButton)?.Name switch
            {
                "HEX" => 16,
                "DEC" => 10,
                "OCT" => 8,
                "BIN" => 2,
                "CUSTOM" => Convert.ToInt32(CustomNotation.Text),
                _ => throw new InvalidOperationException()
            };
            foreach (var button in Buttons.Children.Cast<Button>())
                button.IsEnabled = int.Parse(button.Name, System.Globalization.NumberStyles.HexNumber) < maxDigit;
        }
    }


    public enum CalculatorEvent
    {
        Operator,
        Operand,
        Clear,
        Count
    }
}