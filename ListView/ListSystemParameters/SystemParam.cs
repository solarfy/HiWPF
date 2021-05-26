using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSystemParameters
{
    class SystemParam
    {
        string strName;
        object objValue;

        public string Name
        {
            set => strName = value;
            get => strName;
        }

        public object Value
        {
            set => objValue = value;
            get => objValue;
        }

        public override string ToString()
        {
            return Name + "=" + Value;
        }
    }
}
