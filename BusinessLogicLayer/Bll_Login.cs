using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Entities;

namespace BusinessLogicLayer
{
    public class LoginService
    {
        
        private readonly DataAccess _dataAccess;

      
        public LoginService()
        {
            _dataAccess = new DataAccess(); 
        }

 
        public Usuario ValidateLogin(string username, string password)
        {
            // El nombre del stored procedure para obtener el usuario por nombre de usuario y contraseña
            string sp = "sp_GetUserByUsernameAndPassword";

            
            var user = new Usuario
            {
                Username = username,
                Password = password
            };

           
            var usuario = _dataAccess.Get<Usuario>(sp, user);

            return usuario;
        }
    }
}
