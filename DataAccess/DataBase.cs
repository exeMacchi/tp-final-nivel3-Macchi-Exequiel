using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataBase
    {
        // Atributos
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        private SqlTransaction transaction;
        
        // Propiedades
        public SqlDataReader Reader { get { return reader; } }

        // Constructor
        public DataBase()
        {
            connection = new SqlConnection(ConfigurationManager.AppSettings["DB_CONNECTION_STRING"]);
            command = connection.CreateCommand();
        }

        /* --- Métodos --- */ 
        /// <summary>
        /// Establecer la consulta SQL al SqlCommand
        /// </summary>
        /// <param name="query">Consulta SQL.</param>
        public void SetQuery(string query)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
        }

        /// <summary>
        /// Configurar los parámetros de comando.
        /// </summary>
        /// <param name="param">@Parámetro.</param>
        /// <param name="value">Valor del parámetro.</param>
        public void SetParam(string param, object value)
        {
            command.Parameters.AddWithValue(param, value);
        }

        /// <summary>
        /// Limpiar todos los parámetros de comando.
        /// </summary>
        public void ClearParams()
        {
            command.Parameters.Clear();
        }

        /// <summary>
        /// Iniciar una transacción con la base de datos.
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                command.Transaction = transaction;
            }
            catch (SqlException ex)
            {
                throw new Exception("Se produjo un error al intentar actualizar los datos " +
                                    "de la base de datos. Por favor, revise los datos " +
                                    "ingresados o contacte al soporte técnico si el problema " +
                                    "persiste.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("La operación no se pudo completar debido a un problema " +
                                    "con el estado de la conexión o el comando.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La operación ha excedido el tiempo de espera. " +
                                    "Por favor, intente nuevamente más tarde.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }

        /// <summary>
        /// Confirmar la transacción en la base de datos.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                transaction?.Commit();
            }
            catch (SqlException ex)
            {
                throw new Exception("Se produjo un error al intentar actualizar los datos " +
                                    "de la base de datos. Por favor, revise los datos " +
                                    "ingresados o contacte al soporte técnico si el problema " +
                                    "persiste.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("La operación no se pudo completar debido a un problema " +
                                    "con el estado de la conexión o el comando.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La operación ha excedido el tiempo de espera. " +
                                    "Por favor, intente nuevamente más tarde.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }

        /// <summary>
        /// Revertir la transacción tras un problema.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                transaction?.Rollback();
            }
            catch (SqlException ex)
            {
                throw new Exception("Se produjo un error al intentar actualizar los datos " +
                                    "de la base de datos. Por favor, revise los datos " +
                                    "ingresados o contacte al soporte técnico si el problema " +
                                    "persiste.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("La operación no se pudo completar debido a un problema " +
                                    "con el estado de la conexión o el comando.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La operación ha excedido el tiempo de espera. " +
                                    "Por favor, intente nuevamente más tarde.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }

        /// <summary>
        /// Ejecutar la consulta SQL para una operación SELECT.
        /// </summary>
        public void ExecuteRead()
        {
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception("Se produjo un error al intentar recuperar los datos " +
                                    "desde la base de datos. Por favor, contacte al soporte " +
                                    "técnico si el problema persiste.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Hubo un problema con la operación en la base de datos.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La operación ha excedido el tiempo de espera. " +
                                    "Por favor, intente nuevamente más tarde.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }

        /// <summary>
        /// Ejecutar la consulta SQL para una operación INSERT, UPDATE o DELETE. 
        /// </summary>
        public void ExecuteNonQuery()
        {
            try
            {
                // Esto verifica si ya hay una conexión abierta por una transacción.
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Se produjo un error al intentar actualizar los datos " +
                                    "de la base de datos. Por favor, revise los datos " +
                                    "ingresados o contacte al soporte técnico si el problema " +
                                    "persiste.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("La operación no se pudo completar debido a un problema " +
                                    "con el estado de la conexión o el comando.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La operación ha excedido el tiempo de espera. " +
                                    "Por favor, intente nuevamente más tarde.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }

        /// <summary>
        /// Ejecutar una consulta SQL que espera como resultado un valor booleano.
        /// </summary>
        /// <returns>Resultado booleano de la operación</returns>
        public bool ExecuteScalar()
        {
            try
            {
                connection.Open();
                return (bool)command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception("Se produjo un error al intentar obtener el resultado " +
                                    "de la consulta. Por favor, contacte al soporte " +
                                    "técnico si el problema persiste.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("La operación no se pudo completar debido a un problema " +
                                    "con el estado de la conexión o el comando.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La operación ha excedido el tiempo de espera. " +
                                    "Por favor, intente nuevamente más tarde.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }

        /// <summary>
        /// Cerrar la conexión a la base de datos y posible DataReader.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                reader?.Close();
                connection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Hubo un problema al cerrar la conexión con la base de " +
                                    "datos. Verifique el estado de la conexión o contacte " +
                                    "al soporte técnico.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Se produjo un error al intentar cerrar la conexión. " +
                                    "La conexión podría ya estar cerrada.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado.", ex);
            }
        }
    }
}
