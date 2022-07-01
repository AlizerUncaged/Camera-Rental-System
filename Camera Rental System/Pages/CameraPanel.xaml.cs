using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for CameraPanel.xaml
    /// </summary>
    public partial class CameraPanel : UserControl, ICameraPanel
    {
        private readonly double price;

        public CameraPanel(long id, string name, double price)
        {
            ProductID = id;
            CameraName = name;
            this.price = price;

            // CameraImage = new BitmapImage(new Uri("/Resources/Cam1.png", UriKind.Relative));
            // CameraName = "Canon ";
            this.DataContext = this;

            InitializeComponent();
        }

        public string CameraName { get; }
        public string Price => $"₱{price}";

        public long ProductID { get; }

        public ImageSource CameraImage => new BitmapImage(new Uri($"pack://siteoforigin:,,,/Cameras/{ProductID}.png"));

        public event EventHandler Clicked;

        private void View(object sender, MouseButtonEventArgs e) =>
            Clicked?.Invoke(this, e);
    }
}
