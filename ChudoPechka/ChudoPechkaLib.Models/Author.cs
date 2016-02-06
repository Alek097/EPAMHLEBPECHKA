using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Models
{
    public class Author
    {
        [Key]
        [ForeignKey("User")]
        public Guid Id { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public Author()
        {
            Groups = new List<Group>();
        }
    }
}
