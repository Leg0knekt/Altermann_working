using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRedactorPages
{
    public class ClStudent
    {
        public ClStudent(string id, string surname, string name, string patronymic, string group)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Group = group;
        }
        public string ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Group { get; set; }
    }
}
