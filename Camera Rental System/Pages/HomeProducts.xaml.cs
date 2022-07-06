using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for HomeProducts.xaml
    /// </summary>
    public partial class HomeProducts : UserControl, IPage
    {

        private double _price = 0;
        private double price
        {
            get => _price;

            set
            {
                _price = value;
                PriceText.Content = $"Add to Cart - ₱{price}";

            }
        }
        private double camPrice = 0;
        private readonly long account;

        public HomeProducts(long account, long currentId = -1, bool showCameras = true)
        {
            this.DataContext = this;


            dynamic cam = currentId != -1 ?
                (Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).FirstOrDefault(x => x.Id == currentId) :
                (Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).LastOrDefault();

            if ((Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).Any())
            {
                ProductName = cam.Name;
                ProductDescription = cam.Description;
                ProductID = cam.Id;
                ProductSpecs = cam.Specs;
            }
            InitializeComponent();

            if ((Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).Count() <= 0)
            {
                nocams.Visibility = Visibility.Visible;
                return;
            }

            camPrice = cam.Price;
            price = cam.Price;


            dynamic showObjects = showCameras ?
                (Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).OrderBy(x => Utilities.Random.GlobalRandom.Next()).Take(4) :
                (Database.DatabaseConnection.GetAddOns() as IEnumerable<dynamic>).OrderBy(x => Utilities.Random.GlobalRandom.Next()).Take(4);


            foreach (var i in showObjects)
            {
                var addon = showCameras ? (ICameraPanel)new CameraPanel(i.Id, i.Name, i.Price) :
                    new AddOnPanel(i.Id, i.Name, i.Price);

                var addOnUc = addon as UserControl;
                AddOns.Children.Add(addOnUc);

                if (addon is AddOnPanel addOnPanel)
                {
                    addOnPanel.IsPicked += (s, e) =>
                    {
                        price += e ? addOnPanel.PriceValue : -addOnPanel.PriceValue;
                    };
                }
            }


            for (int i = 0; i < cam.Rating; i++)
            {
                Stars.Children.Add(
                        new PackIcon
                        {
                            Kind = PackIconKind.Star,
                            VerticalAlignment = VerticalAlignment.Center,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF8BF58")),
                            Margin = new Thickness(0, 0, 10, 0)
                        }
                    ); ;
            }

            for (int i = 0; i < 5 - cam.Rating; i++)
            {
                Stars.Children.Add(
                        new PackIcon
                        {
                            Kind = PackIconKind.StarOutline,
                            VerticalAlignment = VerticalAlignment.Center,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF8BF58")),
                            Margin = new Thickness(0, 0, 10, 0)
                        }
                    );
            }

            ProductImage.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/Cameras/{ProductID}.png"));

            this.account = account;
        }

        private void AddOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is CameraPanel camPanel)
                PageChanged?.Invoke(this, new HomeProducts(account, camPanel.ProductID, false));
        }

        public long ProductID { get; }
        public string ProductName { get; }
        public string ProductSpecs { get; }
        public string ProductDescription { get; }


        public event EventHandler<IPage> PageChanged;

        private void CameraPanelClicked(object sender, MouseButtonEventArgs e)
        {

        }

        private void OrderClicked(object sender, RoutedEventArgs e)
        {
            List<dynamic> items = new List<dynamic>();
            items.Add(new { Name = ProductName, Price = camPrice });

            foreach (var child in AddOns.Children)
                if (child is AddOnPanel addOnsPanel && addOnsPanel.IsChosen)
                    items.Add(new { Name = addOnsPanel.AddOnName, Price = addOnsPanel.PriceValue });


            PageChanged?.Invoke(this, new OrderItem(items, account));
        }
    }
}
