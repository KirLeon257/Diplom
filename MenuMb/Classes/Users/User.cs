using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.Users
{
    public class User
    {
        public int Id { get; set; }
        public string FIO { get;set;}
        RoleEnum _role;
        public string Role
        {
            get
            {
                return _role.ToString();
            }
            set
            {
                if (value == "Admin")
                {
                    _role = RoleEnum.Admin;
                }else if (value == "User")
                {
                    _role = RoleEnum.User;
                }
                else
                {
                    throw new ArgumentException("Передан неверный аргумент");
                }
            }

        }
        public string ApiToken { get; set; }
    }
}
