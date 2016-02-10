using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Data.DataAnnotations
{
    public class ContainLoginAttribute : ValidationAttribute
    {
        public ContainLoginAttribute()
        {
            ErrorMessage = "Пользователь не найден";
        }
        public override bool IsValid(object value)
        {
            string login = (string)value;
            using (StoreDB db = new StoreDB())
                return db.IsContainUser(login);
        }
    }
}
