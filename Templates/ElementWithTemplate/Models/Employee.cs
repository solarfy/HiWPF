using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementWithTemplate.Models
{
    class Employee
    {
        string name;
        string face;    //存储的图片路径
        DateTime birthdate;
        bool lefthanded;

        public Employee()
        {

        }

        public Employee(string name, string face, DateTime birthdate, bool lefthanded)
        {
            Name = name;
            Face = face;
            BirthDate = birthdate;
            LeftHanded = lefthanded;
        }

        public string Name
        {
            set => name = value;
            get => name;
        }

        public string Face
        {
            set => face = value;
            //get => face;
            get => $"/ElementWithTemplate;component/{face}";
        }

        public DateTime BirthDate
        {
            set => birthdate = value;
            get => birthdate;
        }

        public bool LeftHanded
        {
            set => lefthanded = value;
            get => lefthanded;
        }

    }
}
