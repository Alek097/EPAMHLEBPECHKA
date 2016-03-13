using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechka.Models
{
    public class RecoveryModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не больше 50 символов и не меньше 7.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        public string newPass { get; set; }
        [Required(ErrorMessage ="Введите E-mail")]
        public string E_Mail { get; set; }
        [Required(ErrorMessage = "Повторите пароль")]
        [Display(Name ="Повторить пароль")]
        [Compare("newPass", ErrorMessage ="Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmNewPass { get; set; }
        [Required(ErrorMessage ="Введите код")]
        [Display(Name ="Код")]
        public string Code { get; set; }
    }
}