using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Data.Model
{
    public class User
    {
        public User()
        {
            Groups = new List<Group>();
            AuthorGroups = new List<Group>();
        }
        public Guid Id { get; set; }
        public DateTime BirthDay { get; set; }
        public string FirsName { get; set; }
        public string Login { get; set; }
        public string ResponseQuestion { get; set; }
        public string SecondName { get; set; }
        public string SecretQuestion { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; }
        public virtual ICollection<Group> AuthorGroups { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public override bool Equals(object obj)
        {
            return this.Id.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
