using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Modelo_Consulta;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace Capa_Controlador_Consulta
{
    public class consultaControlador
    {
        /*
         
         CREADO POR CARLOS GONZÁLEZ 9959-20-6164

         */

        consultaSentencias csSentencias = new consultaSentencias();
        public void ingresarQuery(string[] tipos, string[] datos, string tabla)
        {
            // Datos del arreglo, con corrección en el índice de query
            string nombreConsulta = datos[0];
            string tipoConsulta = datos[1];
            string tblConsultada = datos[2];
            string query = datos[4]; // Se asume que la query está en la posición 4
            string datoQueryGenerado = "SELECT ";
            string status = datos[5];
            string fragmentoActual = "";
            bool esCampo = true;

            foreach (char c in query)
            {
                if (c == '+')
                {
                    fragmentoActual = fragmentoActual.Trim();

                    if (!string.IsNullOrEmpty(fragmentoActual))
                    {
                        if (esCampo)
                        {
                            datoQueryGenerado += $"{fragmentoActual} AS "; // Es un campo
                        }
                        else
                        {
                            datoQueryGenerado += $"{fragmentoActual}, "; // Es un alias
                        }
                        esCampo = !esCampo; // Alternar entre campo y alias
                        fragmentoActual = ""; // Reiniciar el fragmento
                    }
                }
                else
                {
                    fragmentoActual += c; // Agregar el carácter actual al fragmento
                }
            }

            // Procesar el último fragmento si hay un alias pendiente
            if (!string.IsNullOrEmpty(fragmentoActual.Trim()))
            {
                if (esCampo)
                {
                    datoQueryGenerado += $"{fragmentoActual}"; // Si falta procesar un campo
                }
                else
                {
                    datoQueryGenerado += $"{fragmentoActual}"; // Si falta procesar un alias
                }
            }

            // Remover la última coma y agregar el FROM
            datoQueryGenerado = datoQueryGenerado.TrimEnd(',', ' ') + $" FROM {tblConsultada};";

            // Mostrar el query generado
            Console.WriteLine($"Query generado: {datoQueryGenerado}");

            // Construir el tipo para el método de inserción
            string tipo = string.Join(",", tipos);

            // Construir el dato para el método de inserción
            string dato = $"'{nombreConsulta}','{tipoConsulta}','{datoQueryGenerado}','{status}'";

            // Llamar a la capa de modelo para insertar la consulta
            csSentencias.insertar(dato, tipo, tabla);
        }

        public void obtenerColumbasPorTabla(ComboBox comboBoxCampos, string tabla)
        {
            // Recibimos la lista de columnas desde el modelo
            List<string> columnas = csSentencias.ObtenerColumnas(tabla);

            // Limpiamos el comboBox antes de añadir nuevas columnas
            comboBoxCampos.Items.Clear();

            // Añadimos cada columna al ComboBox
            foreach (string columna in columnas)
            {
                comboBoxCampos.Items.Add(columna);
            }
        }
        //Cargar contenido de la tabla de la base de datos
        public void CargarTablas(ComboBox comboBox1, string BD)
        {
            OdbcDataAdapter dt = csSentencias.buscartbl(BD);
            DataTable table = new DataTable();
            dt.Fill(table);
            int contador = 0;
            comboBox1.Items.Clear();
            foreach (DataRow Row in table.Rows)
            {
                comboBox1.Items.Add(table.Rows[contador][0].ToString());
                contador++;
            }
        }
        public void ingresar(string[] tipos, string[] datos, string tabla)
        {
            try
            {
                // Construcción de la cadena de tipos y datos
                string dato = "";
                string tipo = "";

                for (int x = 0; x < datos.Length; x++)
                {
                    if (x == datos.Length - 1) // Si es el último dato, no agregar coma
                    {
                        dato += "'" + datos[x] + "'";
                        tipo += tipos[x];
                    }
                    else
                    {
                        dato += "'" + datos[x] + "',";
                        tipo += tipos[x] + ",";
                    }
                }

                // Generar el query de inserción
                string query = $"INSERT INTO {tabla} ({tipo}) VALUES ({dato})";

                // Llamar a la función de inserción, asumiendo que sn tiene acceso a una conexión válida
                //csSentencias.insertar(query);

                MessageBox.Show("Dato insertado correctamente en la tabla " + tabla);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al insertar los datos: " + e.Message);
            }
        }
        // Fin de la participación de Carlos González















































       //////////////////// // SALVADOR MARTÍNEZ // PESTAÑA EDITAR ////////////////////

        public void obtenerNombresConsultas(ComboBox comboBoxConsultas)
        {
            // Recibimos la lista de nombres de consultas desde el modelo
            List<string> nombresConsultas = csSentencias.ObtenerNombresConsultas();

            // Limpiamos el ComboBox antes de añadir nuevos nombres
            comboBoxConsultas.Items.Clear();

            // Añadimos cada nombre de consulta al ComboBox
            foreach (string nombreConsulta in nombresConsultas)
            {
                comboBoxConsultas.Items.Add(nombreConsulta);
            }
        }

        public void actualizarQuery(string[] tipos, string[] datos, string tabla)
        {
            {
                // Datos del arreglo, con corrección en el índice de query
                string nombreConsulta = datos[0];
                string tipoConsulta = datos[1];
                string tblConsultada = datos[2];
                string query = datos[4]; // Se asume que la query está en la posición 4
                string datoQueryGenerado = "SELECT ";
                string status = datos[5];
                string fragmentoActual = "";
                bool esCampo = true;

                foreach (char c in query)
                {
                    if (c == '+')
                    {
                        fragmentoActual = fragmentoActual.Trim();

                        if (!string.IsNullOrEmpty(fragmentoActual))
                        {
                            if (esCampo)
                            {
                                datoQueryGenerado += $"{fragmentoActual} AS "; // Es un campo
                            }
                            else
                            {
                                datoQueryGenerado += $"{fragmentoActual}, "; // Es un alias
                            }
                            esCampo = !esCampo; // Alternar entre campo y alias
                            fragmentoActual = ""; // Reiniciar el fragmento
                        }
                    }
                    else
                    {
                        fragmentoActual += c; // Agregar el carácter actual al fragmento
                    }
                }

                // Procesar el último fragmento si hay un alias pendiente
                if (!string.IsNullOrEmpty(fragmentoActual.Trim()))
                {
                    if (esCampo)
                    {
                        datoQueryGenerado += $"{fragmentoActual}"; // Si falta procesar un campo
                    }
                    else
                    {
                        datoQueryGenerado += $"{fragmentoActual}"; // Si falta procesar un alias
                    }
                }

                // Remover la última coma y agregar el FROM
                datoQueryGenerado = datoQueryGenerado.TrimEnd(',', ' ') + $" FROM {tblConsultada};";

                // Mostrar el query generado
                Console.WriteLine($"Query generado: {datoQueryGenerado}");

                // Construir la cláusula SET para el método de actualización
                string setClause = $"consulta_SQLE = '{datoQueryGenerado}'";

                // Construir la condición WHERE para la actualización
                string condicion = $"nombre_consulta = '{nombreConsulta}'";

                // Llamar a la capa de modelo para actualizar la consulta
                csSentencias.actualizar(setClause, tabla, condicion);

            }
        }
    }
}

// FIN TRABAJO SALVADOR