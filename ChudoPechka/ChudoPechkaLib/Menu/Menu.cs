using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

namespace ChudoPechkaLib.Menu
{
    class Menu
    {
        public static Menu Field
        {
            get
            {
                if (_field == null)
                    _field = new Menu();
                return _field;
            }
        }
        private static Menu _field;
        public List<MenuItem> MenuItems { get; set; }

        private Thread _refreshMenu;
        private const string _chudoPechka = "http://chudo-pechka.by/";
        private Menu()
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
                html = wc.DownloadString(_chudoPechka);
            }
            return html;
        }

        private void ValidationMenu(string html)
        {
            List<XmlNode> ValidDays = new List<XmlNode>(3);
            string xml = this.GetXmlMenu(html);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode ul = doc.ChildNodes[1];

            for (int i = 0; i < 5; i++)
            {
                XmlNode menuItem = ul.ChildNodes[i];

                if (this.IsContainMenu(menuItem))
                    ValidDays.Add(menuItem);

            }

            foreach (XmlNode item in ValidDays)
            {
                XmlNode img = item.ChildNodes[0];
                XmlAttribute src = img.Attributes["src"];
                src.Value = _chudoPechka + src.Value;
            }

            this.SetMenu(ValidDays);
        }

        private void SetMenu(List<XmlNode> nodes)
        {
            foreach (XmlNode node in nodes)
            {
                MenuItem itemMenu = new MenuItem();

                itemMenu.Day = node.Attributes["id"].InnerText;
                itemMenu.Img = node.FirstChild.OuterXml;
                itemMenu.Menu = node.LastChild.OuterXml
                    .Replace("THIS_IS", "&nbsp")
                    .Replace("<div class=\"but but-zakaz-menu\">Заказать обед</div>", "");
                this.MenuItems.Add(itemMenu);
            }

        }

        private string GetXmlMenu(string html)
        {
            string ul = Regex.Match(html, "<ul id=\"issues\">" + @"(.|\s)+?" + "</ul>").ToString();
            ul = "<?xml version=\"1.0\" encoding=\"utf - 8\" ?>\r\n" + ul;
            ul = ul.Replace("&nbsp", "THIS_IS");
            return ul;
        }

        private bool IsContainMenu(XmlNode menuItem)
        {
            string a = menuItem.ChildNodes[1].InnerXml;

            if (string.IsNullOrEmpty(a) || string.Equals(a, "<span style=\"font-size: 17px;\">THIS_IS;</span>"))
                return false;

            return true;

        }

    }
}
