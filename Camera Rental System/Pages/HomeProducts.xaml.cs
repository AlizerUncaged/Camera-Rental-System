﻿using MaterialDesignThemes.Wpf;
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
        public HomeProducts(long currentId = -1)
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

            InitializeComponent();

            var cameras = Database.DatabaseConnection.GetCameras().Take(4);

            foreach (var i in cameras)
            {
                var camPanel = new CameraPanel(i.Id, i.Name, i.Price);
                camPanel.MouseDown += CamPanel_MouseDown;
                CameraOns.Children.Add(camPanel);
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
                    ); ;
            }
        }

        private void CamPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is CameraPanel camPanel)
                PageChanged?.Invoke(this, new HomeProducts(camPanel.ProductID));
        }

        public long ProductID { get; }
        public string ProductName { get; }
        public string ProductPriceText { get; }
        public string ProductDescription { get; }

        public ImageSource CameraImage => new BitmapImage(new Uri($"pack://siteoforigin:,,,/Cameras/{ProductID}.png"));

        public event EventHandler<IPage> PageChanged;
    }
}
