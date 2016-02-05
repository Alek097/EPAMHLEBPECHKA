using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChudoPechkaLib.Models;

namespace ChudoPechkaLib.Data
{
    public interface IStoreDB : IDisposable
    {
        bool HasUser(string login);
        User GetUser(string login);
        void AddUser(User usr);
    }
}
