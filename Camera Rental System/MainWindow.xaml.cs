﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
            SetPage(new Pages.Register());

        }
        private void SetPage(Pages.IPage e)
        {
            var incoming = e as UserControl;
            while (PageHandler.Children.Count >= 1)
                PageHandler.Children.RemoveAt(0);

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
    }
}
