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

        public MainWindow()
        {
            Database.DatabaseConnection.Connect();
            Database.DatabaseConnection.InitializeTables();
            InitializeComponent();
            SetPage(new Pages.Register());

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

                    if (l.Name is "loggedIn" &&
                        l.Data is bool loggedIn &&
                        loggedIn)
                    {
                        LeftButtons.Visibility = Visibility.Visible;
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
            SetPage(new Pages.HomeProducts());

        private void ViewDirectory(object sender, MouseButtonEventArgs e) =>
            SetPage(new Pages.CameraDirectory());
    }
}
