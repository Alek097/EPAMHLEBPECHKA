using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Data;
using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data.DataAnnotations
{
    public class DuplicateLoginAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string login = (string)value;

            using (StoreDB db = new StoreDB())
                return !db.IsContainUser(login);
        }
    }
}
