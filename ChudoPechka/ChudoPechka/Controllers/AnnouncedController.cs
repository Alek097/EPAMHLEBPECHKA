using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChudoPechka.Models;
using ChudoPechkaLib.Models;

namespace ChudoPechka.Controllers
{
    public class AnnouncedController : ChudoPechka.Controllers.Base.BaseController
    {
        public ActionResult Index()
        {

            /*
                ----------ПАМЯТКА---------
                если page = 50, тогда вернуть первые 50
                если page = 100, вернуть следующие 50
                если page = 150, вернуть вторые следующие 50
                и т.д.
            */
            if (!Auth.IsAuthentication)
                return Redirect(Url.Action("Index", "Home"));
            else
            {
                List<Announced> Anns = Auth.User.Announceds.ToList();

                if (Anns.Count > 50) // Если больше 50 то мы продолжаем, ИНАЧЕ мы возвращаем то что есть
                {
                    List<Announced> newAnn = new List<Announced>(50);
                    int page;
                    if (string.IsNullOrEmpty(Request.Params["page"]))//ЕСЛИ строка NULL или Empty, page принимает значение по умолчанию. ИНАЧЕ берёт какое кесть 
                        page = 50;
                    else
                        page = int.Parse(Request.Params["page"]);

                    if (page % 50.0 != 0)//Проверка на кратность
                        throw new ArgumentException("Недопустимое значение, значение должно быть кратно 50", "page");
                    else
                    {
                        int start = 0;
                        if (Anns.Count < page) //Допустим у нас 130 элементов а запросили 150. Что делать?
                        {
                            int min = Anns.Count;
                            while (min > 0)
                                min -= 50;
                            min += 50;
                            start = Anns.Count - min;
                            page = Anns.Count;
                        }
                        else
                            start = page - 50;

                        for (; start < page; start++)
                            newAnn.Add(Anns[start]);

                    }

                    return View(newAnn);
                }
                else
                    return View(Anns);
            }
        }
        public void SendAnnounced(AnnouncedModel model)
        {
            if (Auth.IsAuthentication)
                Auth.SendAnnounced(model);
        }
    }
}