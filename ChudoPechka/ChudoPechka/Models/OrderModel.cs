using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechka.Models
{
    public class OrderModel
    {
        public string Day { get; set; }
        public string Type { get; set; }
        public List<SlectedGroup> SelectedGroups { get; set; }
    }
    public class SlectedGroup
    {
        public bool Selected { get; set; }
        public string GroupName { get; set; }
        public Guid GroupId { get; set; }
    }

}