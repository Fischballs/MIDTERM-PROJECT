using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Globalization;
using BudgetTracker.ohaha; 

namespace BudgetTracker
{
    public partial class MainWindow : Window
    {
        private BudgetManager tracker = new BudgetManager();

        public MainWindow()
        {
            InitializeComponent();
            UpdateSummary();
            LoadTransactions();
        }
        // diri mo input
        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                var transaction = new Transaction(
                    txtDescription.Text,
                    amount,
                    (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    txtCategory.Text,
                    dpDate.SelectedDate ?? DateTime.Now
                );

                tracker.AddTransaction(transaction);
                LoadTransactions();
                UpdateSummary();
            }
            else
            {
                MessageBox.Show("Invalid amount entered.");
            }
        }
        //for adding filter
        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string category = txtFilterCategory.Text;
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            lstTransactions.ItemsSource = tracker.GetFilteredTransactions(category, startDate, endDate);
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = lstTransactions.SelectedItems.Cast<Transaction>().ToList();
            if (selectedItems.Any())
            {
                foreach (var transaction in selectedItems)
                {
                    tracker.RemoveTransaction(transaction);
                }
                LoadTransactions();
                UpdateSummary();
            }
        }
       //showing filtered transaction
        private void LoadTransactions()
        {
            lstTransactions.ItemsSource = tracker.GetFilteredTransactions();
        }

        private void UpdateSummary()
        {
            decimal totalIncome = tracker.GetTotalIncome();
            decimal totalExpenses = tracker.GetTotalExpenses();
            decimal netSavings = tracker.GetNetSavings();

            txtTotalIncome.Text = $"Total Income: P {totalIncome}";
            txtTotalExpenses.Text = $"Total Expenses: P {totalExpenses}";
            txtNetSavings.Text = $"Net Savings: P {netSavings}";

        }
        //sa textboxes
        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }
       
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        // para numbers ray ma input
        private void AmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only digits and one optional decimal point
            if (!decimal.TryParse(((TextBox)sender).Text + e.Text, out _))
            {
                e.Handled = true; // block the input
            }
        }

        private void lstTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
        }
    }
}
