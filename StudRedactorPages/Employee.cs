using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRedactorPages
{
    public class ClEmployee
    {
        public ClEmployee(string surname, string name, string patronymic, string login, string password, string role)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Login = login;
            Password = password;
            Role = role;
        }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
