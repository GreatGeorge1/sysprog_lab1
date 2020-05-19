using System;
using System.Linq;

namespace lab1{
partial class Program{
    static void ChangePasswordForUserForm(){
        string username=string.Empty;
        Console.Write("username:");
        username=Console.ReadLine().Trim();
        var user=USERS.FirstOrDefault(x=>x.Login.Equals(username));
        if(user is null){
            Console.WriteLine($"no user found: {username}");
            return;
        }
        ChangePasswordForm(user);
    }
    static void ChangePasswordForm(IUser user){
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        Console.WriteLine($"change password for user: {user.Login}");
        string password=string.Empty;
        string repeat=string.Empty;
        for(int i=0;i<3;i++){
            Console.Write("new password:");
            password=GetHiddenConsoleInput();
            Console.WriteLine();
            Console.Write("repeat password:");
                Console.WriteLine();
            repeat=GetHiddenConsoleInput();
            if(string.Equals(password,repeat)){
                break;
            }else{
                Console.WriteLine("passwords not same");
            }
        }
        try{
            LoginFormValidator.Validate(user.Login,password);
        }catch(System.Exception e){
            Console.WriteLine(e.Message);
            Console.WriteLine("failed to change password");
            return;
        }
        ChangePassword(user,password);
        Console.WriteLine("password changed");
    }
}
}
