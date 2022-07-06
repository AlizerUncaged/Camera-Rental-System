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
    /// Interaction logic for UserOrOrder.xaml
    /// </summary>
    public partial class UserOrOrder : UserControl, IPage
    {
        private readonly string camname;
        private readonly string paidamount;
        private readonly string customerName;
        private readonly string shipping;
        private readonly string address;

        public UserOrOrder(string camname, string paidamount, string buttontext, string customerName, string shipping, string address, bool hideButton)
        {
            InitializeComponent();

            if (hideButton) jjjjj.Visibility = Visibility.Collapsed;

            namm.Text = customerName; namm2.Text = camname;
            jjjjj.Content = buttontext;
            this.camname = camname;
            this.paidamount = paidamount;
            this.customerName = customerName;
            this.shipping = shipping;
            this.address = address;
        }

        public event EventHandler<IPage> PageChanged;
        public event EventHandler<string> Transmission;

        private void k(object sender, RoutedEventArgs e)
        {
            Transmission?.Invoke(this, $"Client: {customerName}{Environment.NewLine}" +
                $"Product: {camname}{Environment.NewLine}" +
                $"Shipping: {shipping}{Environment.NewLine}" +
                $"Address: {address}{Environment.NewLine}");
        }
    }
}
