using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChudoPechkaLib.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid();
            this.Groups = new List<Group>();
        }
        [Key]
        [ForeignKey("Author")]
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string FirsName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; }
        public string ResponseQuestion { get; set; }
        public string SecretQuestion { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Author Author { get; set; }

        public static explicit operator Author(User usr)
        {
            return new Author()
            {
                Id = usr.Id
            };
        }
    }
}
