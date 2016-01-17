using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChudoPechkaLib.Data.Model
{
    public interface IUser
    {
        string Login { get; set; }
        string Password { get; set; }
        string FirsName { get; set; }
        string SecondName { get; set; }
        DateTime BirthDay { get; set; }
        string SecretQuestion { get; set; }
        string ResponseQuestion { get; set; }
        void Start(HttpContext context);
    }
}
