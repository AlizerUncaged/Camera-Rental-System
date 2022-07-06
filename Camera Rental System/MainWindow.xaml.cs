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

namespace Camera_Rental_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        bool admin = false;
        public MainWindow()
        {
            Database.DatabaseConnection.Connect();
            Database.DatabaseConnection.InitializeTables();
            InitializeComponent();
            SetPage(new Pages.Register());
            asdasdasd.Visibility = Visibility.Collapsed;

        }
        private void SetPage(Pages.IPage e)
        {
            var incoming = e as UserControl;
            while (PageHandler.Children.Count >= 1)
                PageHandler.Children.RemoveAt(0);

            if (e is Pages.IDataPage dataPage)
                dataPage.DataTransmission += (s, l) =>
                {
                    if (!Data.ContainsKey(l.Name))
                        Data.Add(l.Name, l.Data);
else Data[l.Name] = l.Data;

                    if (l.Name is "loggedIn")
                    {
                        asdasdasd.Visibility = Visibility.Visible;
                        var accId = long.Parse(Data["accountId"].ToString());

                        SetPage(new Pages.CameraDirectory(accId));

                        if (!(Database.DatabaseConnection.GetClients() as IEnumerable<dynamic>).Where(x => (long)x.Id == accId).Any())
                        {
                            SetPage(new Pages.AskClient(accId));
                        }


                        LeftButtons.Visibility =
                            l.Data is bool loggedIn &&
                            loggedIn ? Visibility.Visible : Visibility.Collapsed;
                    }
                    if (l.Name is "isAdmin")
                    {
                        admin = true;
                        AddItems.Visibility =
                        l.Data is bool isAdmin && isAdmin ? Visibility.Visible : Visibility.Collapsed;
                    }


                };

            e.PageChanged += (s, k) => SetPage(k);
            PageHandler.Children.Add(incoming);
        }

        private void Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void ViewHome(object sender, MouseButtonEventArgs e) =>
            SetPage(new Pages.HomeProducts(long.Parse(Data["accountId"].ToString())));

        private void ViewDirectory(object sender, MouseButtonEventArgs e) =>
            SetPage(new Pages.CameraDirectory(long.Parse(Data["accountId"].ToString())));

        private void AddItemsClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Button image = sender as Button;
                ContextMenu contextMenu = image.ContextMenu;
                contextMenu.PlacementTarget = image;
                contextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void AddCamerasClicked(object sender, RoutedEventArgs e)
        {
            if (Data.TryGetValue("isAdmin", out var isAdmin)
                && isAdmin is bool isAdminBool
                && isAdminBool)

                SetPage(new Pages.AddItems());
        }

        private void AddShippingMethod(object sender, RoutedEventArgs e)
        {
            SetPage(new Pages.AddShipping());
        }

        private void ViewOrders(object sender, MouseButtonEventArgs e)
        {
            SetPage(new Pages.ViewOrders(admin, long.Parse(Data["accountId"].ToString())));
        }

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            SetPage(new Pages.Register());
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
