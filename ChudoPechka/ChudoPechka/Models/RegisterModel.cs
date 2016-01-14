using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ChudoPechka.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль.")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не больше 50 символов и не меньше 7.", MinimumLength = 7)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Введите своё имя.")]
        public string FirsName { get; set; }
        [Required(ErrorMessage ="Введите свою фамилию.")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Введите дату своего рождения.")]
        public DateTime Birthday { get; set; }
    }
}