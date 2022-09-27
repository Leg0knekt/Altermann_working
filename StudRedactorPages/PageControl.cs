using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRedactorPages
{
    public class PageControl
    {
        private static Speciality speciality;
        private static Group group;
        private static Student student;
        private static Auth auth;
        private static Registration registration;
        public static Speciality Speciality
        {
            get
            {
                if (speciality == null)
                {
                    speciality = new Speciality();
                }
                return speciality;
            }
        }
        public static Group Group
        {
            get
            {
                if (group == null)
                {
                    group = new Group();
                }
                return group;
            }
        }
        public static Student Student
        {
            get
            {
                if (student == null)
                {
                    student = new Student();
                }
                return student;
            }
        }
        public static Auth Auth
        {
            get
            {
                if (auth == null)
                {
                    auth = new Auth();
                }
                return auth;
            }
        }
        public static Registration Registration
        {
            get
            {
                if (registration == null)
                {
                    registration = new Registration();
                }
                return registration;
            }
        }
    }
}
