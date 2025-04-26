using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.OC
{
    public class ResponseblePerson
    {
        public ResponseblePerson() { }
        public ResponseblePerson(int code, string name, string surname, string patronymic)
        {
            Code = code;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }

        public int Code { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}
