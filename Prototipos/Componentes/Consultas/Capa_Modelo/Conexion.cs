using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace Capa_ModeloConsultas
{
    public class Conexion
    {

        public OdbcConnection connection()
        {
            OdbcConnection conn = new OdbcConnection("Dsn=PRUEBA-GIT-");
            try
            {
                conn.Open();
            }
            catch (OdbcException e)
            {
                Console.WriteLine(e.Message);
            }
            return conn;
        }


        public void disconnect(OdbcConnection conn)
        {
            try
            {
                conn.Close();
            }
            catch (OdbcException)
            {
                Console.WriteLine("No Conectó");
            }
        }
    }
}
