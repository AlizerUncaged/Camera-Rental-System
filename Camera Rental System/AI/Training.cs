﻿using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
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
        private EigenFaceRecognizer recognizer;
        List<string> fileNames = new List<string>();
        private double threshold = 2000;
        public Training(string path)
        {
            this.path = path;
        }

        public FaceRecognizer.PredictionResult Predict(Image<Bgr, byte> image)
        {
            if (recognizer is null)
                throw new InvalidOperationException("You haven't trained any images yet.");


            return recognizer.Predict(image);
        }

        public string LabelFromIndex(int i) => fileNames[i];

        public async Task StartTraningAsync() =>
            await Task.Run(() =>
            {
                var files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

                if (!files.Any())
                    throw new InvalidOperationException("No faces registered yet.");

                List<Mat> trainedMats = new List<Mat>();
                foreach (var file in files)
                {
                    Mat j = new Mat(file);
                    trainedMats.Add(j);
                    fileNames.Add(Path.GetFileNameWithoutExtension(file));
                }

                var labels = Enumerable.Range(0, fileNames.Count).ToArray();

                recognizer = new EigenFaceRecognizer(trainedMats.Count, threshold);
                recognizer.Train(new VectorOfMat(trainedMats.ToArray()),
                    new VectorOfInt(labels));
            });


    }
}
