using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Camera_Rental_System.AI
{
    public class FaceDetector
    {
        private readonly ImageBrush imageSource;

        private VideoCapture capture = new VideoCapture();
        private Image<Bgr, byte> currentFrame;
        private CascadeClassifier faceClassifier = new CascadeClassifier("./Classifiers/haarcascade_frontalface_alt.xml");

        public FaceDetector(ref ImageBrush imageSource)
        {
            this.imageSource = imageSource;
        }


        public void StartRecognizing()
        {
            StartCapture();
        }
        public void StopRecognizing()
        {
            capture.Stop();
        }
        private void StartCapture()
        {
            Mat m = new Mat();

            capture.ImageGrabbed += (s, e) =>
            {
                capture.Retrieve(m);
                currentFrame = m.ToImage<Bgr, byte>();

                // Now try detecting face.
                Mat gray = new Mat();
                CvInvoke.CvtColor(currentFrame, gray, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(gray, gray);

                var faces = faceClassifier.DetectMultiScale(
                    gray,
                    minSize: System.Drawing.Size.Empty, maxSize: System.Drawing.Size.Empty);

                if (faces.Any())
                {
                    foreach (var face in faces)
                    {
                        CvInvoke.Rectangle(currentFrame, face, new Bgr(System.Drawing.Color.Aqua).MCvScalar, 2);

                        // Get only the face.
                        Image<Bgr, byte> resultImage = currentFrame.Convert<Bgr, byte>();
                        resultImage.ROI = face;

                        CvInvoke.CvtColor(resultImage, resultImage, ColorConversion.Bgr2Gray);

                        FoundFace?.Invoke(this, resultImage);
                    }
                }

                imageSource.Dispatcher.Invoke(new Action(() =>
                    imageSource.ImageSource = CreateSource(currentFrame.ToBitmap())));

            };

            capture.Start();
        }

        public event EventHandler<Image<Bgr, byte>> FoundFace;

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource CreateSource(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
               IntPtr.Zero, Int32Rect.Empty,
               System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ip);


            return bs;
        }
    }
}
