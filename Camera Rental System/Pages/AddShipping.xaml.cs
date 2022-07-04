using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Camera_Rental_System.Pages
{
    /// <summary>
    /// Interaction logic for AddShipping.xaml
    /// </summary>
    public partial class AddShipping : UserControl, IPage
    {
        public AddShipping() =>
            InitializeComponent();


        public event EventHandler<IPage> PageChanged;

        private void AddItem(object sender, RoutedEventArgs e) =>
            Database.DatabaseConnection.InsertShppingMethod(ItemName.Text, Description.Text, Company.Text);

    }
}
