using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechka.Models
{
    public class OrderModel
    {
        public string Day { get; set; }
        [Display(Name ="Полный")]
        public bool Full { get; set; }
        [Display(Name ="Только первое")]
        public bool First { get; set; }
        [Display(Name = "Только второе")]
        public bool Second { get; set; }
    }
}