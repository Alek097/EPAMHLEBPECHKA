using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public int Type { get; set; }
        public int Day { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }
    }
}
