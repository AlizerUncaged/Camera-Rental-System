using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera_Rental_System.AI
{
    public class Training
    {
        private readonly string path;
        private double threshold = -1;

        public Training(string path)
        {
            this.path = path;
        }

        public async Task StartTraningAsync() =>
            await Task.Run(() =>
            {
                var files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

                List<Mat> trainedMats = new List<Mat>();

                foreach (var file in files)
                {
                    Mat j = new Mat(file);
                    trainedMats.Add(j);
                }

                EigenFaceRecognizer recognizer = new EigenFaceRecognizer(trainedMats.Count, threshold);
                recognizer.Train(new VectorOfMat(trainedMats.ToArray()), new VectorOfInt(Enumerable.Range(1, trainedMats.Count).ToArray()));
            });
    }
}
