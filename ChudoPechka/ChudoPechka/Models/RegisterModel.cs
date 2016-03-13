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
        [DuplicateLogin(ErrorMessage = "Логин уже занят.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не больше 50 символов и не меньше 7.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Повторите пароль.")]
        [Display(Name = "Повторить пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Пароли не совпадют")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Введите своё имя.")]
        [Display(Name = "Имя")]
        public string FirsName { get; set; }
        [Required(ErrorMessage = "Введите свою фамилию.")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Введите дату своего рождения.")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        [Required(ErrorMessage = "Введите E-Mail.")]
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string E_Mail { get; set; }

        public static implicit operator User(RegisterModel model)
        {
            User usr = new User();

            usr.Login = model.Login;
            usr.Password = model.Password;
            usr.FirsName = model.FirsName;
            usr.SecondName = model.SecondName;
            usr.BirthDay = model.BirthDay;
            usr.E_Mail = model.E_Mail;
            usr.AvatarPath = string.Empty;

            return usr;
        }
    }
}