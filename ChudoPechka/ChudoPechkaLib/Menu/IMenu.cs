using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Menu
{
    public interface IMenu
    {
        List<MenuItem> MenuItems { get; set; }
    }
}
