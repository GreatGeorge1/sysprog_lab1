using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace lab1{
partial class Program{
    static IEnumerable<IUserInternal> USERS;

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

    static void ChangePassword(IUser user, string newPassword){
            USERS.First(x=>x.Login.Equals(user.Login)).Password=newPassword;
            SaveUsers();
        }

}
}