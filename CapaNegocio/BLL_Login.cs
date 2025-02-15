using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.Mappers;
using Entities;

namespace CapaNegocio
{
    public class BLL_Login
    {

        Mp_User _user = new Mp_User();

        public BLL_Login()
        {
       
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

        public int CambiarClave(int Id_Usuario, string nuevaPassword, out string Mensaje)
        {
            return _user.CambiarClave(Id_Usuario,nuevaPassword, out Mensaje);
        }

        public bool RestablecerClave(int Id_Usuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string password = BLL_Recursos.GenerarPassword();
            int Resultado = _user.RestablecerClave(Id_Usuario, BLL_Recursos.EncriptarContraseña(password), out Mensaje);

            if (Resultado == 1)
            {
                string asunto = "Contraseña Restableceida";
                string Mensaje_correo = "<h3> Su cuenta fue restablecida correctamente</h3></br><P> Su contraseña para acceder ahora es : !clave!</P>";
                Mensaje_correo = Mensaje_correo.Replace("!clave!", password);
                bool respuesta = BLL_Recursos.EnviarCorreo(correo, asunto, Mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo restablecer la contraseña";
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


            public DataTable GetAllUsers()
        {
            return _user.GetAllUsers();
        }


        public bool ValidarExistencia(string nom,string correo)
        {
            return _user.VerificarExistencia(nom, correo);
        }

        public int RegistrarUsuario(Usuario us, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(us.Nombre) || string.IsNullOrWhiteSpace(us.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";
            }
            if (string.IsNullOrEmpty(us.Apellido) || string.IsNullOrWhiteSpace(us.Apellido))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";
            }
            if (string.IsNullOrEmpty(us.Correo) || string.IsNullOrWhiteSpace(us.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string password = BLL_Recursos.GenerarPassword();

                string asunto = "Creacion de Cuenta";
                string Mensaje_correo = "<h3> Su cuenta fue creada correctamente</h3></br><P> Su contraseña para acceder es : !clave!</P>";
                Mensaje_correo = Mensaje_correo.Replace("!clave!", password);


                int Final = 0;
                us.Restablecer = true;

                us.Password = BLL_Recursos.EncriptarContraseña(password);

                Final = _user.RegistrarUsuario(us, out Mensaje);

                if(Final != -1)
                {
                    bool respuesta = BLL_Recursos.EnviarCorreo(us.Correo, asunto, Mensaje_correo);

                    if (respuesta)
                    {
                        return Final;
                    }
                    else
                    {
                        Mensaje = "No se puede enviar el correo";
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }
      
            }
            else
            {
                return -1;
            }
      
        }

        public int RegistrarCliente(Usuario us, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(us.Nombre) || string.IsNullOrWhiteSpace(us.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";
            }
            if (string.IsNullOrEmpty(us.Apellido) || string.IsNullOrWhiteSpace(us.Apellido))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";
            }
            if (string.IsNullOrEmpty(us.Correo) || string.IsNullOrWhiteSpace(us.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string password = us.Password;

                string asunto = "Creacion de Cuenta";
                string Mensaje_correo = "<h3> Su cuenta fue creada correctamente</h3></br><P> Su contraseña para acceder es : !clave!</P>";
                Mensaje_correo = Mensaje_correo.Replace("!clave!", password);


                int Final = 0;

                us.Telefono = "";
                us.Tipo = "Cliente";
                us.Restablecer = false;
                us.Activo = true;
                us.Password = BLL_Recursos.EncriptarContraseña(password);

                Final = _user.RegistrarClientes(us, out Mensaje);

                if (Final != -1)
                {
                    bool respuesta = BLL_Recursos.EnviarCorreo(us.Correo, asunto, Mensaje_correo);

                    if (respuesta)
                    {
                        return Final;
                    }
                    else
                    {
                        Mensaje = "No se puede enviar el correo";
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }

            }
            else
            {
                return -1;
            }

        }

        public int UpdateUsuario(Usuario us, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(us.Nombre) || string.IsNullOrWhiteSpace(us.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";
            }
            if (string.IsNullOrEmpty(us.Apellido) || string.IsNullOrWhiteSpace(us.Apellido))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";
            }
            if (string.IsNullOrEmpty(us.Correo) || string.IsNullOrWhiteSpace(us.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return _user.UpdateUsuario(us, out Mensaje);
            }
            else
            {
                return 0;
            }
        }


        //public Usuario ObtenerUserXNombre(string nom)
        //{
        //    return _user.ObtenerUsuarioXNom(nom);
        //}


    }
}
