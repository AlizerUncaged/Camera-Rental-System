using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera_Rental_System.Pages
{
    public interface IPage
    {
        event EventHandler<IPage> PageChanged;
    }
}
