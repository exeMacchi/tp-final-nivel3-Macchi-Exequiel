using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class UserBLL
    {
        /// <summary>
        /// Obtener la información de un <see cref="User"/> a partir de un correo electrónico
        /// y una contraseña especificada.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña de la cuenta.</param>
        /// <returns><see cref="User"/> con la información cargada.</returns>
        public static User GetUser(string email, string password)
        {
            DataBase db = new DataBase();
            string query = "SELECT Id AS UserID, " +
                           "       email AS UserEmail, " +
                           "       nombre AS UserFirstname, " +
                           "       apellido AS UserLastname, " +
                           "       urlImagenPerfil AS UserAvatar, " +
                           "       admin AS UserAdmin " +
                           "FROM USERS " +
                           "WHERE email = @Email AND pass = @Pass;";
            try
            {
                db.SetQuery(query);
                db.SetParam("@Email", email);
                db.SetParam("@Pass", password);
                db.ExecuteRead();

                User user = new User();
                if (db.Reader.Read())
                {
                    user.ID = (int)db.Reader["UserID"];
                    user.Email = db.Reader["UserEmail"].ToString();

                    if (!(db.Reader["UserFirstname"] is DBNull))
                        user.Firstname = db.Reader["UserFirstname"].ToString();

                    if (!(db.Reader["UserLastname"] is DBNull))
                        user.Lastname = db.Reader["UserLastname"].ToString();

                    if (!(db.Reader["UserAvatar"] is DBNull))
                        user.Avatar = db.Reader["UserAvatar"].ToString();

                    user.IsAdmin = (bool)db.Reader["UserAdmin"];
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Devolver el ID de un usuario según un correo electrónico especificado.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <returns>
        /// Devuelve el ID del usuario del email especificado o, en caso contrario,
        /// devuelve 0.
        /// </returns>
        public static int GetUserIDByEmail(string email)
        {
            DataBase db = new DataBase();
            string query = "SELECT Id AS ID FROM USERS WHERE email = @Email;";
            try
            {
                db.SetQuery(query);
                db.SetParam("@Email", email);
                db.ExecuteRead();

                if (db.Reader.Read())
                {
                    return (int)db.Reader["ID"];
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Crear un nuevo <see cref="User"/> en la base de datos.
        /// </summary>
        /// <param name="email">Correo electrónico del nuevo usuario</param>
        /// <param name="password">Contraseña de la cuenta del nuevo usuario</param>
        public static void CreateUser(string email, string password)
        {
            DataBase db = new DataBase();
            string query = "INSERT INTO USERS VALUES (@Email, @Pass, NULL, NULL, NULL, 0);";

            try
            {
                db.SetQuery(query);
                db.SetParam("@Email", email);
                db.SetParam("@Pass", password);
                db.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Actualizar la información de un usuario en la base de datos.
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUser(User user)
        {
            DataBase db = new DataBase();
            string query = "UPDATE USERS " +
                           "SET email = @UserEmail, " +
                           "    nombre = @UserFirstname, " +
                           "    apellido = @UserLastname, " +
                           "    urlImagenPerfil = @UserAvatar " +
                           "WHERE Id = @UserID;";
            try
            {
                db.SetQuery(query);
                db.SetParam("@UserEmail", user.Email);
                db.SetParam("@UserFirstname", (object)user.Firstname ?? DBNull.Value);
                db.SetParam("@UserLastname", (object)user.Lastname ?? DBNull.Value);
                db.SetParam("@UserAvatar", user.Avatar ?? Constants.AvatarPlaceholderPath);
                db.SetParam("@UserID", user.ID);
                db.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Actualizar en la base de datos la contraseña de un usuario por una nueva.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="password">Nueva contraseña</param>
        public static void UpdateUserPassword(int id, string password)
        {
            DataBase db = new DataBase();
            string query = "UPDATE USERS SET pass = @Pass WHERE Id = @ID;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@Pass", password);
                db.SetParam("@ID", id);
                db.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Verificar si en la base de datos existe un email prexistente al que
        /// un usuario intenta utilizar para registrarse.
        /// </summary>
        /// <param name="email">Cadena que repersenta el email a verificar</param>
        /// <returns>Valor booleano que indica la existencia o no del email introducido</returns>
        public static bool EmailExistsInDB(string email)
        {
            DataBase db = new DataBase();
            string query = "DECLARE @Email VARCHAR(100) = @UserEmail; " +
                           "DECLARE @Exists BIT; " +
                           "SET @Exists = 0; " +
                           "IF EXISTS (SELECT 1 FROM USERS WHERE email = @Email) " +
                           "BEGIN " +
                           "    SET @Exists = 1; " +
                           "END " +
                           "SELECT @Exists AS EmailExists;";
            try
            {
                db.SetQuery(query);
                db.SetParam("@UserEmail", email);
                return db.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
