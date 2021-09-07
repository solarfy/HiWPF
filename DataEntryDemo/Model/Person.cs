using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Serialization;
using System.ComponentModel;

namespace DataEntryDemo.Model
{
    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;       //实现该接口，让该类型的property可以当作数据绑定源，但不能作为数据绑定目标

        string strFirstName = "<first name>";
        string strMiddleName = "";
        string strLastName = "<last name>";
        DateTime? dtBirthDate = new DateTime(1800, 1, 1);
        DateTime? dtDeathDate = new DateTime(1900, 12, 31);

        public string FirstName
        {
            set
            {
                strFirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }

            get => strFirstName;
        }

        public string MiddleName
        {
            set
            {
                strMiddleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }

            get => strMiddleName;
        }

        public string LastName
        {
            set
            {
                strLastName = value;
                OnPropertyChanged(nameof(LastName));
            }

            get => strLastName;
        }

        [XmlElement(DataType="date")]       //指定Xml序列化时的数据类型  XML架构参考 http://www.w3.org/XML/Schema
        public DateTime? BirthDate
        {
            set
            {
                dtBirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }

            get => dtBirthDate;
        }

        [XmlElement(DataType ="date")]
        public DateTime? DeathDate
        {
            set
            {
                dtDeathDate = value;
                OnPropertyChanged(nameof(DeathDate));
            }

            get => dtDeathDate;
        }

        protected virtual void OnPropertyChanged(string strPropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
        }
    }
}
