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
    class Program
    {
        static IEnumerable<IUserInternal> USERS;
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
               // Console.ReadKey();
            }
            SaveUsers();

        }

        static IEnumerable<IUserInternal> GetDefaultUsers(){
            return new List<User>(){
                new User("user1","pass1"),
                new User("user2","pass2"),
                new User("user3","adminpass",Roles.Admin)
            };
        }
        static IEnumerable<IUserInternal> GetUsers(){
            IFormatter formatter = new BinaryFormatter();  
            using(Stream stream = new FileStream("Users.bin", FileMode.OpenOrCreate, FileAccess.Read, FileShare.None)){
                if(stream.Length==0){
                    USERS= (IEnumerable<User>)GetDefaultUsers();
                } 
                else{
                    IEnumerable<User> users= (IEnumerable<User>)formatter.Deserialize(stream);
                    USERS=users;
                }
            }  
            return USERS;
        }
        static void SaveUsers(){
            IFormatter formatter = new BinaryFormatter();  
            using(Stream stream = new FileStream("Users.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)){
                formatter.Serialize(stream, USERS); 
            }  
        }

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
  
        static void UserUi(IUser user){
            Console.WriteLine($"Hello, {user.Login}. You are user.");
            Console.WriteLine("Commands list:");
            Console.WriteLine("0) exit (default)");
            Console.WriteLine("1) change current user password");
        }

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
                //Console.WriteLine(command);
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

        static void ChangePassword(IUser user, string newPassword){
            USERS.First(x=>x.Login.Equals(user.Login)).Password=newPassword;
            SaveUsers();
        }

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

        static void ListUsers(){
            var users=USERS;
            Console.WriteLine($"Users list-------------------------------");
            foreach(var user in users){
                Console.WriteLine($"username: {user.Login}; pass: {user.Password}; role: {user.Role.ToString()}");
            }
             Console.WriteLine($"----------------------------------------");
        }
        enum UserCommands{
            NotSet=0,
            ChangePassword=1
        }
        enum AdminCommands{
            NotSet=0,
            ChangePasswordForUser=3,
            ListUsers=2
        }
    }
}
