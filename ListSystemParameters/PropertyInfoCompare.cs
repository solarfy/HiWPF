using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ListSystemParameters
{
    class PropertyInfoCompare : IComparer<PropertyInfo>
    {
        public int Compare(PropertyInfo x, PropertyInfo y)
        {
            return string.Compare(x.Name, y.Name);  // 1:x>y   0:x=y    -1:x<y  
        }
    }
}
