using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Camera_Rental_System.AI
{
    public class Training
    {
        private readonly string path;
        private EigenFaceRecognizer recognizer;
        List<string> fileNames = new List<string>();
        private double threshold = 10000;
        public Training(string path)
        {
            this.path = path;
        }

        public FaceRecognizer.PredictionResult Predict(Image<Bgr, byte> image)
        {
            if (recognizer is null)
                throw new InvalidOperationException("You haven't trained any images yet.");

            var j = image.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
            CvInvoke.EqualizeHist(j, j);

            return recognizer.Predict(j);
        }

        public string LabelFromIndex(int i) => fileNames[i];

        public void StartTraning()
        {

            var files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

            if (!files.Any())
                throw new InvalidOperationException("No faces registered yet.");


            VectorOfMat trainedMats = new VectorOfMat();
            VectorOfInt labels = new VectorOfInt();
            int i = 0;
            foreach (var file in files)
            {
                trainedMats.Push(new Image<Gray, byte>(file).Mat);
                fileNames.Add(Path.GetFileNameWithoutExtension(file));
                labels.Push(new[] { i++ });
            }

            recognizer = new EigenFaceRecognizer(trainedMats.Size, threshold);

            recognizer.Train(trainedMats, labels);

        }
    }
}
