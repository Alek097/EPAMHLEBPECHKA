using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

namespace ChudoPechkaLib.Menu
{
    public class Menu : IMenu
    {


        public List<MenuItem> MenuItems
        {
            get
            {
                return this._menuItems;
            }

            set
            {
                throw new System.Data.ReadOnlyException();
            }
        }

        private List<MenuItem> _menuItems = new List<MenuItem>(5);
        private Thread _refreshMenu;
        private const string _chudoPechka = "http://chudo-pechka.by/";
        public Menu()
        {
            this.Loop();
            _refreshMenu = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Thread.Sleep(600000);
                    this.Loop();
                }
            }));
            _refreshMenu.IsBackground = true;
            _refreshMenu.Start();
        }

        private void Loop()
        {
            string html = this.GetHtml();

            this.ValidationMenu(html);

        }
        private string GetHtml()
        {
            string html;
            using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
            {
                html = wc.DownloadString(_chudoPechka);//TODO:Обработать WebExeption
            }
            return html;
        }

        private void ValidationMenu(string html)
        {
            List<XmlNode> ValidDays = new List<XmlNode>(5);
            string xml = this.GetMenu(html);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode ul = doc.ChildNodes[1];

            foreach (XmlNode item in ul.ChildNodes)
            {
                XmlNode img = item.ChildNodes[0];
                XmlAttribute src = img.Attributes["src"];
                src.Value = _chudoPechka + src.Value;
                ValidDays.Add(item);
            }

            this.SetMenu(ValidDays);
        }

        private void SetMenu(List<XmlNode> nodes)
        {
            List<MenuItem> MenuItems = new List<MenuItem>(5);
            foreach (XmlNode node in nodes)
            {

                MenuItem itemMenu = new MenuItem();

                Match FullPrice = Regex.Match(node.InnerText, @"[0-9]* [0-9]* руб.");
                Match WithoutFullPrice = Regex.Match(node.InnerText, @"[0-9]* [0-9]* руб. без первого");

                itemMenu.FullPrice = int.Parse(FullPrice.ToString().Replace("руб.", "").Replace(" ", ""));
                itemMenu.WithoutFullPrice = int.Parse(WithoutFullPrice.ToString().Replace("руб. без первого", "").Replace(" ", ""));
                itemMenu.Day = node.Attributes["id"].InnerText;
                itemMenu.Img = node.FirstChild.OuterXml;
                itemMenu.Menu = node.LastChild.OuterXml
                    .Replace("THIS_IS", "&nbsp");

                itemMenu.Menu = Regex.Replace(itemMenu.Menu, "<div class=\"but but-zakaz-menu\"" + @"(.|\s)*?" + ">Заказать обед</div>", "");

                MenuItems.Add(itemMenu);
            }
            this._menuItems = MenuItems;
        }

        private string GetMenu(string html)
        {
            //Они отвечают за найденные элементы, чтобы они не повторялись
            bool monday = false;
            bool tuesday = false;
            bool wednesday = false;
            bool thursday = false;
            bool friday = false;

            List<DateTime> workdays = new List<DateTime>();

            string ul = Regex.Match(html, "<ul id=\"issues\">" + @"(.|\s)+?" + "</ul>").ToString();//Убеждаемся что мы взяли нужный ul, а то кто знает какие ещё у них извращения в голове появятся
            MatchCollection li = Regex.Matches(ul, "<li" + @"(.|\s)+?>" + @"(.|\s)+?" + @"(.|\s)+?</li>");//Берём все li из ul. Их аж 50, Карл!
            ul = "<?xml version=\"1.0\"?><ul>";
            foreach (Match item in li)
                if (Regex.IsMatch(item.ToString(), @"[0-9]+?(\s)*гр"))//Находим все элементы в которых указан вес, если есть вес значит этот элемент содержит какое-то блюдо, да хоть хлеб
                    if (item.ToString().Contains("id=\"Понедельник\"") && !monday)
                    {
                        monday = true;
                        ul += item.ToString();
                        DateTime workday = DateTime.Now.Date;
                        while (workday.DayOfWeek != DayOfWeek.Monday)
                            workday.AddDays(1);

                        workdays.Add(workday);
                    }
                    else if (item.ToString().Contains("id=\"Вторник\"") && !tuesday)
                    {
                        tuesday = true;
                        ul += item.ToString();

                        DateTime workday = DateTime.Now.Date;
                        while (workday.DayOfWeek != DayOfWeek.Tuesday)
                            workday.AddDays(1);

                        workdays.Add(workday);
                    }
                    else if (item.ToString().Contains("id=\"Среда\"") && !wednesday)
                    {
                        wednesday = true;
                        ul += item.ToString();

                        DateTime workday = DateTime.Now.Date;
                        while (workday.DayOfWeek != DayOfWeek.Wednesday)//добавляем рабочие дни, это пригодится при валидации
                            workday.AddDays(1);

                        workdays.Add(workday);
                    }
                    else if (item.ToString().Contains("id=\"Четверг\"") && !thursday)
                    {
                        thursday = true;
                        ul += item.ToString();

                        DateTime workday = DateTime.Now.Date;
                        while (workday.DayOfWeek != DayOfWeek.Thursday)
                            workday.AddDays(1);

                        workdays.Add(workday);
                    }
                    else if (item.ToString().Contains("id=\"Пятница\"") && !friday)
                    {
                        friday = true;
                        ul += item.ToString();

                        DateTime workday = DateTime.Now.Date;
                        while (workday.DayOfWeek != DayOfWeek.Monday)
                            workday.AddDays(1);

                        workdays.Add(workday);
                    }
                    else
                        break;//Возможно все элементы найдены

            ul += "</ul>";

            ul = ul.Replace("&nbsp", "THIS_IS");

            Workdays.GetWorkdays.Days = workdays;

            return ul;
        }

        private void AddWorkDay(DayOfWeek day)
        {

        }
    }
}
