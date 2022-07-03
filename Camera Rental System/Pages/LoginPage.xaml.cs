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
    public partial class LoginPage : UserControl, IPage, IDataPage
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
                await trainer.StartTraningAsync();
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

            if (result.Label > -1)
            {
                PersonName.Dispatcher.Invoke(() =>
                {
                    RegisterInstead.Content = "OK";
                    RegisterInstead.Click -= PasswordIn;
                    string username = trainer.LabelFromIndex(result.Label);
                    PersonName.Text = $"Logged in as {username}";
                    SuccessDialog.IsOpen = true;

                    RegisterInstead.Click += (p, o) =>
                    {
                        var similarAccounts = Database.DatabaseConnection.GetAccounts().Where(x => x.Name.ToLower().Trim() == username.ToLower().Trim() && x.Password == Password.Text);
                        if (similarAccounts.Any())
                        {
                            DataTransmission?.Invoke(this, ("user", username));
                            DataTransmission?.Invoke(this, ("loggedIn", true));
                            DataTransmission?.Invoke(this, ("isAdmin", similarAccounts.FirstOrDefault().AccountType == 1));
                            PageChanged?.Invoke(this, new CameraDirectory());

                            detector.StopRecognizing();
                        }
                        else BadPassword.Visibility = Visibility.Visible;
                    };
                });
            }
            else
            {

                NegativeResult.Dispatcher.Invoke(() =>
                {
                    NegativeResult.Visibility = Visibility.Visible;
                });
            }
        }


        public event EventHandler<IPage> PageChanged;
        public event EventHandler<(string Name, object Data)> DataTransmission;

        private void PasswordIn(object sender, RoutedEventArgs e)
        {
            PageChanged?.Invoke(this, new Register());
        }
    }
}
