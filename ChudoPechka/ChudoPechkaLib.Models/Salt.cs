using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Models
{
    public class Salt
    {
        [Key]
        public Guid Id { get; set; }
        public string SaltString { get; set; }
    }
}
