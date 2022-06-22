using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Runtime.InteropServices;
using System.IO;

namespace Camera_Rental_System.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl, IPage
    {
        const int TrainingCount = 1;
        public Register()
        {
            this.DataContext = this;
            InitializeComponent();

            detector = new AI.FaceDetector(ref WebcamControl);
            detector.FoundFace += FaceFound;
        }


        List<Image<Bgr, byte>> faces = new List<Image<Bgr, byte>>();

        private void FaceFound(object sender, Image<Bgr, byte> e)
        {
            if (faces.Count() > TrainingCount)
            {
                RegisterDialog.Dispatcher.Invoke(() =>
                {
                    RegisterDialog.IsOpen = true;
                });
            }
            else faces.Add(e);
        }

        private AI.FaceDetector detector;

        public event EventHandler<IPage> PageChanged;

        private void LoginInstead(object sender, RoutedEventArgs e) =>
            PageChanged?.Invoke(this, new LoginPage());

        private async void SetPersonName(object sender, RoutedEventArgs e)
        {
            await new AI.Checkpoint(faces, PersonName.Text).StartTraningAsync();
            PageChanged?.Invoke(this, new LoginPage());

        }

        private void StartCamera(object sender, RoutedEventArgs e)
        {

            detector.StartRecognizing();
        }
    }
}
