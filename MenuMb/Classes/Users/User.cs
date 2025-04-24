using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes.Users
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        RoleEnum _role;
        public string Role
        {
            get
            {
                return _role.ToString();
            }
            set
            {
                if (value == "Admin" || value == "Администратор")
                {
                    _role = RoleEnum.Admin;
                }else if (value == "User" || value == "Пользователь")
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

    class NewUserInfo : User
    {
        public DateTime DateOfBirth { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ApiTokenAdmin { get; set; }
    }
}
