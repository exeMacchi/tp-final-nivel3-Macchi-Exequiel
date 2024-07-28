using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class UserBBL
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
                // TODO: manejar error
                throw ex;
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
                // TODO: manejar error
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
