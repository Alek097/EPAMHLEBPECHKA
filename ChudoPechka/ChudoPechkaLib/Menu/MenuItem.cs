using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Menu
{
    public struct MenuItem
    {
        public string Day { get; set; }
        public string Img { get; set; }
        public string Menu { get; set; }
        public int FullPrice { get; set; }
        public int WithoutFullPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
