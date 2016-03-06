using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib;
using ChudoPechkaLib.Menu;
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
            IMenu menu = DependencyResolver.Current.GetService<IMenu>();
            
            Order ord = new Order();

            ord.Type = model.Type;
            ord.UserId = auth.User.Id;
            ord.Status = "Ожидается";

            ord.Day = DateTime.Now.Date;

            while ((int)(ord.Day.DayOfWeek) != model.Day)
                ord.Day = ord.Day.AddDays(1);

            MenuItem mItem;
            try
            {
                mItem = menu.MenuItems.First(m => m.Date.Equals(ord.Day));
            }
            catch(InvalidOperationException)
            {
                throw new InvalidOperationException("Не найден день с такой датойы");
            }

            if (model.Type.Equals("Полный"))
                ord.Price = mItem.FullPrice;
            else
                ord.Price = mItem.WithoutFullPrice;

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