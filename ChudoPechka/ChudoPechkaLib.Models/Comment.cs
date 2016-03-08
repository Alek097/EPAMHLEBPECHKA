using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public virtual User User { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
