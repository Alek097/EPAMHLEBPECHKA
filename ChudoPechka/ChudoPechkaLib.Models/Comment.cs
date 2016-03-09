using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Models
{
    public class Comment : IComparable
    {
        public Comment()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Dish Dish { get; set; }

        public int CompareTo(object obj)
        {
            return Date.CompareTo((obj as Comment).Date);
        }
    }
}
