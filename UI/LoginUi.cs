using System;
using System.Linq;

namespace lab1{
partial class Program{
    static IUserInternal LoginUi(){
        IUserInternal user=null;
        string password;
        for(int i=0;i<3;i++){
            Console.Write("username:");
            string username=Console.ReadLine().Trim();
            Console.Write("password:");
            password=GetHiddenConsoleInput();
                Console.WriteLine();
            try
            {
                LoginFormValidator.Validate(username,password);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            user=USERS
                .FirstOrDefault(
                    x=>x.Login.Equals(username) & x.Password.Equals(password));
            if(user is null){
                Console.WriteLine("username or password is incorrect");
                continue; 
            }else{
                break;
            }
        }
        if(user is null){
            Console.WriteLine("failed to login");
        }
        return user;
    }
}
}
