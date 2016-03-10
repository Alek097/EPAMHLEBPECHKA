using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Models;
using ChudoPechkaLib;
using ChudoPechkaLib.Data.DataAnnotations;
using ChudoPechkaLib.Data;

namespace ChudoPechka.Models
{
    public class EditModel
    {
        [Required(ErrorMessage = "Отсутсвует ID заказа")]
        public Guid id { get; set; }
        [Required(ErrorMessage = "Отсутствует тип заказа")]
        public string Type { get; set; }
        [DataType(DataType.Date)]
        [ValidateDate(ErrorMessage = "Введите допустимую дату")]
        [Required(ErrorMessage = "Отсутствует дата")]
        public DateTime Day { get; set; }
        [Required(ErrorMessage = "Выберите группу")]
        public List<SlectedGroup> SelectedGroups { get; set; }
        [Required(ErrorMessage ="Отсутствует цена")]
        public int Price { get; set; }
        [Required(ErrorMessage ="Отсутствует статус")]
        public string Status { get; set; }
        [Required(ErrorMessage ="Отсутсвует владеец заказа")]
        public Guid UserId { get; set; }
        [Required]
        public bool IsOrdered { get; set; }

        public static implicit operator EditModel(Order model)
        {
            IDBManager auth = DependencyResolver.Current.GetService<IDBManager>();
            User usr = auth.User;

            EditModel Emodel = new EditModel();
            Emodel.id = model.Id;
            Emodel.Type = model.Type;
            Emodel.Day = model.Day;
            Emodel.Price = model.Price;
            Emodel.UserId = (Guid)model.UserId;
            Emodel.Status = model.Status;
            Emodel.IsOrdered = model.IsOrdered;
            Emodel.SelectedGroups = new List<SlectedGroup>();

            List<Group> groups = new List<Group>();
            groups.AddRange(usr.AdministartionGroups);
            groups.AddRange(usr.Groups);
            groups.Sort();

            foreach (Group grp in groups)
            {
                SlectedGroup tmp = new SlectedGroup();
                tmp.GroupId = grp.Id;
                tmp.GroupName = grp.Name;
                tmp.Selected = model.Groups.FirstOrDefault(g => g.Id.Equals(grp.Id)) != null;//Выбрана ли эта группа             
                Emodel.SelectedGroups.Add(tmp);
            }

            return Emodel;
        }
        public static implicit operator Order(EditModel model)
        {
            IStoreDB db = DependencyResolver.Current.GetService<IStoreDB>();

            Order ord = new Order();
            ord.Id = model.id;
            ord.Price = model.Price;
            ord.Status = model.Status;
            ord.Type = model.Type;
            ord.UserId = model.UserId;
            ord.IsOrdered = model.IsOrdered;
            ord.Day = model.Day;
            ord.Groups = new List<Group>();

            foreach (SlectedGroup item in model.SelectedGroups)
            {
                if (item.Selected && db.IsContainGroup(item.GroupId))
                    ord.Groups.Add(db.GetGroup(item.GroupId));
            }
            return ord;
        }
    }
}