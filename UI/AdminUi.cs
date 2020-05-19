using System;
using System.Linq;

namespace lab1{
partial class Program{
      static void AdminUi(IUser user){
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Role!=Roles.Admin){
                throw new ArgumentException("user must be admin!",nameof(user));
            }
            Console.WriteLine($"Hello, {user.Login}. You are admin.");
            int command=0;
            do{
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Commands list:");
                Console.WriteLine("0) exit (default)");
                Console.WriteLine("1) change current user password");
                Console.WriteLine("2) list all users");
                Console.WriteLine("3) change password for another user");
                try{
                    command=Convert.ToInt32(Console.ReadLine());
                }
                catch(FormatException){
                    command=0;
                }
                switch(command){
                    case (int)UserCommands.ChangePassword: ChangePasswordForm(user);
                        break;
                    case (int)AdminCommands.ListUsers: ListUsers();
                        break;
                    case (int)AdminCommands.ChangePasswordForUser: ChangePasswordForUserForm();
                        break;
                }
            }while(command!=0);

        }
}
}
