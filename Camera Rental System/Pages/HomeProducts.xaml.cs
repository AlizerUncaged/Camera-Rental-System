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


            dynamic cam = currentId != -1 ?
                (Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).FirstOrDefault(x => x.Id == currentId) :
                (Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).LastOrDefault();

            ProductName = cam.Name;
            ProductDescription = cam.Description;
            ProductPriceText = $"Add to Cart - ₱{cam.Price}";
            ProductID = cam.Id;
            ProductSpecs = cam.Specs;

            InitializeComponent();

            dynamic showObjects = showCameras ?
                (Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).OrderBy(x => Utilities.Random.GlobalRandom.Next()).Take(4) :
                (Database.DatabaseConnection.GetAddOns() as IEnumerable<dynamic>).OrderBy(x => Utilities.Random.GlobalRandom.Next()).Take(4);


            foreach (var i in showObjects)
            {
                var addon = showCameras ? (ICameraPanel)new CameraPanel(i.Id, i.Name, i.Price) :
                    new AddOnPanel(i.Id, i.Name, i.Price);
                AddOns.Children.Add(addon as UserControl);
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
        }

        private void AddOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is CameraPanel camPanel)
                PageChanged?.Invoke(this, new HomeProducts(camPanel.ProductID, false));
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
