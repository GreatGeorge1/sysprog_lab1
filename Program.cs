using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace lab1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            GetUsers();
            IUserInternal user=LoginUi();
            if(!(user is null)){
                switch(user.Role){
                    case Roles.Default:UserUi(user);
                        break;
                    case Roles.Admin:AdminUi(user);
                        break;
                    default: UserUi(user);
                        break;
                }
            }
            SaveUsers();

        }
       
        static string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }
  
        static void ListUsers(){
            var users=USERS;
            Console.WriteLine($"Users list-------------------------------");
            foreach(var user in users){
                Console.WriteLine($"username: {user.Login}; pass: {user.Password}; role: {user.Role.ToString()}");
            }
            Console.WriteLine($"----------------------------------------");
        }
    }
}
