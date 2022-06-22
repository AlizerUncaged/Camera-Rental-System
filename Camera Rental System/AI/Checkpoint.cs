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
        private const string facesFolder = "./Faces";

        public Checkpoint(List<Image<Bgr, byte>> mats, string name)
        {
            this.mats = mats;

            if (!Directory.Exists(facesFolder)) Directory.CreateDirectory(facesFolder);

            this.dataFolder = Path.Combine(facesFolder, $"{name}.png");

        }

        public async Task<bool> StartTraningAsync() =>
         await Task.Run(() =>
         {
             foreach (var image in mats)
             {
                 Image<Bgr, byte> sized = image.Resize(300, 300, Emgu.CV.CvEnum.Inter.Cubic);
                 sized.Save(dataFolder);
             }
             return true;
         });

    }
}
