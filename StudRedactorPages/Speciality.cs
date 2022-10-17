using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StudRedactorPages
{
    public class ClSpeciality
    {
        public ClSpeciality(int id, string code, string specname, string qualification)
        {
            Id = id;
            Code = code;
            Specname = specname;
            Qualification = qualification;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Specname { get; set; }
        public string Qualification { get; set; }
    }
}
