using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _customNotation = 10;
        private int CurrentNotation
        {
            get
            {
                if ((bool)HEX.IsChecked)
                    return 16;
                if ((bool)DEC.IsChecked)
                    return 10;
                if ((bool)OCT.IsChecked)
                    return 8;
                if ((bool)BIN.IsChecked)
                    return 2;
                return Convert.ToInt32(CustomNotation.Text);
            }
        }

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
                        Result2.Text =
                            Calculator.ArbitraryToArbitrarySystem(Calculator.Get(Expression.Text, CurrentNotation),
                                CurrentNotation, 2);
                        Result8.Text =
                            Calculator.ArbitraryToArbitrarySystem(Calculator.Get(Expression.Text, CurrentNotation),
                                CurrentNotation, 8);
                        Result10.Text = Calculator.ArbitraryToArbitrarySystem(Calculator.Get(Expression.Text, CurrentNotation), CurrentNotation, 10);
                        Result16.Text = Calculator.ArbitraryToArbitrarySystem(Calculator.Get(Expression.Text, CurrentNotation), CurrentNotation, 16);
                        ResultCustom.Text = Calculator.ArbitraryToArbitrarySystem(Calculator.Get(Expression.Text, CurrentNotation), CurrentNotation, Convert.ToInt32(CustomNotation.Text));
                    }
                    catch (Exception ex)
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
            var digits = "0123456789ABCDEF";
            Expression.Text = "";
            try
            {
                foreach (var button in Buttons?.Children.Cast<Button>()
                             .Where(b => digits.Contains(b.Content.ToString())) ?? new List<Button>(0))
                    button.IsEnabled =
                        int.Parse(button.Content.ToString(), NumberStyles.HexNumber) < CurrentNotation;
            }
            catch (Exception _)
            {
                // ignored
            }
        }
    }
}