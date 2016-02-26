﻿using System;
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
            List<MenuItem> MenuItems = new List<MenuItem>(5);
            foreach (XmlNode node in nodes)
            {

                MenuItem itemMenu = new MenuItem();

                Match FullPrice = Regex.Match(node.InnerText, @"[0-9]* [0-9]* руб.");
                Match WithoutFullPrice = Regex.Match(node.InnerText, @"[0-9]* [0-9]* руб. без первого");

                itemMenu.FullPrice = int.Parse(FullPrice.ToString().Replace("руб.", "").Replace(" ",""));
                itemMenu.WithoutFullPrice = int.Parse(WithoutFullPrice.ToString().Replace("руб. без первого", "").Replace(" ", ""));
                itemMenu.Day = node.Attributes["id"].InnerText;
                itemMenu.Img = node.FirstChild.OuterXml;
                itemMenu.Menu = node.LastChild.OuterXml
                    .Replace("THIS_IS", "&nbsp")
                    .Replace("<div class=\"but but-zakaz-menu\">Заказать обед</div>", "");

                MenuItems.Add(itemMenu);
            }
            this._menuItems = MenuItems;
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
