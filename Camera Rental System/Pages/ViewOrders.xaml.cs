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
    /// Interaction logic for ViewOrders.xaml
    /// </summary>
    public partial class ViewOrders : UserControl, IPage
    {
        public ViewOrders(bool isAdmin, long accid)
        {
            InitializeComponent();

            if (!isAdmin)
            {
                myorders.Text = "Your Orders";
            }

            var orders = Database.DatabaseConnection.GetOrders() as IEnumerable<dynamic>;

            var account = (Database.DatabaseConnection.GetClients() as IEnumerable<dynamic>).FirstOrDefault(x => x.Id == accid);

            if (!isAdmin)
                orders = orders.Where(x => x.Name == account.Name);

            foreach (var order in orders)
            {
                var j = new UserOrOrder(isAdmin ? order.Type :order.Name , $"{order.AmountPaid}", "Check Details", order.Name, order.Shipping, order.Address, !isAdmin);
                j.Transmission += J_Transmission;
                CustomerOrders.Children.Add(j);
            }

        }

        private void J_Transmission(object sender, string e)
        {
            OrderDetails.Text = e;
            Success.IsOpen = true;
        }

        public event EventHandler<IPage> PageChanged;

        private void DoOrder(object sender, RoutedEventArgs e)
        {
            Success.IsOpen = false;
        }
    }
}
