using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Entities;

namespace BusinessLogicLayer
{
    public class LoginService
    {

        Mp_User _user = new Mp_User();

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
                Nombre = username,
                Password = password
            };

           
            var usuario = _dataAccess.Get<Usuario>(sp, user);

            if (usuario != null)
            {
                Debug.WriteLine($"Usuario encontrado: {usuario.Nombre}");
            }
            else
            {
                Debug.WriteLine("Usuario no encontrado o credenciales incorrectas.");
            }


            return usuario;
        }


        public DataTable autenticar(string username, string passw)
        {
            var usr = _user.autenticar(username, passw);
            if (usr != null)
            {
                return usr;
            }
            else
            {
                return null;
            }

        }

        public bool ValidarUsuario(string Nombre)
        {
            return _user.ValidarUsuario(Nombre);
        }


        public bool ValidarExistencia(string nom,string correo)
        {
            return _user.VerificarExistencia(nom, correo);
        }

        public int AgregarUser(string nom, int? tel, string correo, string contra)
        {
            return _user.RegistrarUsuario(nom,tel,correo,contra);
        }




    }
}
