using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib;
using ChudoPechkaLib.Models;
using ChudoPechkaLib.Data;

namespace ChudoPechka.Models
{
    public class OrderModel
    {
        public string Day { get; set; }
        public string Type { get; set; }
        public List<SlectedGroup> SelectedGroups { get; set; }

        public static implicit operator Order(OrderModel model)
        {
            IAuthentication auth = DependencyResolver.Current.GetService<IAuthentication>();
            IStoreDB db = DependencyResolver.Current.GetService<IStoreDB>();
            Order ord = new Order();

            ord.Day = model.Day;
            ord.Type = model.Type;
            ord.User = auth.User;

            foreach (SlectedGroup item in model.SelectedGroups)
                if (db.IsContainGroup(item.GroupId))
                {
                    Group inGroup = db.GetGroup(item.GroupId);
                    ord.Groups.Add(inGroup);
                }
            

            return ord;
        }

    }
    public class SlectedGroup
    {
        public bool Selected { get; set; }
        public string GroupName { get; set; }
        public Guid GroupId { get; set; }
    }

}