using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Data;

namespace ChudoPechkaLib.Data.DataAnnotations
{
    public class DuplicateLoginAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            using (UsersStoreDB db = new UsersStoreDB())
                return !db.HasUser(value.ToString());
        }
    }
}
