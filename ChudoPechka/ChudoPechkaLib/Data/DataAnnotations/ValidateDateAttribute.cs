using System;
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
      
                if (orderDay == today)
                    return false;
                else if (orderDay > today && orderDay != DayOfWeek.Saturday)
                {
                    if (orderDay == today + 1 && !(now < _validTime))
                        return false;
                    return this.IsWorkDay(date, orderDay, false);
                }
                else if (orderDay < today && orderDay != DayOfWeek.Sunday && orderDay != DayOfWeek.Saturday)// Если пятница то меню поменялось, здесь мы смотрим чьто возможно день закза - не рабочий
                {
                    return this.IsWorkDay(date, orderDay, true);
                }
                else
                    return false;
            }
            else if (value is DateTime)
            {
                DateTime val = (DateTime)value;
                DayOfWeek nowDay = date.DayOfWeek;
                TimeSpan nowTime = date.TimeOfDay;

                if (val > date && val.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (val == date.AddDays(1) && !(now < _validTime))
                        return false;
                    return this.IsWorkDay(date, val.DayOfWeek, true);
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
        private bool IsWorkDay(DateTime today, DayOfWeek orderDay, bool TillFriday)
        {

            if (TillFriday)
            {
                while (today.DayOfWeek != orderDay)
                    today = today.AddDays(1);
                if (_workdays.Days.Contains(today))
                    return true;
                else
                    return false;
            }
            else
            {
                while (today.DayOfWeek != orderDay)
                    today = today.AddDays(-1);
                if (_workdays.Days.Contains(today))
                    return true;
                else
                    return false;
            }
        }

    }
}
