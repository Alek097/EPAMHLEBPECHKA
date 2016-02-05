using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

using ChudoPechkaLib.Data.DataAnnotations;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        [Display(Name = "Логин")]
        [DuplicateLogin(ErrorMessage ="Логин уже занят.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не больше 50 символов и не меньше 7.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Введите своё имя.")]
        [Display(Name = "Имя")]
        public string FirsName { get; set; }
        [Required(ErrorMessage = "Введите свою фамилию.")]
        [Display(Name ="Фамилия")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Введите дату своего рождения.")]
        [Display(Name ="Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        [Required(ErrorMessage ="Введите секретный вопрос.")]
        [Display(Name ="Секретный вопрос")]
        public string SecretQuestion { get; set; }
        [Required(ErrorMessage = "Введите ответ на вопрос.")]
        [Display(Name = "Ответ на секретный вопрос")]
        public string ResponseQuestion { get; set; }

        public static implicit operator User(RegisterModel model)
        {
            User usr = new User();

            usr.Login = model.Login;
            usr.Password = model.Password;
            usr.FirsName = model.FirsName;
            usr.SecondName = model.SecondName;
            usr.SecretQuestion = model.SecretQuestion;
            usr.BirthDay = model.BirthDay;
            usr.ResponseQuestion = model.ResponseQuestion;
            usr.AvatarPath = string.Empty;

            return usr;
        }
    }
}