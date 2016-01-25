using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechka.Models
{
    public class RecoveryModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        public string login { get; set; }
        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не больше 50 символов и не меньше 7.", MinimumLength = 7)]
        public string newPass { get; set; }
        [Required(ErrorMessage ="Введите ответ на секретный вопрос")]
        public string responseQuestion { get; set; }
    }
}