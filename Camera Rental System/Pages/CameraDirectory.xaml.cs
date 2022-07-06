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
    /// Interaction logic for CameraDirectory.xaml
    /// </summary>
    public partial class CameraDirectory : UserControl, IPage
    {
        public CameraDirectory(long account)
        {
            InitializeComponent();

            if ((Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).Count() <= 0)
            {
                nocams.Visibility = Visibility.Visible;
                return;
            }

            dynamic cameras = Database.DatabaseConnection.GetCameras();
            foreach (var camera in cameras)
            {
                var cam = new CameraPanel(camera.Id, camera.Name, camera.Price);
                cam.MouseLeftButtonDown += (s, e) => PageChanged?.Invoke(this, new HomeProducts(account, camera.Id, false));
                Cameras.Children.Add(cam);
            }
        }

        public event EventHandler<IPage> PageChanged;
    }
}
