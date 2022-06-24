using Emgu.CV;
using Emgu.CV.Structure;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl, IPage
    {
        private AI.FaceDetector detector;
        public LoginPage()
        {
            InitializeComponent();

            detector = new AI.FaceDetector(ref WebcamControl);
            detector.StartRecognizing();
            detector.FoundFace += FaceFound;
        }

        private async void FaceFound(object sender, Image<Bgr, byte> e)
        {
            Image<Bgr, byte> sized = e.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);

            var trainer = new AI.Training("./Faces");

            try
            {
                trainer.StartTraning();
            }
            catch (Exception ex)
            {
                PersonName.Dispatcher.Invoke(() =>
                {
                    PersonName.Text = $"{ex.Message}";
                    SuccessDialog.IsOpen = true;
                });
                return;
            }

            var result = trainer.Predict(sized);
            NegativeResult.Dispatcher.Invoke(() =>
            {
                NegativeResult.Visibility = Visibility.Visible;
                NegativeResult.Text = $"Label: {result.Label}, Distance: {result.Distance}";
            });

            return;

            if (result.Label > -1)
            {
                PersonName.Dispatcher.Invoke(() =>
                {
                    RegisterInstead.Content = "OK";
                    RegisterInstead.Click -= RegisterClicked;
                    RegisterInstead.Click += (p, o) => PageChanged?.Invoke(this, new HomeProducts());

                    PersonName.Text = $"Logged in as {trainer.LabelFromIndex(result.Label)}";
                    SuccessDialog.IsOpen = true;
                });
            }
            else
            {
                //
                //    NegativeResult.Dispatcher.Invoke(() =>
                //    {
                //        NegativeResult.Visibility = Visibility.Visible;
                //    });
            }
        }


        public event EventHandler<IPage> PageChanged;

        private void RegisterClicked(object sender, RoutedEventArgs e) =>
            PageChanged?.Invoke(this, new Register());
    }
}
