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
    /// Interaction logic for AskClient.xaml
    /// </summary>
    public partial class AskClient : UserControl, IPage
    {
        public AskClient(long id)
        {
            InitializeComponent();
            Id = id;
        }

        public long Id { get; }

        public event EventHandler<IPage> PageChanged;

        private void SetInformation(object sender, RoutedEventArgs e)
        {//
            Database.DatabaseConnection.AddClient(Id, Name.Text, Address.Text, POB.Text);

            if (!(Database.DatabaseConnection.GetCameras() as IEnumerable<dynamic>).Any())
                PageChanged?.Invoke(this, new Pages.CameraDirectory(Id));
            else
                PageChanged?.Invoke(this, new HomeProducts(Id));
        }
    }
}
