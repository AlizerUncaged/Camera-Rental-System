using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Camera_Rental_System.Pages
{
    public interface ICameraPanel
    {
        long ProductID { get; }

        ImageSource CameraImage { get; }

        event EventHandler Clicked;
    }
}
