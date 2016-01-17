using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Data.Model
{
    internal class CarrierUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDay { get; set; }
        public string SecretQuestion { get; set; }
        public string ResponseQuestion { get; set; }
    }
}
