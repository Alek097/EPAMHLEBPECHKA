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
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? AuthorId { get; set; }
        public virtual Author Author { get; set; }//Автор группы
        public virtual ICollection<User> Users { get; set; }

        public Group(string name, Author author)
        {
            this.Name = name;
            this.AuthorId = author.Id;
            this.Id = Guid.NewGuid();
        }
    }
}
