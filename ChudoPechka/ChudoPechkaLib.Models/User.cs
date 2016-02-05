using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Models
{
    public class User
    {
        public User()
        {
            AvatarPath = "/img/Standart/Avatar.jpg";
            Groups = new List<Group>();
            AuthorGroups = new List<Group>();
        }
        [Key]
        public string Login { get; set; }
        public DateTime BirthDay { get; set; } 
        public string FirsName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; }
        public string ResponseQuestion { get; set; }
        public string SecretQuestion { get; set; }
        public virtual ICollection<Group> AuthorGroups { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
