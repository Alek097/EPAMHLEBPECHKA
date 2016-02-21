using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChudoPechkaLib.Models
{
    public class Group
    {
        public Group()
        {
            this.Id = Guid.NewGuid();
            this.Users = new List<User>();
            this.Authors = new List<Author>();
            this.Orders = new List<Order>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }//Автор группы
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public Group(string name, Author author) : this()
        {
            this.Name = name;
            this.Authors.Add(author);
        }
    }
}
