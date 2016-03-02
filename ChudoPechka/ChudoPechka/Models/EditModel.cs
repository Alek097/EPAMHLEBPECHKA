using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Models;
using ChudoPechkaLib;
using ChudoPechkaLib.Data.DataAnnotations;

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

        public static implicit operator EditModel(Order model)
        {
            IAuthentication auth = DependencyResolver.Current.GetService<IAuthentication>();
            User usr = auth.User;

            EditModel Emodel = new EditModel();
            Emodel.id = model.Id;
            Emodel.Type = model.Type;
            Emodel.Day = model.Day;
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
    }
}