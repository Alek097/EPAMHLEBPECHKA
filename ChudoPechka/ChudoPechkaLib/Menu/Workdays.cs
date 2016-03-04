using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Menu
{
    class Workdays
    {
        public static Workdays GetWorkdays
        {
            get
            {
                if (_Workdays == null)
                    _Workdays = new Workdays();
                return _Workdays;
            }
        }
        private static Workdays _Workdays;
        private Workdays()
        {
            Days = new List<DateTime>();
        }
        public List<DateTime> Days { get; set; }
    }
}
