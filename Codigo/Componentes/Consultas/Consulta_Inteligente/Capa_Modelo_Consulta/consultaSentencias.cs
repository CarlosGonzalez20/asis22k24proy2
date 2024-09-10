using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace Capa_Modelo_Consulta
{
    public class consultaSentencias
    {
        consultaConexion con = new consultaConexion();
        protected consultaConexion conn;
        private static string baseDatos = "";

        public consultaSentencias()
        {
            this.conn = new consultaConexion();
            baseDatos = this.conn.connection().Database;
        }

        /*
         Modificado por Carlos González 
         */

        public OdbcDataAdapter buscartbl(string BD)
        {
            // Usa el esquema correcto para seleccionar las tablas del esquema dado
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + BD + "'";
            OdbcDataAdapter datatable = new OdbcDataAdapter(sql, con.connection());

            return datatable;
        }

        //insertar datos
        public void insertar(string dato, string tipo, string tabla)
        {
            try
            {
                string sql = "insert into " + tabla + "(" + tipo + ") values (" + dato + ")";
                OdbcCommand cmd = new OdbcCommand(sql, con.connection());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public List<string> ObtenerColumnas(string tabla)
        {
            List<string> columns = new List<string>();

            try
            {
                // Usa la variable 'baseDatos' ya definida en la clase
                string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                               "WHERE TABLE_SCHEMA = '" + baseDatos + "' AND TABLE_NAME = '" + tabla + "';";

                // Ejecutamos el comando con la conexión activa
                using (OdbcCommand cmd = new OdbcCommand(query, this.conn.connection()))
                {
                    OdbcDataReader reader = cmd.ExecuteReader();

                    // Añadimos las columnas a la lista
                    while (reader.Read())
                    {
                        string column = reader.GetString(0);
                        columns.Add(column);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener columnas: " + e.Message);
            }

            return columns;
        }

        /*
         Fin de la participación de Carlos González
         */




        //SALVADOR MARTINEZ// PESTAÑA EDITAR

        public List<string> ObtenerNombresConsultas()
        {
            List<string> nombresConsultas = new List<string>();

            try
            {
                // Consulta para obtener solo los nombres de las consultas
                string query = "SELECT nombre_consulta FROM tbl_consultaInteligente;";

                // Ejecutamos el comando con la conexión activa
                using (OdbcCommand cmd = new OdbcCommand(query, this.conn.connection()))
                {
                    OdbcDataReader reader = cmd.ExecuteReader();

                    // Añadimos los nombres de consulta a la lista
                    while (reader.Read())
                    {
                        string nombreConsulta = reader.GetString(0);
                        nombresConsultas.Add(nombreConsulta);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener nombres de consultas: " + e.Message);
            }

            return nombresConsultas;
        }

        //modificar
        public void actualizar(string setClause, string tabla, string condicion)
        {
            try
            {
                // Construir la consulta UPDATE con la cláusula SET y la condición WHERE
                string sql = $"UPDATE {tabla} SET {setClause} WHERE {condicion}";
                OdbcCommand cmd = new OdbcCommand(sql, con.connection());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //////////////// FIN SALVADOR MARTÍNEZ ////////////////
        

    }
        public void insertarconsulta(string cadena)
        {
            try
            {
                OdbcCommand consulta = new OdbcCommand(cadena, con.connection());
                consulta.ExecuteNonQuery();
            }
            catch (Exception Error)
            {
                Console.WriteLine("Error en modelo" + Error);
            }
        }
        public OdbcDataAdapter llenartb2(string sql)
        {
            OdbcDataAdapter datatable = new OdbcDataAdapter(sql, con.connection());
            return datatable;
        }

        public string ObtenerTipoDeDato(string seleccion, string tablaN1)
        {
            string Tipo = string.Empty;
            using (OdbcConnection conn = con.connection())
            {
                string query = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA ='" + baseDatos + "' AND TABLE_NAME = '" + tablaN1 + "' AND COLUMN_NAME = '" + seleccion + "'";
                using (OdbcCommand cmd = new OdbcCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        Tipo = result.ToString();
                    }
                }
            }
            return Tipo;
        }

        public DataTable BuscarPor2(string datobuscar, string buscaren, string operador, string tableN, DataTable dt, Label lbl_cadena)
        {
            OdbcConnection con = new OdbcConnection("Dsn=HotelSConexion");
            if (string.IsNullOrEmpty(buscaren))
            {
                String textalert = " El campo buscar, se encuentra vacio ";
                MessageBox.Show(textalert);
            }
            else
            {
                try
                {
                    String cadenaB = "";
                    con.Open();
                    cadenaB = "SELECT * FROM " + tableN + " WHERE " + buscaren + " " + operador + " '" + datobuscar + "';";
                    lbl_cadena.Text = "Buscando : " + datobuscar + " En Columna : " + buscaren;
                    OdbcDataAdapter datos = new OdbcDataAdapter(cadenaB, con);
                    datos.Fill(dt);
                    OdbcCommand comando = new OdbcCommand(cadenaB, con);
                    OdbcDataReader leer;
                    leer = comando.ExecuteReader();

                }
                catch (Exception ex)
                {
                    String textalert = "No se pudo";
                    MessageBox.Show("Error: " + ex.Message);
                    MessageBox.Show(textalert);
                }

                con.Close();
            }
            return dt;
        }
    }
}
