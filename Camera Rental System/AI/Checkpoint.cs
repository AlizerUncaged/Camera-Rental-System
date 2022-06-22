using Emgu.CV;
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
    public class Checkpoint
    {
        const int Threshold = 7000;
        private readonly List<Image<Bgr, byte>> mats;
        private readonly string dataFolder;

        public Checkpoint(List<Image<Bgr, byte>> mats, string name)
        {
            this.mats = mats;
            this.dataFolder = $"./Faces/{name}";

            if (!Directory.Exists(this.dataFolder)) Directory.CreateDirectory(this.dataFolder);
        }

        public async Task<bool> StartTraningAsync() =>
         await Task.Run(() =>
         {
             foreach (var image in mats)
             {
                 Image<Bgr, byte> sized = image.Resize(300, 300, Emgu.CV.CvEnum.Inter.Cubic);
                 sized.Save(Path.Combine(dataFolder, $"{mats.IndexOf(image)}.png"));
             }
             //var matCount = mats.Count();
             //EigenFaceRecognizer recognizer = new EigenFaceRecognizer(matCount, Threshold);

             //recognizer.Train(new VectorOfMat(mats.ToArray()),
             //    new VectorOfInt(Enumerable.Range(1, matCount).ToArray()));

             //foreach (var mat in mats)
             //    mat.Save(Path.Combine(dataFolder, $"{mats.IndexOf(mat)}.png"));

             return true;
         });

    }
}
