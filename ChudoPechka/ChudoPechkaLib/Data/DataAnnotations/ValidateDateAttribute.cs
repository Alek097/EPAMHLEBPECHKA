using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChudoPechkaLib.Data.DataAnnotations
{
    public class ValidateDateAttribute : ValidationAttribute
    {
        private static TimeSpan _validTime = new TimeSpan(17, 0, 0);

        public override bool IsValid(object value)
        {
            DateTime date = DateTime.Now;
            int dayNum = 0;

            if (value is int)
            {
                dayNum = (int)value;
                DayOfWeek orderDay = GetDay(dayNum);
                DayOfWeek today = date.DayOfWeek;

                TimeSpan now = date.TimeOfDay;

                if (orderDay > today)
                {
                    if (orderDay == today + 1 && now < _validTime)
                        return true;
                    else if (orderDay == today + 1 && !(now < _validTime))
                        return false;
                    return true;
                }
                else
                    return false;
            }
            else if (value is DateTime)
            {
                DateTime val = (DateTime)value;
                DayOfWeek nowDay = date.DayOfWeek;
                TimeSpan nowTime = date.TimeOfDay;
                if (date < val)
                    return true;
                else if (nowDay == val.DayOfWeek - 1 && nowTime < _validTime)
                    return true;
                else if ((nowDay == DayOfWeek.Saturday || nowDay == DayOfWeek.Sunday) && date < val)
                    return true;
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

    }
}
