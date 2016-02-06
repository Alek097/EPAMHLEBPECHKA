using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChudoPechkaLib.Models
{
    public class Order
    {
         [Key]
       public Guid Id { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int Day { get; set; }
        public Guid? GroupId { get; set; }
        public Group Group { get; set; }
        public Guid? UserId{ get; set; }
        public User User { get; set; }
    }
}
