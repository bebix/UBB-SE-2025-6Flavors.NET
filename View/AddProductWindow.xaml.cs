using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WorkoutApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WorkoutApp.View
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            this.InitializeComponent();
        }

        public event Action<IProduct> ProductAdded;
        IProduct newProduct { get; set; }

        private void ProductTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedType = (ProductTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            ClothesPanel.Visibility = Visibility.Collapsed;
            FoodPanel.Visibility = Visibility.Collapsed;

            if (selectedType == "Clothes")
            {
                ClothesPanel.Visibility = Visibility.Visible;
            }
            else if (selectedType == "Food")
            {
                FoodPanel.Visibility = Visibility.Visible;
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string selectedType = (ProductTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedType == "Clothes")
            {
                newProduct = new ClothesProduct
                (
                    id: 0, // you can auto-generate or set later
                    name: NameTextBox.Text,
                    price: double.TryParse(PriceTextBox.Text, out double price) ? price : 0,
                    stock: 0,
                    categoryId: 1,
                    color: ColorsTextBox.Text.Split(',')[0].Trim(),
                    size: SizesTextBox.Text.Split(',')[0].Trim(),
                    description: "",
                    fileUrl: ImageTextBox.Text,
                    isActive: true
                );
            }
            else if (selectedType == "Food")
            {
                // create a FoodProduct instead if you have that class
                newProduct = new FoodProduct
                (
                    id: 0,
                    name: NameTextBox.Text,
                    price: double.TryParse(PriceTextBox.Text, out double price) ? price : 0,
                    stock: 0,
                    categoryId: 2,
                    size: WeightsTextBox.Text.Split(',')[0].Trim(),
                    description: "",
                    fileUrl: ImageTextBox.Text,
                    isActive: true
                );
            }

            ProductAdded?.Invoke(newProduct);
            this.Close();
        }

    }
}
