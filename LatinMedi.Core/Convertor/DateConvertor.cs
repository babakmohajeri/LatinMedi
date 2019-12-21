using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LatinMedia.Core.Convertor
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime dateTime)
        {

            PersianCalendar pc = new PersianCalendar();
            string ShamsiDate = pc.GetYear(dateTime).ToString() + "/" +
                                pc.GetMonth(dateTime).ToString("00") + "/" +
                                pc.GetDayOfMonth(dateTime).ToString("00");
               return ShamsiDate;

        }
    }
}
