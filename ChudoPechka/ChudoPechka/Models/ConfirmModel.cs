using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Data.DataAnnotations;

namespace ChudoPechka.Models
{
    public class ConfirmModel
    {
        [Required(ErrorMessage = "Отсутствует E-mail")]
        public string E_Mail { get; set; }
        [Required(ErrorMessage = "Отсутствует логин")]
        [ContainLogin(ErrorMessage = "Пользователь не найден")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите код подтверждения")]
        public string Code { get; set; }
    }
}