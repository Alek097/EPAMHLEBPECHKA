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
        public string Type { get; set; }
        public DateTime Day { get; set; }
        public int Price { get; set; }
        public bool IsOrdered { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public Order()
        {
            this.Id = Guid.NewGuid();
            this.Groups = new List<Group>();
        }
    }
}
