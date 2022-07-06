using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Camera_Rental_System.Pages
{
    /// <summary>
    /// Interaction logic for AddItems.xaml
    /// </summary>
    public partial class AddItems : System.Windows.Controls.UserControl, IPage
    {
        public AddItems()
        {
            InitializeComponent();
        }

        public event EventHandler<IPage> PageChanged;

        private string Filename;

        private void FindImages(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ImageFilePath.Text = dlg.FileName;
                    Filename = dlg.FileName;
                }
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            string folder = "Cameras";
            long lastInsert = -1;
            if ((bool)IsCam.IsChecked)
                lastInsert = Database.DatabaseConnection.InsertCamera(
                    ItemName.Text, Description.Text, Specs.Text, double.Parse(Price.Text),
                   Manufacturer.Text, 5);
            else
            {
                folder = "AddOns";
                lastInsert = Database.DatabaseConnection.InsertAddOn(
                 ItemName.Text, Description.Text, Specs.Text, double.Parse(Price.Text),
                Manufacturer.Text);
            }

            if (lastInsert != -1 && !string.IsNullOrWhiteSpace(Filename))
            {
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var cameraImage = $"./{folder}/{lastInsert}.png";
                File.Copy(Filename, cameraImage);
                additembutton.Content = "Add More";
            }
        }
    }
}
