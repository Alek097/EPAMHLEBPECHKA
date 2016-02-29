﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib;
using ChudoPechkaLib.Models;
using ChudoPechkaLib.Data;
using ChudoPechkaLib.Data.DataAnnotations;

namespace ChudoPechka.Models
{
    public class OrderModel
    {
        [Required(ErrorMessage = "Укажите тип заказа")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Выберите день заказа")]
        [ValidateDate(ErrorMessage = "Укажите допустимую дату")]
        public int Day { get; set; }
        [Required(ErrorMessage = "Выберите группы")]
        public List<SlectedGroup> SelectedGroups { get; set; }

        public static implicit operator Order(OrderModel model)
        {
            IAuthentication auth = DependencyResolver.Current.GetService<IAuthentication>();
            IStoreDB db = DependencyResolver.Current.GetService<IStoreDB>();
            Order ord = new Order();

            ord.Type = model.Type;
            ord.UserId = auth.User.Id;

            int addDay = ((int)(model.Day - DayOfWeek.Sunday)) + 1;//т.к. в js отсчёт идёт от нуля а в c# 0 - воскресенье

            ord.Day = /*DateTime.Now.AddDays(addDay).Date*/ new DateTime(2016,3,5).AddDays(addDay).Date;

            if (model.SelectedGroups != null)
            {
                foreach (SlectedGroup item in model.SelectedGroups)
                    if (db.IsContainGroup(item.GroupId) && item.Selected)
                    {
                        Group inGroup = db.GetGroup(item.GroupId);
                        ord.Groups.Add(inGroup);
                    }
            }
            else throw new NullReferenceException("Группа не выбрана");


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