using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChudoPechkaLib.Data.DataAnnotations;
using ChudoPechkaLib.Data;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Models
{
    public class AnnouncedModel
    {
        public Guid From { get; set; }
        [ContainLogin]
        public string To { get; set; }
        public int Type { get; set; }
        public static implicit operator  Announced(AnnouncedModel model)
        {
            User usr;
            using (StoreDB db = new StoreDB())
                usr = db.GetUser(model.To);

            return new Announced
            {
                To = usr,
                From = model.From,
                Type = model.Type
            };
        }
    }
}