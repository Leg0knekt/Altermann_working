using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRedactorPages
{
    public class ClGroup
    {
        public ClGroup(int id ,string number, int course, ClSpeciality spec)
        {
            Id = id;
            Num = number;
            Course = course;
            Spec = spec;
        }
        public int Id { get; set; }
        public string Num { get; set; }
        public int Course { get; set; }
        public ClSpeciality Spec { get; set; }
    }
}
