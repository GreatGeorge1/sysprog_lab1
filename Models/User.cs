using System;

namespace lab1
{
    [Serializable]  
    public class User : IUser, IUserInternal{
        public string Login {get;set;}
        public string Password {get;set;}
        public Roles Role{get;set;}
        public User(string login,string pass, Roles role=0){
            LoginFormValidator.Validate(login, pass);
            Login=login;
            Password=pass;
            Role=role;
        }
    }

    public static class LoginFormValidator{
        /// <summary>
        /// throws ArgumentException if not valid
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        public static void Validate(string login,string pass){
            if(string.IsNullOrWhiteSpace(login)){
                throw new ArgumentException("login cannot be null or whitespace!", nameof(login));
            }
              if(string.IsNullOrWhiteSpace(pass)){
                throw new ArgumentException("password cannot be null or whitespace!", nameof(pass));
            }
        }
    }
}
