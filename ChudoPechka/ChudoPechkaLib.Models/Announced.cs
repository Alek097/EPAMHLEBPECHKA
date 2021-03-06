﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChudoPechkaLib.Models
{
    public class Announced
    {
        public Announced()
        {
            this.Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid From { get; set; }
        public int Type { get; set; }
        public bool IsRead { get; set; }
    }
}
