using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Models;

namespace ChudoPechka.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}