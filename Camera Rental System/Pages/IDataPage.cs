using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera_Rental_System.Pages
{
    public interface IDataPage
    {
        event EventHandler<(string Name, object Data)> DataTransmission;
    }
}
