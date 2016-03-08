using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

using Word = Microsoft.Office.Interop.Word;
using ChudoPechkaLib.Data;

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
                    /*
                    Вам наверное интересно почему такая долгая задержка у этого потока аж если я помню в 10 минут?
                    Всё очень просто, когда я парсил сайт у меня постоянно бомбило по поводу их ущербной разметки, хоть я сам пишу не лучше (за что мне всей душой стыдно)
                    Но именно с ними я понял что надо быть готовым ко всему что они выкинут.
                    По этому парсер работает так на всякий случай, вдруг они совершат опечатку в названии блюда или дате а потом исправят её.
                    */
                    Thread.Sleep(600000);
                    this.Loop();
                }
            }));
            _refreshMenu.IsBackground = true;
            _refreshMenu.Start();
        }

        //Если не работает, попробуйте поставить свечку в церкви
        private void Loop()
        {
            bool isSuccessfulSearch = false;
            string html = this.GetHtml();
            string urlDocMenu = _chudoPechka + this.GetUrlDock(html);//Из Html получаем ссылку
            List<DateTime> dates = new List<DateTime>();
            List<string> menuTexts = new List<string>();
            DateTime minDate = DateTime.Now;
            DateTime maxDate = DateTime.Now;

            List<MenuItem> menuItems = null;
            menuItems = this.GetOther(html);


            Word.Application MSWord = new Word.Application();

            Word.Document Doc = MSWord.Documents.Open(urlDocMenu, ConfirmConversions: true);

            for (int i = 0; i < Doc.Paragraphs.Count; i++)
            {
                string text = Doc.Paragraphs[i + 1].Range.Text.ToString();

                if (Regex.IsMatch(text, @"(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])([- /.](19|20)\d\d)?"))
                {
                    isSuccessfulSearch = true;

                    MatchCollection matchDates = Regex.Matches(text, @"(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])([- /.](19|20)\d\d)?");
                    for (int j = 0; j < matchDates.Count; j++)
                    {
                        if (matchDates[j].ToString().Split('.').Length == 2)
                        {
                            int year = DateTime.Now.Year;
                            if (j == 0)
                                minDate = DateTime.Parse(string.Format("{0}.{1}", matchDates[j], year));
                            else
                                maxDate = DateTime.Parse(string.Format("{0}.{1}", matchDates[j], year));
                        }
                        else if (matchDates[i].ToString().Split('.').Length == 3)//На случай если вдруг укажут год
                        {
                            if (j == 0)
                                minDate = DateTime.Parse(matchDates[j].ToString());
                            else
                                maxDate = DateTime.Parse(matchDates[j].ToString());
                        }
                        else
                            throw new ArgumentException("Неверный формат даты");
                    }

                }
            }

            if (!isSuccessfulSearch)
                throw new InvalidOperationException("дата не найдена в документе");

            while (minDate <= maxDate)//Добавление всех дат в промежутке, они нам потребуются для валидации
            {
                dates.Add(minDate);
                minDate = minDate.AddDays(1);
            }

            if (dates.Count == menuItems.Count)
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    MenuItem tmp = menuItems[i];
                    tmp.Date = dates[i];
                    menuItems[i] = tmp;//Это кажется что элементы коллекции структуры
                }
            }
            else
                throw new InvalidOperationException("Количество допустимых дат и меню не совапают");

            Workdays.GetWorkdays.Days = dates;//учтанавливаем рабочие дни

            string menutext = null;
            int index = 0;//Для подставления цены.
            foreach (Word.Table WTable in Doc.Tables)
            {
                foreach (Word.Row WRow in WTable.Rows)
                {
                    foreach (Word.Cell WCell in WRow.Cells)
                    {
                        string cellVal = WCell.Range.Text.Replace("\r\a", "");

                        if (cellVal.Equals("Наименование") || cellVal.Equals("Выход"))
                            continue;
                        else
                        {
                            switch (cellVal)
                            {
                                case "Понедельник":
                                    {
                                        if (menutext != null)
                                        {
                                            menutext += string.Format("<div><span style=\"font-size: 19px;\">{0:N} руб.</span><br/><span style=\"font-size: 15px;\">({1:N} руб. без первого)</span></div>", menuItems[index].FullPrice, menuItems[index].WithoutFullPrice);
                                            menutext += "</div>";
                                            menuTexts.Add(menutext);
                                        }

                                        menutext = "<div class=\"text\">";
                                        break;
                                    }
                                case "Вторник":
                                    {
                                        if (menutext != null)
                                        {
                                            menutext += string.Format("<div><span style=\"font-size: 19px;\">{0:N} руб.</span><br/><span style=\"font-size: 15px;\">({1:N} руб. без первого)</span></div>", menuItems[index].FullPrice, menuItems[index].WithoutFullPrice);
                                            menutext += "</div>";
                                            menuTexts.Add(menutext);
                                        }

                                        menutext = "<div class=\"text\">";
                                        break;
                                    }
                                case "Среда":
                                    {
                                        if (menutext != null)
                                        {
                                            menutext += string.Format("<div><span style=\"font-size: 19px;\">{0:N} руб.</span><br/><span style=\"font-size: 15px;\">({1:N} руб. без первого)</span></div>", menuItems[index].FullPrice, menuItems[index].WithoutFullPrice); ;
                                            menutext += "</div>";
                                            menuTexts.Add(menutext);
                                            index++;
                                        }

                                        menutext = "<div class=\"text\">";
                                        break;
                                    }
                                case "Четверг":
                                    {
                                        if (menutext != null)
                                        {
                                            menutext += string.Format("<div><span style=\"font-size: 19px;\">{0:N} руб.</span><br/><span style=\"font-size: 15px;\">({1:N} руб. без первого)</span></div>", menuItems[index].FullPrice, menuItems[index].WithoutFullPrice);
                                            menutext += "</div>";
                                            menuTexts.Add(menutext);
                                            index++;
                                        }

                                        menutext = "<div class=\"text\">";
                                        break;
                                    }
                                case "Пятница":
                                    {
                                        if (menutext != null)
                                        {

                                            menutext +=string.Format("<div><span style=\"font-size: 19px;\">{0:N} руб.</span><br/><span style=\"font-size: 15px;\">({1:N} руб. без первого)</span></div>",menuItems[index].FullPrice, menuItems[index].WithoutFullPrice);
                                            menutext += "</div>";
                                            menuTexts.Add(menutext);
                                            index++;
                                        }

                                        menutext = "<div class=\"text\">";
                                        break;
                                    }
                                default:
                                    {
                                        if (Regex.IsMatch(cellVal, @"[0-9]+?\s*?гр"))
                                            menutext += string.Format("{0}</span><br/><br/>", cellVal);
                                        else
                                        {
                                            cellVal = Regex.Replace(cellVal, @"/(.|\s)+?/", "");
                                            menutext += "<span>" + cellVal + ", ";
                                        }
                                        
                                        break;
                                    }
                            }
                        }                      
                    }
                }
            }
            menutext += string.Format("<div><span style=\"font-size: 19px;\">{0:N} руб.</span><br/><span style=\"font-size: 15px;\">({1:N} руб. без первого)</span></div>", menuItems[index].FullPrice, menuItems[index].WithoutFullPrice);
            menuTexts.Add(menutext);//т.к. меню последнего дня не попадает добавим его здесь

            if (menuItems.Count != menuTexts.Count)
                throw new InvalidOperationException("Количество меню и html кода не совпадают");
            else
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    MenuItem tmp = menuItems[i];
                    tmp.Menu = menuTexts[i];
                    menuItems[i] = tmp;
                }

            }
            this._menuItems = menuItems;

            Doc.Application.Quit();//закрываем документ
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
        private string GetUrlDock(string html)
        {
            string pattern = "<a target=\"_blank\" href=\"" + @".+" + "\" class=\"file but\">Скачать меню</a>";
            string Url = Regex.Match(html, pattern).ToString();
            Url = Regex.Match(Url, "href=\"" + @".+?" + "\"").ToString().Replace("href=", "").Replace("\"", "");
            return Url;
        }

        private List<MenuItem> GetOther(string html)
        {
            List<XmlNode> Days = new List<XmlNode>(5);
            string xml = this.GetMenu(html);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode ul = doc.ChildNodes[1];

            foreach (XmlNode item in ul.ChildNodes)
            {
                XmlNode img = item.ChildNodes[0];
                XmlAttribute src = img.Attributes["src"];
                src.Value = _chudoPechka + src.Value;
                Days.Add(item);
            }

            return this.GetPrice(Days);
        }

        private List<MenuItem> GetPrice(List<XmlNode> nodes)
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

                MenuItems.Add(itemMenu);
            }
            return MenuItems;
        }

        private string GetMenu(string html)
        {
            //Они отвечают за найденные элементы, чтобы они не повторялись
            bool monday = false;
            bool tuesday = false;
            bool wednesday = false;
            bool thursday = false;
            bool friday = false;

            string ul = Regex.Match(html, "<ul id=\"issues\">" + @"(.|\s)+?" + "</ul>").ToString();//Убеждаемся что мы взяли нужный ul, а то кто знает какие ещё у них извращения в голове появятся
            MatchCollection li = Regex.Matches(ul, "<li" + @"(.|\s)+?>" + @"(.|\s)+?" + @"(.|\s)+?</li>");//Берём все li из ul. Их аж 50, Карл!
            ul = "<?xml version=\"1.0\"?><ul>";
            foreach (Match item in li)
                if (Regex.IsMatch(item.ToString(), @"[0-9]+?(\s)*гр"))//Находим все элементы в которых указан вес, если есть вес значит этот элемент содержит какое-то блюдо, да хоть хлеб
                    if (item.ToString().Contains("id=\"Понедельник\"") && !monday)
                    {
                        monday = true;
                        ul += item.ToString();
                    }
                    else if (item.ToString().Contains("id=\"Вторник\"") && !tuesday)
                    {
                        tuesday = true;
                        ul += item.ToString();
                    }
                    else if (item.ToString().Contains("id=\"Среда\"") && !wednesday)
                    {
                        wednesday = true;
                        ul += item.ToString();
                    }
                    else if (item.ToString().Contains("id=\"Четверг\"") && !thursday)
                    {
                        thursday = true;
                        ul += item.ToString();
                    }
                    else if (item.ToString().Contains("id=\"Пятница\"") && !friday)
                    {
                        friday = true;
                        ul += item.ToString();
                    }
                    else
                        break;//Возможно все элементы найдены

            ul += "</ul>";

            ul = ul.Replace("&nbsp", "THIS_IS");

            return ul;
        }
    }
}
