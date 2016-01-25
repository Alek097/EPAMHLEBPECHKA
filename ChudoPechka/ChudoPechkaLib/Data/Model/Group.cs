using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoPechkaLib.Data.Model
{
    public class Group
    {
        public Group()
        {
            this.Members = new List<User>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Author { get; set; }
        public List<User> Members { get; set; }

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
