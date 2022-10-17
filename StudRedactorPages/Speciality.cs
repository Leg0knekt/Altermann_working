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
        public ClSpeciality(string code, string specname, string cvalification)
        {
            Code = code;
            Specname = specname;
            Cvalification = cvalification;
        }
        public string Code { get; set; }
        public string Specname { get; set; }
        public string Cvalification { get; set; }
    }
}
