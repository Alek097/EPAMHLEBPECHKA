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
            return false; //TODO проверка на повторяющийся логин
        }
    }
}
