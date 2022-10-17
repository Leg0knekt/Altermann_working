using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRedactorPages
{
    public class ClStudent
    {
        public ClStudent(string id, string surname, string name, string patronymic, string group, int groupId)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            GroupNumber = group;
            GroupID = groupId;
        }
        public string ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string GroupNumber { get; set; }
        public int GroupID { get; set; }
    }
}
