using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;

namespace DataEntryDemo.Model
{
    class MyGroupDescription : GroupDescription
    {
        public override object GroupNameFromItem(object item, int level, CultureInfo culture)
        {
            Person person = item as Person;

            if (person.BirthDate == null)
                return "Unknow";

            int year = ((DateTime)person.BirthDate).Year;

            return (year / 100 + 1) + "世纪";
        }
    }
}
