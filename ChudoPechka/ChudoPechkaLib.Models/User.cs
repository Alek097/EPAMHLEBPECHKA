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
            AvatarPath = "/img/Standart/Avatar.jpg";
            Groups = new List<Group>();
        }
        [Key]
        [ForeignKey("Author")]
        public Guid Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string FirsName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string AvatarPath { get; set; }
        [Required]
        public string ResponseQuestion { get; set; }
        [Required]
        public string SecretQuestion { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual Author Author { get; set; }

        public static implicit operator Author(User usr)
        {
            return new Author()
            {
                User = usr
            };
        }
    }
}
