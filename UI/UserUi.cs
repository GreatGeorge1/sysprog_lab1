using System;
using System.Linq;

namespace lab1{
partial class Program{
      static void UserUi(IUser user){
            Console.WriteLine($"Hello, {user.Login}. You are user.");
            int command=0;
            do{
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Commands list:");
                Console.WriteLine("0) exit (default)");
                Console.WriteLine("1) change current user password");
                try{
                    command=Convert.ToInt32(Console.ReadLine());
                }
                catch(FormatException){
                    command=0;
                }
                switch(command){
                    case (int)UserCommands.ChangePassword: ChangePasswordForm(user);
                        break;
                }
            }while(command!=0);
           
        }

}
}
