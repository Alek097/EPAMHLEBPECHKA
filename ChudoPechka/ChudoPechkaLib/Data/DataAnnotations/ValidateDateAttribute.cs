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
                dayNum = (int)value;
            else
                throw new FormatException("Неверный формат числа");

            DayOfWeek orderDay = GetDay(dayNum);
            DayOfWeek today = date.DayOfWeek;

            TimeSpan now = date.TimeOfDay;

            return orderDay > today && now < _validTime;
        }

        private DayOfWeek GetDay(int dayNum)
        {
            DayOfWeek day;
            switch(dayNum)
            {
                case 0:
                    day = DayOfWeek.Monday;
                    break;
                case 1:
                    day = DayOfWeek.Tuesday;
                    break;
                case 2:
                    day = DayOfWeek.Wednesday;
                    break;
                case 3:
                    day = DayOfWeek.Thursday;
                    break;
                case 4:
                    day = DayOfWeek.Friday;
                    break;
                default:
                    throw new IndexOutOfRangeException("Разрешено заказывать с Пн по Пт");
            }
            return day;
        }

    }
}
