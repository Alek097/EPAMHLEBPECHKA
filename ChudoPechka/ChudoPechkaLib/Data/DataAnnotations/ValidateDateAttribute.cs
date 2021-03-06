﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using ChudoPechkaLib.Menu;

namespace ChudoPechkaLib.Data.DataAnnotations
{
    public class ValidateDateAttribute : ValidationAttribute
    {
        private static TimeSpan _validTime = new TimeSpan(17, 0, 0);
        private static Workdays _workdays = Workdays.GetWorkdays;

        public override bool IsValid(object value)
        {
            DateTime date = DateTime.Now.Date;
            int dayNum = 0;
            TimeSpan now = date.TimeOfDay;
            date = date.Date;

            if (value is int)
            {
                dayNum = (int)value;
                DayOfWeek orderDay = GetDay(dayNum);
                DayOfWeek today = date.DayOfWeek;

                if (today == DayOfWeek.Friday)//В пятницу нельзя заказывать так как невозможно угадать когда у них меняется меню, а сделать это я уже не успеваю
                    return false;
                else if (orderDay == today)
                    return false;
                else if (orderDay > today && orderDay != DayOfWeek.Saturday)
                {
                    if (orderDay == today + 1 && !(now < _validTime))
                        return false;
                    return this.IsWorkDay(date, orderDay);
                }
                else if (orderDay < today && orderDay != DayOfWeek.Sunday && orderDay != DayOfWeek.Saturday)// здесь мы смотрим чьто возможно день закза - не рабочий
                {
                    return this.IsWorkDay(date, orderDay);
                }
                else
                    return false;
            }
            else if (value is DateTime)
            {
                DateTime val = (DateTime)value;
                DayOfWeek nowDay = date.DayOfWeek;
                TimeSpan nowTime = date.TimeOfDay;
                if (nowDay == DayOfWeek.Friday)
                    return false;
                if (val > date && val.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (val == date.AddDays(1) && !(now < _validTime))
                        return false;
                    return this.IsWorkDay(date, val.DayOfWeek);
                }
                else
                    return false;

            }
            else
                return false;
        }

        private DayOfWeek GetDay(int dayNum)
        {
            DayOfWeek day;
            switch (dayNum)
            {
                case 1:
                    day = DayOfWeek.Monday;
                    break;
                case 2:
                    day = DayOfWeek.Tuesday;
                    break;
                case 3:
                    day = DayOfWeek.Wednesday;
                    break;
                case 4:
                    day = DayOfWeek.Thursday;
                    break;
                case 5:
                    day = DayOfWeek.Friday;
                    break;
                default:
                    throw new IndexOutOfRangeException("Разрешено заказывать с Пн по Пт");
            }
            return day;
        }
        private bool IsWorkDay(DateTime today, DayOfWeek orderDay)
        {
            while (today.DayOfWeek != orderDay)
                today = today.AddDays(1);
            if (_workdays.Days.Contains(today))
                return true;
            else
                return false;

        }

    }
}
