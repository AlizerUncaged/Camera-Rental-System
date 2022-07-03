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
        public HomeProducts(long currentId = -1, bool showCameras = true)
        {
            this.DataContext = this;

            // aaaaaaaaaaaaaaaaaaaaaaaaaa

            var cam = Database.DatabaseConnection.GetCameras().LastOrDefault();

            if (currentId != -1)
                cam = Database.DatabaseConnection.GetCameras().FirstOrDefault(x => x.Id == currentId);
            else cam = Database.DatabaseConnection.GetCameras().LastOrDefault();

            ProductName = cam.Name;
            ProductDescription = cam.Description;
            ProductPriceText = $"Add to Cart - ₱{cam.Price}";
            ProductID = cam.Id;
            ProductSpecs = cam.Specs;

            InitializeComponent();

            if (showCameras)
            {
                var addons = Database.DatabaseConnection.GetAddOns().OrderBy(x => Utilities.Random.GlobalRandom.Next()).Take(4);

                foreach (var i in addons)
                {
                    var addon = new AddOnPanel(i.Id, i.Name, i.Price);
                    // addon.MouseDown += AddOnMouseDown;
                    AddOns.Children.Add(addon);
                }
            }
            else
            {

                var addons = Database.DatabaseConnection.GetCameras().OrderBy(x => Utilities.Random.GlobalRandom.Next()).Take(4);

                foreach (var i in addons)
                {
                    var addon = new AddOnPanel(i.Id, i.Name, i.Price);
                    // addon.MouseDown += AddOnMouseDown;
                    AddOns.Children.Add(addon);
                }
            }

            for (int i = 0; i < cam.rating; i++)
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

            for (int i = 0; i < 5 - cam.rating; i++)
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
        }

        private void AddOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is CameraPanel camPanel)
                PageChanged?.Invoke(this, new HomeProducts(camPanel.ProductID));
        }

        public long ProductID { get; }
        public string ProductName { get; }
        public string ProductPriceText { get; }
        public string ProductSpecs { get; }
        public string ProductDescription { get; }

        public ImageSource CameraImage => new BitmapImage(new Uri($"pack://siteoforigin:,,,/Cameras/{ProductID}.png"));

        public event EventHandler<IPage> PageChanged;

        private void CameraPanelClicked(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
