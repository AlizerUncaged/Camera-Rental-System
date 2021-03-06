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
    /// Interaction logic for OrderItem.xaml
    /// </summary>
    public partial class OrderItem : UserControl, IPage
    {
        private readonly long accountId;
        public OrderItem(IEnumerable<dynamic> items, long accountId)
        {
            InitializeComponent();
            Items = items;
            this.accountId = accountId;
            double totalPrice = 0;
            foreach (var item in items)
            {
                AllItems.Text += $"{item.Name}: ₱{item.Price}{Environment.NewLine}";
                totalPrice += item.Price;
            }

            _ShippingServices_.ItemsSource =
                (Database.DatabaseConnection.GetShippingServices() as IEnumerable<dynamic>).Select(x => x.Name);

            if (_ShippingServices_.Items.Count >= 1)
            {
                _ShippingServices_.SelectedIndex = 0;
            }

            TotalPrice.Text = $"Total: ₱{totalPrice}";

            var account = (Database.DatabaseConnection.GetClients() as IEnumerable<dynamic>).FirstOrDefault(x => x.Id == accountId);
            if (account is null)
            {
                PageChanged?.Invoke(this, new AskClient(accountId));
                return;
            }
            Address.Text = $"Delivered To: {account.Address}";
        }

        public IEnumerable<dynamic> Items { get; }

        public event EventHandler<IPage> PageChanged;

        private void ItemOrderClicked(object sender, RoutedEventArgs e)
        {
            var account = (Database.DatabaseConnection.GetClients() as IEnumerable<dynamic>).FirstOrDefault(x => x.Id == accountId);
            foreach (var item in Items)
            {
                Database.DatabaseConnection
                    .AddRentalOrder(item.Name, DateTime.Now, item.Price, _ShippingServices_.SelectedItem as string,
                    account.Name, account.Address);
            }

            Success.IsOpen = true;
        }

        private void DoOrder(object sender, RoutedEventArgs e) =>
            PageChanged?.Invoke(this, new HomeProducts(accountId));
    }
}
