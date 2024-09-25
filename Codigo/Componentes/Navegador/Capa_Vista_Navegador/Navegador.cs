using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capa_Controlador_Navegador;
//using Capa_Datos_Navegador;
using Capa_Controlador_Reporteria;
using Capa_Vista_Reporteria;
using Capa_Vista_Consulta;
using CapaDatos;

namespace Capa_Vista_Navegador
{
    public partial class Navegador : UserControl
    {
        Validaciones v = new Validaciones(); // Instancia para validaciones
        logicaNav logic = new logicaNav(); // Lógica del navegador
        Form cerrar; // Formulario de cierre
        int correcto = 0; // Variable de control para verificar si todo está correcto
        string tabla = "def"; // Nombre de la tabla principal
        string otratabla = "def"; // Nombre de una tabla secundaria
        List<string> listaTablasAdicionales = new List<string>(); // Lista de tablas adicionales

        string nomForm; // Nombre del formulario
        int pos = 8; // Posición inicial de componentes
        string idRepo = ""; // ID del reporte

        int[] modoCampoCombo = new int[40]; // Modo de los campos de combo
        int noCampos = 1; // Contador de campos
        int x = 30; // Posición X inicial para componentes
        int y = 30; // Posición Y inicial para componentes
        int activar = 0; // Variable para reconocer la función del botón de guardar (1. Ingresar, 2. Modificar, 3. Eliminar)
        string[] aliasC = new string[40]; // Alias para los campos
        string[] tipoCampo = new string[30]; // Tipo de los campos
        string[] tablaCombo = new string[30]; // Nombre de las tablas de combo
        string[] campoCombo = new string[30]; // Campos de combo
        string[] listaItems = new string[30]; // Lista de items
        string[] campoDisplayCombo = new string[30]; // Campos display del combo

        string tablarelacionada = ""; // Tabla relacionada
        string campodescriptivo = ""; // Campo descriptivo
        string columnaprimararelacionada = ""; // Columna primaria relacionada
        string columnaForanea = ""; // Columna foránea

        int posCombo = 10; // Posición del combo
        int noCombo = 0; // Número de combos
        int noComboAux = 0; // Número auxiliar de combos
        int estado = 1; // Estado del formulario (1. Activado, 0. Desactivado)
        Color Cfuente = Color.White; // Color de la fuente
        Color nuevoColor = Color.White; // Nuevo color de fondo
        bool presionado = false; // Verifica si se ha presionado un botón
        sentencia sn = new sentencia(); // Objeto para obtener métodos de bitácora
        string idUsuario = ""; // ID del usuario
        string idAplicacion = ""; // ID de la aplicación
        string userActivo = ""; // Usuario activo
        string aplActivo = ""; // Aplicación activa
        string idyuda; // ID de la ayuda
        string AsRuta; // Ruta de la ayuda
        string AsIndice; // Índice de la ayuda
        string Asayuda = ""; // Estado de la ayuda
        Font fuente = new Font("Century Gothic", 13.0f, FontStyle.Regular, GraphicsUnit.Pixel); // Fuente para labels
        ToolTip ayuda_tp = new ToolTip(); // ToolTip para mostrar ayudas en la interfaz

        public Navegador()
        {
            InitializeComponent();
            limpiarListaItems(); // Limpia la lista de items
            this.AutoScaleMode = AutoScaleMode.Dpi; // Escala automática
            userActivo = idUsuario;
            aplActivo = idAplicacion;
            // Configuración del ToolTip
            ayuda_tp.IsBalloon = true;
            ayuda_tp.AutoPopDelay = 5000; // Mantener el ToolTip visible por 5 segundos
            ayuda_tp.InitialDelay = 1000; // Retraso de 1 segundo antes de que aparezca el ToolTip
            ayuda_tp.ReshowDelay = 500;   // Retraso de medio segundo antes de reaparecer
            ayuda_tp.ShowAlways = true;   // Mostrar el ToolTip siempre, incluso cuando el control no tiene el foco
            // Asignación de ToolTips a botones
            ayuda_tp.SetToolTip(Btn_Ingresar, "Agregar un nuevo registro al sistema.");
            ayuda_tp.SetToolTip(Btn_Modificar, "Modificar el registro seleccionado.");
            ayuda_tp.SetToolTip(Btn_Guardar, "Guardar los cambios realizados.");
            ayuda_tp.SetToolTip(Btn_Cancelar, "Cancelar la operación actual.");
            ayuda_tp.SetToolTip(Btn_Eliminar, "Eliminar el registro seleccionado.");
            ayuda_tp.SetToolTip(Btn_Consultar, "Acceder a las consultas avanzadas.");
            ayuda_tp.SetToolTip(Btn_Refrescar, "Actualizar los datos de la tabla.");
            ayuda_tp.SetToolTip(Btn_FlechaInicio, "Ir al primer registro.");
            ayuda_tp.SetToolTip(Btn_Anterior, "Mover al registro anterior.");
            ayuda_tp.SetToolTip(Btn_Siguiente, "Mover al siguiente registro.");
            ayuda_tp.SetToolTip(Btn_FlechaFin, "Ir al último registro.");
            ayuda_tp.SetToolTip(Btn_Ayuda, "Ver la ayuda del formulario.");
            ayuda_tp.SetToolTip(Btn_Salir, "Cerrar el formulario actual.");
            ayuda_tp.SetToolTip(Btn_Imprimir, "Mostrar un Reporte");
            ayuda_tp.SetToolTip(button1, "Asignar Documento de Ayuda");
            ayuda_tp.SetToolTip(btn_Reportes_Principal, "Mostrar un Reporte");
        }

        // Método para manejar la carga del navegador
        private void Navegador_Load(object sender, EventArgs e)
        {
            colorDialog1.Color = nuevoColor; // Asigna el color de fondo seleccionado
            this.BackColor = colorDialog1.Color; // Aplica el color de fondo al formulario
            botonesYPermisos(); // Asigna los permisos a los botones según el usuario

            if (tabla != "def") // Verifica si la tabla no es "def" (valor por defecto)
            {
                string TablaOK = logic.TestTabla(tabla); // Verifica la existencia de la tabla
                if (TablaOK == "" && correcto == 0) // Si la tabla existe y no hay errores previos
                {
                    string EstadoOK = logic.TestEstado(tabla); // Verifica el estado de la tabla
                    if (EstadoOK == "" && correcto == 0) // Si el estado de la tabla es correcto
                    {
                        if (Asayuda == "0") // Verifica si hay registros en la tabla Ayuda
                        {
                            MessageBox.Show("No se encontró ningún registro en la tabla Ayuda");
                            Application.Exit(); // Sale de la aplicación si no se encuentra ayuda
                        }
                        else
                        {
                            if (numeroAlias() == logic.contarCampos(tabla)) // Verifica si el número de alias coincide con el número de campos de la tabla
                            {
                                int i = 0;
                                DataTable dt = logic.consultaLogica(tabla, tablarelacionada, campodescriptivo, columnaForanea, columnaprimararelacionada); // Realiza la consulta lógica de la tabla
                                dataGridView1.DataSource = dt; // Asigna el resultado de la consulta al DataGridView

                                // Asigna los alias como encabezados de las columnas
                                int head = 0;
                                while (head < logic.contarCampos(tabla))
                                {
                                    dataGridView1.Columns[head].HeaderText = aliasC[head];
                                    head++;
                                }

                                CreaComponentes(); // Crea los componentes del formulario
                                colorTitulo(); // Cambia el color del título
                                lblTabla.ForeColor = Cfuente; // Asigna el color de la fuente al label de la tabla
                                deshabilitarcampos_y_botones(); // Deshabilita los campos y botones inicialmente

                                Btn_Modificar.Enabled = true;
                                Btn_Eliminar.Enabled = true;

                                // Verifica si hay registros en la tabla y habilita/deshabilita controles según corresponda
                                if (logic.TestRegistros(tabla) > 0)
                                {
                                    int numCombo = 0;
                                    foreach (Control componente in Controls)
                                    {
                                        if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                                        {
                                            if (componente is ComboBox)
                                            {
                                                // Asigna valores a los ComboBox según la lógica de la aplicación
                                                if (modoCampoCombo[numCombo] == 1)
                                                {
                                                    componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                                                }
                                                else
                                                {
                                                    componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                                                }

                                                numCombo++;
                                            }
                                            else
                                            {
                                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                                            }

                                            i++;
                                        }
                                        if (componente is Button)
                                        {
                                            // Configura los botones según el estado del registro
                                            string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                                            if (var1 == "0")
                                            {
                                                componente.Text = "Desactivado";
                                                componente.BackColor = Color.Red;
                                            }
                                            if (var1 == "1")
                                            {
                                                componente.Text = "Activado";
                                                componente.BackColor = Color.Green;
                                            }
                                            componente.Enabled = false; // Deshabilita el botón
                                        }
                                    }
                                }
                                else // Si no hay registros, deshabilita los botones de navegación y edición
                                {
                                    Btn_Anterior.Enabled = false;
                                    Btn_Siguiente.Enabled = false;
                                    Btn_FlechaInicio.Enabled = false;
                                    Btn_FlechaFin.Enabled = false;
                                    Btn_Modificar.Enabled = false;
                                    Btn_Eliminar.Enabled = false;
                                }
                            }
                            else
                            {
                                // Si el número de alias no coincide con el número de campos, muestra un mensaje de error y sale de la aplicación
                                if (numeroAlias() < logic.contarCampos(tabla))
                                {
                                    DialogResult validacion = MessageBox.Show(EstadoOK + "El número de Alias asignados es menor que el requerido \n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
                                    if (validacion == DialogResult.OK)
                                    {
                                        Application.Exit();
                                    }
                                }
                                else
                                {
                                    if (numeroAlias() > logic.contarCampos(tabla))
                                    {
                                        DialogResult validacion = MessageBox.Show(EstadoOK + "El número de Alias asignados es mayor que el requerido \n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
                                        if (validacion == DialogResult.OK)
                                        {
                                            Application.Exit();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Muestra un mensaje de error si el estado de la tabla no es correcto y sale de la aplicación
                        DialogResult validacion = MessageBox.Show(EstadoOK + "\n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
                        if (validacion == DialogResult.OK)
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    // Muestra un mensaje de error si la tabla no existe y sale de la aplicación
                    DialogResult validacion = MessageBox.Show(TablaOK + "\n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
                    if (validacion == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                // Si la tabla es "def", no realiza ninguna acción
            }
        }


        //-----------------------------------------------Funciones-----------------------------------------------//

        //******************************************** CODIGO HECHO POR DIEGO MARROQUIN ***************************** 

        void colorTitulo()
        {
            foreach (Control componente in Controls)
            {
                if (componente is Label)
                {

                    componente.ForeColor = Cfuente; // Cambia el color de la fuente de los labels
                }
            }
        }

        public void ObtenerIdUsuario(string idUsuario)
        {
            this.idUsuario = idUsuario; // Asigna el ID del usuario
        }

        public void asignarforaneas(string table, string tablarela, string campodescri, string columnafora, string columnaprimaria)
        {
            tabla = table;
            tablarelacionada = tablarela;
            campodescriptivo = campodescri;
            columnaForanea = columnafora;
            columnaprimararelacionada = columnaprimaria; // Asigna las claves foráneas y relacionadas entre tablas
        }
        //******************************************** CODIGO HECHO POR DIEGO MARROQUIN ***************************** 

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ ***************************** 
        public void ObtenerIdAplicacion(string idAplicacion)
        {
            this.idAplicacion = idAplicacion; // Asigna el ID de la aplicación
        }

        private int numeroAlias()
        {
            int i = 0;
            foreach (string cad in aliasC)
            {
                if (cad != null && cad != "")
                {
                    i++;
                }
            }
            return i; // Devuelve el número de alias asignados
        }

        public string obtenerDatoTabla(int pos)
        {
            pos = pos - 1;
            return dataGridView1.CurrentRow.Cells[pos].Value.ToString(); // Devuelve un dato específico de la tabla según la posición
        }
        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ ***************************** 

        //******************************************** CODIGO HECHO POR PABLO FLORES ***************************** 
        public string obtenerDatoCampos(int pos)
        {
            int i = 0;
            pos = pos - 1;  // Ajuste del índice, ya que 'pos' es 1-based, pero los arrays son 0-based
            string dato = "";

            // Recorre los controles relevantes: TextBox, DateTimePicker, ComboBox
            foreach (Control componente in Controls)
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    if (i == pos)  // Verifica si el índice coincide con el solicitado
                    {
                        // Obtiene el valor del control según su tipo
                        if (componente is TextBox || componente is ComboBox)
                        {
                            dato = componente.Text;  // Para TextBox y ComboBox, toma el texto
                        }
                        else if (componente is DateTimePicker dateTimePicker)
                        {
                            dato = dateTimePicker.Value.ToString("yyyy-MM-dd");  // Formato de fecha
                        }
                        break;  // Una vez encontrado el campo, termina el ciclo
                    }
                    i++;  // Incrementa el índice solo si es un control relevante
                }
            }

            return dato; // Devuelve el valor del campo encontrado
        }

        public void asignarAlias(string[] alias)
        {
            alias.CopyTo(aliasC, 0); // Asigna los alias a la lista correspondiente
        }
        //******************************************** CODIGO HECHO POR PABLO FLORES ***************************** 

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS ***************************** 
        public void asignarAyuda(string ayudar)
        {
            string AyudaOK = logic.TestTabla("ayuda");
            if (AyudaOK == "")
            {
                if (logic.contarRegAyuda(ayudar) > 0)
                {
                    idyuda = ayudar;
                    AsRuta = logic.MRuta(idyuda);
                    AsIndice = logic.MIndice(idyuda);
                    if (AsRuta == "" || AsIndice == "" || AsRuta == null || AsIndice == null)
                    {
                        DialogResult validacion = MessageBox.Show("La Ruta o índice de la ayuda está vacía", "Verificación de requisitos", MessageBoxButtons.OK);
                        if (validacion == DialogResult.OK)
                        {
                            correcto = 1;
                        }
                    }
                }
                else
                {
                    DialogResult validacion = MessageBox.Show("Por favor verifique el id de Ayuda asignado al form", "Verificación de requisitos", MessageBoxButtons.OK);
                    if (validacion == DialogResult.OK)
                    {
                        correcto = 1;
                    }
                }

            }
            else
            {
                DialogResult validacion = MessageBox.Show(AyudaOK + ", Por favor incluirla", "Verificación de requisitos", MessageBoxButtons.OK);
                if (validacion == DialogResult.OK)
                {
                    correcto = 1;
                }
            }
        }

        public void asignarReporte(string repo)
        {
            idRepo = repo; // Asigna el ID del reporte
        }
        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS ***************************** 

        //******************************************** CODIGO HECHO POR JOSUE CACAO ***************************** 
        public void asignarSalida(Form salida)
        {
            cerrar = salida; // Asigna el formulario de cierre
        }

        public void asignarColorFuente(Color FuenteC)
        {
            Cfuente = FuenteC; // Asigna el color de la fuente
        }

        public void asignarTabla(string table)
        {
            tabla = table; // Asigna la tabla principal
        }

        public void asignarTablas(List<string> tablas)
        {
            listaTablasAdicionales = tablas; // Asigna las tablas adicionales
        }

        public void asignarNombreForm(string nom)
        {
            nomForm = nom; // Asigna el nombre del formulario
            lblTabla.Text = nomForm; // Cambia el texto del label de la tabla
        }
        //******************************************** CODIGO HECHO POR JOSUE CACAO ***************************** 

        //******************************************** CODIGO HECHO POR ANIKA ESCOTO ***************************** 
        public void asignarComboConTabla(string tabla, string campoClave, string campoDisplay, int modo)
        {
            // Verifica si la tabla existe
            string TablaOK = logic.TestTabla(tabla);
            if (TablaOK == "")
            {
                // Asigna los valores para el combo
                modoCampoCombo[noCombo] = modo;
                tablaCombo[noCombo] = tabla;
                campoCombo[noCombo] = campoClave; // Este será el valor subyacente (id_raza)
                campoDisplayCombo[noCombo] = campoDisplay; // Este será lo que se muestra (nombre_raza)
                noCombo++;
            }
            else
            {
                // Muestra error si la tabla o campo son incorrectos
                DialogResult validacion = MessageBox.Show(TablaOK + ", o el campo seleccionado\n para el ComboBox es incorrecto", "Verificación de requisitos", MessageBoxButtons.OK);
                if (validacion == DialogResult.OK)
                {
                    correcto = 1;
                }
            }
        }
        //******************************************** CODIGO HECHO POR ANIKA ESCOTO ***************************** 

        //******************************************** CODIGO HECHO POR SEBASTIAN LETONA ***************************** 
        public void asignarColorFondo(Color nuevo)
        {
            nuevoColor = nuevo; // Asigna el nuevo color de fondo
        }

        public void asignarComboConLista(int pos, string lista)
        {
            posCombo = pos - 1; // Ajusta la posición del combo
            limpiarLista(lista); // Limpia la lista de items
            modoCampoCombo[noCombo] = 0;
            noCombo++;
        }

        void limpiarLista(string cadena)
        {
            limpiarListaItems(); // Limpia la lista de items
            int contadorCadena = 0;
            int contadorArray = 0;
            string palabra = "";
            while (contadorCadena < cadena.Length)
            {
                if (cadena[contadorCadena] != '|')
                {
                    palabra += cadena[contadorCadena];
                    contadorCadena++;
                }
                else
                {

                    listaItems[contadorArray] = palabra; // Agrega la palabra a la lista de items
                    palabra = "";
                    contadorArray++;
                    contadorCadena++;
                }
            }
        }

        void limpiarListaItems()
        {
            for (int i = 0; i < listaItems.Length; i++)
            {
                listaItems[i] = ""; // Limpia los elementos de la lista de items
            }
        }
        //******************************************** CODIGO HECHO POR SEBASTIAN LETONA ***************************** 

        //******************************************** CODIGO HECHO POR JORGE AVILA***************************** 
        void CreaComponentes()
        {
            // Obtiene los campos, tipos de datos y llaves de la tabla
            string[] Campos = logic.campos(tabla);
            string[] Tipos = logic.tipos(tabla);
            string[] LLaves = logic.llaves(tabla);
            int i = 0;
            int fin = Campos.Length;
            while (i < fin)
            {
                // Ajusta la posición y el desplazamiento de los componentes según el número de campos
                if (noCampos == 6 || noCampos == 11 || noCampos == 16 || noCampos == 21) { pos = 8; }
                if (noCampos >= 6 && noCampos < 10) { x = 300; }
                if (noCampos >= 11 && noCampos < 15) { x = 600; }
                if (noCampos >= 16 && noCampos < 20) { x = 900; }
                if (noCampos >= 21 && noCampos < 25) { x = 900; }

                // Crea un Label para cada campo
                Label lb = new Label();
                lb.Text = aliasC[i];
                Point p = new Point(x + pos, y * pos);
                lb.Location = p;
                lb.Name = "lb_" + Campos[i];
                lb.Font = fuente;
                lb.ForeColor = Cfuente;
                this.Controls.Add(lb);

                // Verifica si el campo es una clave primaria y no es el primer campo
                if (LLaves[i] == "PRI" && i != 0)
                {
                    LLaves[i] = "MUL"; // Cambia la llave a "MUL" para manejarla como una clave foránea
                }

                // Dependiendo del tipo de campo, crea el componente adecuado
                switch (Tipos[i])
                {
                    case "int":
                        tipoCampo[noCampos - 1] = "Num";
                        if (LLaves[i] != "MUL")
                        {
                            crearTextBoxnumerico(Campos[i]); // Crea un TextBox numérico
                        }
                        else
                        {
                            crearComboBox(Campos[i]); // Crea un ComboBox si es una clave foránea
                        }
                        break;

                    case "varchar":
                        tipoCampo[noCampos - 1] = "Text";
                        if (LLaves[i] != "MUL")
                        {
                            crearTextBoxvarchar(Campos[i]); // Crea un TextBox para texto
                        }
                        else
                        {
                            crearComboBox(Campos[i]); // Crea un ComboBox si es una clave foránea
                        }
                        break;

                    case "date":
                    case "datetime":
                        tipoCampo[noCampos - 1] = "Text";
                        if (LLaves[i] != "MUL")
                        {
                            crearDateTimePicker(Campos[i]); // Crea un DateTimePicker para fechas
                        }
                        else
                        {
                            crearComboBox(Campos[i]); // Crea un ComboBox si es una clave foránea
                        }
                        break;

                    case "text":
                        tipoCampo[noCampos - 1] = "Text";
                        if (LLaves[i] != "MUL")
                        {
                            crearTextBoxvarchar(Campos[i]); // Crea un TextBox para texto largo
                        }
                        else
                        {
                            crearComboBox(Campos[i]); // Crea un ComboBox si es una clave foránea
                        }
                        break;

                    case "time":
                        tipoCampo[noCampos - 1] = "Text";
                        crearcampohora(Campos[i]); // Crea un TextBox para horas
                        break;

                    case "float":
                    case "decimal":
                    case "double":
                        tipoCampo[noCampos - 1] = "Text";
                        crearcampodecimales(Campos[i]); // Crea un TextBox para valores decimales
                        break;

                    case "tinyint":
                        tipoCampo[noCampos - 1] = "Num";
                        if (LLaves[i] != "MUL")
                        {
                            crearBotonEstado(Campos[i]); // Crea un botón para indicar estado activado/desactivado
                        }
                        break;

                    default:
                        if (Tipos[i] != null && Tipos[i] != "")
                        {
                            DialogResult validacion = MessageBox.Show("La tabla " + tabla + " posee un campo " + Tipos[i] + ", este tipo de dato no es reconocido por el navegador\n Solucione este problema...", "Verificación de requisitos", MessageBoxButtons.OK);
                            if (validacion == DialogResult.OK)
                            {
                                Application.Exit(); // Termina la aplicación si se encuentra un tipo de dato no reconocido
                            }
                        }
                        break;
                }
                noCampos++;
                i++;
            }
        }
        //******************************************** CODIGO HECHO POR JORGE AVILA***************************** 

        //******************************************** CODIGO HECHO POR DIEGO MARROQUIN*****************************

        // Función que maneja el evento click de los botones, cambiando su estado entre "Activado" y "Desactivado"
        void func_click(object sender, EventArgs e)
        {
            foreach (Control componente in Controls) // Itera sobre todos los controles en el formulario
            {
                if (componente is Button) // Verifica si el control es un botón
                {
                    if (estado == 1) // Si el estado es 1 (Activado)
                    {
                        componente.Text = "Desactivado"; // Cambia el texto del botón a "Desactivado"
                        componente.BackColor = Color.Red; // Cambia el color de fondo a rojo
                        estado = 0; // Cambia el estado a 0 (Desactivado)
                    }
                    else // Si el estado no es 1 (es decir, es 0)
                    {
                        componente.Text = "Activado"; // Cambia el texto del botón a "Activado"
                        componente.BackColor = Color.Green; // Cambia el color de fondo a verde
                        estado = 1; // Cambia el estado a 1 (Activado)
                    }
                }
            }
        }
        //******************************************** CODIGO HECHO POR DIEGO MARROQUIN*****************************

        //******************************************** CODIGO HECHO POR PABLO FLORES*****************************

        // Función que crea un botón con un estado inicial de "Activado" y lo añade al formulario
        void crearBotonEstado(String nom)
        {
            Button btn = new Button(); // Crea un nuevo botón
            Point p = new Point(x + 125 + pos, y * pos); // Define la ubicación del botón
            btn.Location = p;
            btn.Text = "Activado"; // Establece el texto inicial del botón
            btn.BackColor = Color.Green; // Establece el color de fondo inicial
            btn.Click += new EventHandler(func_click); // Asigna la función de clic al botón
            btn.Name = nom; // Establece el nombre del botón
            btn.Enabled = false; // Deshabilita el botón inicialmente
            this.Controls.Add(btn); // Añade el botón al formulario
            pos++; // Incrementa la posición para el próximo control
        }

        // Función que crea un TextBox para números y lo añade al formulario
        void crearTextBoxnumerico(String nom)
        {
            TextBox tb = new TextBox(); // Crea un nuevo TextBox
            Point p = new Point(x + 125 + pos, y * pos); // Define la ubicación del TextBox
            tb.Location = p;
            tb.Name = nom; // Establece el nombre del TextBox
            this.Controls.Add(tb); // Añade el TextBox al formulario
            tb.KeyPress += Paravalidarnumeros_KeyPress; // Asigna la función de validación de números al evento KeyPress
            this.KeyPress += Paravalidarnumeros_KeyPress; // Asegura que el evento KeyPress valide solo números
            pos++; // Incrementa la posición para el próximo control
        }
        //******************************************** CODIGO HECHO POR PABLO FLORES*****************************

        //******************************************** CODIGO HECHO POR EMANUEL BARAHONA*****************************

        // Función que crea un TextBox para texto de tipo varchar y lo añade al formulario
        void crearTextBoxvarchar(String nom)
        {
            TextBox tb = new TextBox(); // Crea un nuevo TextBox
            Point p = new Point(x + 125 + pos, y * pos); // Define la ubicación del TextBox
            tb.Location = p;
            tb.Name = nom; // Establece el nombre del TextBox
            this.Controls.Add(tb); // Añade el TextBox al formulario
            tb.KeyPress += Paravalidarvarchar_KeyPress; // Asigna la función de validación varchar al evento KeyPress
            this.KeyPress += Paravalidarvarchar_KeyPress; // Asegura que el evento KeyPress valide solo varchar
            pos++; // Incrementa la posición para el próximo control
        }

        // Función que crea un TextBox para la hora y lo añade al formulario
        void crearcampohora(String nom)
        {
            TextBox tb = new TextBox(); // Crea un nuevo TextBox
            Point p = new Point(x + 125 + pos, y * pos); // Define la ubicación del TextBox
            tb.Location = p;
            tb.Name = nom; // Establece el nombre del TextBox
            this.Controls.Add(tb); // Añade el TextBox al formulario
            tb.KeyPress += Paravalidarhora_KeyPress; // Asigna la función de validación de hora al evento KeyPress
            this.KeyPress += Paravalidarhora_KeyPress; // Asegura que el evento KeyPress valide solo hora
            pos++; // Incrementa la posición para el próximo control
        }

        // Función que crea un TextBox para decimales y lo añade al formulario
        void crearcampodecimales(String nom)
        {
            TextBox tb = new TextBox(); // Crea un nuevo TextBox
            Point p = new Point(x + 125 + pos, y * pos); // Define la ubicación del TextBox
            tb.Location = p;
            tb.Name = nom; // Establece el nombre del TextBox
            this.Controls.Add(tb); // Añade el TextBox al formulario
            tb.KeyPress += Paravalidardecimales_KeyPress; // Asigna la función de validación de decimales al evento KeyPress
            this.KeyPress += Paravalidardecimales_KeyPress; // Asegura que el evento KeyPress valide solo decimales
            pos++; // Incrementa la posición para el próximo control
        }
        //******************************************** CODIGO HECHO POR EMANUEL BARAHONA*****************************


        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS*****************************

        // Estos métodos manejan eventos KeyPress para validar diferentes tipos de entradas en los controles.

        private void Paravalidardecimales_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Valida que el input sea un número decimal.
            v.Camposdecimales(e);
        }

        private void Paravalidarnumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Valida que el input sea un número entero.
            v.CamposNumericos(e);
        }

        private void Paravalidarhora_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Valida que el input siga el formato de hora.
            v.CamposHora(e);
        }

        private void Paravalidarvarchar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Valida que el input sea un texto tipo varchar.
            v.CamposVchar(e);
        }

        private void Paravalidartexto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Valida que el input contenga solo letras.
            v.CamposLetras(e);
        }

        private void Paravalidacombo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Maneja la validación del input en un ComboBox.
            v.Combobox(e);
        }

        // Este método crea y configura un ComboBox dinámicamente basado en los parámetros y la lógica del sistema.
        void crearComboBox(String nom)
        {
            // Se obtienen los elementos para el ComboBox, que son una lista de pares clave-valor.
            Dictionary<string, string> items;

            // Si hay una tabla y campo específicos para el ComboBox, se utilizan.
            if (tablaCombo[noComboAux] != null)
            {
                items = logic.items(tablaCombo[noComboAux], campoCombo[noComboAux], campoDisplayCombo[noComboAux]);
                if (noCombo > noComboAux) { noComboAux++; }
            }
            else
            {
                // Si no, se utiliza una tabla y campo por defecto (en este caso, películas).
                items = logic.items("Peliculas", "idPelicula", "nombrePelicula");
                if (noCombo > noComboAux) { noComboAux++; }
            }

            // Se crea un nuevo ComboBox.
            ComboBox cb = new ComboBox();
            // Se define la ubicación del ComboBox en el formulario.
            Point p = new Point(x + 125 + pos, y * pos);
            cb.Location = p;
            cb.Name = nom;  // Se asigna el nombre al ComboBox.

            // Se utiliza un BindingSource para enlazar los elementos al ComboBox.
            BindingSource bs = new BindingSource();
            bs.DataSource = items;

            // Se asigna el DataSource al ComboBox, mostrando el nombre y guardando la clave.
            cb.DataSource = bs;
            cb.DisplayMember = "Value";  // Muestra el nombre en el ComboBox.
            cb.ValueMember = "Key";      // Almacena el ID seleccionado.

            // Se añade el ComboBox al formulario.
            this.Controls.Add(cb);
            pos++;
        }

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS*****************************



        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ***************************** 
        void crearDateTimePicker(String nom)
        {
            DateTimePicker dtp = new DateTimePicker();
            Point p = new Point(x + 125 + pos, y * pos);
            dtp.Location = p;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "yyyy-MM-dd";
            dtp.Width = 100;
            dtp.Name = nom;
            this.Controls.Add(dtp);
            pos++;
        }

        public void deshabilitarcampos_y_botones()
        {
            foreach (Control componente in Controls)
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Enabled = false; // De esta manera bloqueamos todos los TextBox, DateTimePicker y ComboBox
                }
            }
            Btn_Modificar.Enabled = false;
            Btn_Eliminar.Enabled = false;
            Btn_Guardar.Enabled = false;
            Btn_Cancelar.Enabled = false;
        }

        public void habilitarcampos_y_botones()
        {
            foreach (Control componente in Controls)
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Enabled = true; // Habilita todos los TextBox, DateTimePicker y ComboBox para edición
                }
            }
            Btn_Modificar.Enabled = true;
            Btn_Eliminar.Enabled = true;
            Btn_Guardar.Enabled = true;
            Btn_Cancelar.Enabled = true;
        }

        public void actualizardatagriew()
        {
            DataTable dt = logic.consultaLogica(tabla, tablarelacionada, campodescriptivo, columnaForanea, columnaprimararelacionada);
            dataGridView1.DataSource = dt;
            int head = 0;
            while (head < logic.contarCampos(tabla))
            {
                dataGridView1.Columns[head].HeaderText = aliasC[head];
                head++;
            }
        }
        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ***************************** 


        //******************************************** CODIGO HECHO POR MATY MANCILLA*****************************

        // Este método genera una consulta SQL para "eliminar" un registro de la base de datos.
        // En lugar de eliminar físicamente el registro, se actualiza el campo 'estado' a 0, lo que indica que está inactivo.
        string crearDelete()
        {
            // Inicia la construcción de la consulta SQL con la instrucción UPDATE para la tabla especificada.
            string query = "UPDATE " + tabla + " SET estado=0";

            // Esta variable construye la cláusula WHERE de la consulta SQL.
            string whereQuery = " WHERE  ";

            int posCampo = 0;  // Posición del campo en la tabla.
            int i = 0;         // Índice utilizado para ComboBoxes.
            string campos = ""; // Acumula los campos de la tabla (no se utiliza en este contexto específico).

            // Recorre todos los controles en el formulario.
            foreach (Control componente in Controls)
            {
                // Solo se consideran TextBox, DateTimePicker y ComboBox.
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    // La condición solo se ejecuta para el primer campo (posCampo == 0), asumiendo que es la clave principal.
                    if (posCampo == 0)
                    {
                        // Determina el tipo de campo (texto o numérico) y construye la cláusula WHERE correspondiente.
                        switch (tipoCampo[posCampo])
                        {
                            case "Text": // Si el campo es de tipo texto.
                                if (componente is ComboBox)
                                {
                                    // Si el componente es un ComboBox, se utiliza la lógica específica para obtener el valor correcto.
                                    if (modoCampoCombo[i] == 1)
                                    {
                                        whereQuery += componente.Name + " = '" + logic.llaveCampolo(tablaCombo[i], campoCombo[i], componente.Text) + "' , ";
                                    }
                                    else
                                    {
                                        whereQuery += componente.Name + " = '" + componente.Text + "' , ";
                                    }
                                    i++;
                                }
                                else
                                {
                                    whereQuery += componente.Name + " = '" + componente.Text + "' , ";
                                }
                                break;

                            case "Num": // Si el campo es de tipo numérico.
                                if (componente is ComboBox)
                                {
                                    if (modoCampoCombo[i] == 1)
                                    {
                                        whereQuery += componente.Name + " = " + logic.llaveCampolo(tablaCombo[i], campoCombo[i], componente.Text);
                                    }
                                    else
                                    {
                                        whereQuery += componente.Name + " = " + componente.Text;
                                    }
                                    i++;
                                }
                                else
                                {
                                    whereQuery += componente.Name + " = " + componente.Text;
                                }
                                break;
                        }
                    }
                    posCampo++;
                }
            }

            // Elimina posibles espacios y comas al final de las cadenas.
            campos = campos.TrimEnd(' ');
            campos = campos.TrimEnd(',');
            whereQuery = whereQuery.TrimEnd(' ');
            whereQuery = whereQuery.TrimEnd(',');

            // Completa la construcción de la consulta SQL uniendo la cláusula WHERE.
            query += whereQuery + ";";

            // Imprime la consulta en la consola para depuración.
            Console.Write(query);

            // Retorna la consulta construida.
            return query;
        }

        //******************************************** CODIGO HECHO POR MATY MANCILLA*****************************



        //******************************************** CODIGO HECHO POR JOEL LOPEZ***************************** 
        string crearInsert(string nombretabla)
        {
            // Inicializa las cadenas para la consulta INSERT y los valores a insertar
            string query = "INSERT INTO " + nombretabla + " (";
            string valores = "VALUES (";

            int posCampo = 0;
            string campos = "";
            string valoresCampos = "";

            // Recorre todos los controles del formulario
            foreach (Control componente in Controls)
            {
                // Si el control es un TextBox, DateTimePicker o ComboBox
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    // Obtiene el nombre del campo de la tabla y el valor del control
                    string nombreCampo = logic.campos(nombretabla)[posCampo];
                    string valorCampo = string.Empty;

                    // Si el control es un ComboBox, obtiene el valor seleccionado (ID)
                    if (componente is ComboBox cb)
                    {
                        valorCampo = cb.SelectedValue.ToString();
                    }
                    else
                    {
                        valorCampo = componente.Text;
                    }

                    // Si el valor del campo no está vacío, lo agrega a las cadenas de campos y valores
                    if (!string.IsNullOrEmpty(valorCampo))
                    {
                        campos += nombreCampo + ", ";
                        valoresCampos += "'" + valorCampo + "', ";
                    }

                    posCampo++;
                }
            }

            // Elimina las últimas comas y cierra las instrucciones
            campos = campos.TrimEnd(' ', ',');
            valoresCampos = valoresCampos.TrimEnd(' ', ',');

            query += campos + ") " + valores + valoresCampos + ");";

            return query;
        }
        //******************************************** CODIGO HECHO POR JOEL LOPEZ***************************** 

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ***************************** 
        private IEnumerable<Control> GetAllControls(Control container)
        {
            // Función recursiva para obtener todos los controles, incluyendo los anidados
            foreach (Control control in container.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
                yield return control;
            }
        }

        string crearUpdate(string tabla, string nombreClavePrimaria, string nombreClaveForanea)
        {
            // Obtiene las columnas existentes en la tabla
            string[] columnasTabla = logic.campos(tabla);

            string query = "UPDATE " + tabla + " SET ";
            string whereQuery = " WHERE ";
            int i = 0;
            string campos = "";

            // Recorre todos los controles, incluyendo los anidados
            foreach (Control componente in GetAllControls(this))
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    string nombreCampo = componente.Name;
                    Console.WriteLine($"Evaluando control: {nombreCampo} para la tabla: {tabla}");

                    // Verifica si el control corresponde a una columna de la tabla
                    if (columnasTabla.Contains(nombreCampo))
                    {
                        // Excluye claves primarias y foráneas
                        if (nombreCampo != nombreClavePrimaria && nombreCampo != nombreClaveForanea)
                        {
                            string valorCampo = componente.Text;
                            campos += $"{nombreCampo} = '{valorCampo}', ";
                            i++;
                            Console.WriteLine($"Asignando valor '{valorCampo}' al campo '{nombreCampo}' en la tabla '{tabla}'");
                        }
                        else
                        {
                            Console.WriteLine($"El campo {nombreCampo} es clave primaria o foránea y no se actualizará.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"El control {nombreCampo} no corresponde a una columna en la tabla {tabla}");
                    }
                }
            }

            if (string.IsNullOrEmpty(campos))
            {
                Console.WriteLine($"No hay campos para actualizar en la tabla {tabla}");
                return null;
            }

            campos = campos.TrimEnd(' ', ',');

            // Usa la clave primaria o foránea en la cláusula WHERE
            string valorClave = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            if (!string.IsNullOrEmpty(nombreClaveForanea))
            {
                whereQuery += $"{nombreClaveForanea} = '{valorClave}'";
            }
            else
            {
                whereQuery += $"{nombreClavePrimaria} = '{valorClave}'";
            }

            query += campos + whereQuery + ";";
            Console.WriteLine("Consulta generada para el UPDATE: " + query);

            return query;
        }
        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ***************************** 

        //******************************************** CODIGO HECHO POR PABLO FLORES***************************** 
        public void guardadoforsozo()
        {
            // Ejecuta la consulta de inserción generada y limpia los campos del formulario
            logic.nuevoQuery(crearInsert(tabla));
            foreach (Control componente in Controls)
            {
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Enabled = true;
                    componente.Text = "";
                }
            }
            actualizardatagriew();
        }

        public void habilitarallbotones() // Habilita todos los botones
        {
            Btn_Guardar.Enabled = true;
            Btn_Ingresar.Enabled = true;
            Btn_Modificar.Enabled = true;
            Btn_Cancelar.Enabled = false;
            Btn_Eliminar.Enabled = true;
        }
        //******************************************** CODIGO HECHO POR PABLO FLORES***************************** 

        //-----------------------------------------------Funcionalidad de Botones-----------------------------------------------//

        //******************************************** CODIGO HECHO POR DIEGO MARROQUIN***************************** 
        private void Btn_Ingresar_Click(object sender, EventArgs e)
        {
            string[] Tipos = logic.tipos(tabla);

            bool tipoInt = false;
            bool ExtraAI = false;
            string auxId = "";
            int auxLastId = 0;

            // Verifica si el primer campo de la tabla es de tipo entero y autoincremental
            if (Tipos[0] == "int")
            {
                tipoInt = true;

                // Obtiene el último ID insertado en la tabla si el campo es autoincrementable
                auxId = logic.lastID(tabla);

                // Verifica si el ID existe o la tabla está vacía
                if (!string.IsNullOrEmpty(auxId))
                {
                    auxLastId = Int32.Parse(auxId);
                }
                else
                {
                    auxLastId = 0; // Si no hay registros previos, inicializa el ID en 0
                }

                ExtraAI = true; // Suponemos que el campo es autoincremental
            }

            activar = 2; // Define que se realizará una inserción
            habilitarcampos_y_botones();

            foreach (Control componente in Controls)
            {
                if (componente is TextBox && tipoInt && ExtraAI)
                {
                    // Incrementa el último ID para el nuevo registro
                    auxLastId += 1;
                    componente.Text = auxLastId.ToString();
                    componente.Enabled = false; // Deshabilita el campo autoincremental para que no sea editable
                    tipoInt = false;
                    ExtraAI = false;
                }
                else if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Enabled = true;
                    componente.Text = "";
                }
                else if (componente is Button)
                {
                    componente.Enabled = true;
                }

                Btn_Ingresar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Eliminar.Enabled = false;
                Btn_Cancelar.Enabled = true;
            }

            botonesYPermisosSinMensaje();
            // Habilita y deshabilita botones según el usuario
            Btn_Ingresar.Enabled = false;
            Btn_Modificar.Enabled = false;
            Btn_Eliminar.Enabled = false;
        }
        //******************************************** CODIGO HECHO POR DIEGO MARROQUIN***************************** 

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ ***************************** 
        private void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Muestra un mensaje de confirmación antes de proceder con la modificación
                DialogResult result = MessageBox.Show(
                    "Está a punto de modificar un registro existente.\n\n" +
                    "Asegúrese de que todos los datos sean correctos antes de continuar.\n\n" +
                    "¿Desea proceder con la modificación de este registro?",
                    "Confirmación de Modificación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, // Icono de advertencia para hacerlo más formal
                    MessageBoxDefaultButton.Button2 // Por defecto, la opción seleccionada será 'No'
                );

                // Si el usuario selecciona "No", se cancela la operación
                if (result == DialogResult.No)
                {
                    return;
                }

                // Habilita campos para edición
                habilitarcampos_y_botones();
                activar = 1; // Define que se realizará una modificación
                int i = 0;

                string[] Tipos = logic.tipos(tabla);
                int numCombo = 0;

                // Recorre los controles para habilitar la edición
                foreach (Control componente in Controls)
                {
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        // Deshabilita el campo del ID si es entero (autoincremental)
                        if (i == 0 && Tipos[0] == "int")
                        {
                            componente.Enabled = false;
                        }
                        else
                        {
                            if (componente is ComboBox)
                            {
                                if (modoCampoCombo[numCombo] == 1)
                                {
                                    componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                                }
                                else
                                {
                                    componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                                }
                                numCombo++;
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                        }

                        i++;
                    }

                    if (componente is Button)
                    {
                        componente.Enabled = true;
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();

                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                            estado = 0;
                        }
                        else if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                            estado = 1;
                        }
                        componente.Enabled = true;
                    }
                }

                // Habilita y deshabilita botones según el usuario
                botonesYPermisosSinMensaje();
                Btn_Ingresar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Eliminar.Enabled = false;
                Btn_Cancelar.Enabled = true;
            }
            catch (Exception ex)
            {
                // Manejo de errores y muestra un mensaje más profesional
                MessageBox.Show(
                    "Ocurrió un error durante la modificación del registro.\n\n" +
                    "Detalles del error: " + ex.Message + "\n\n" +
                    "Por favor, intente nuevamente o contacte al administrador del sistema.",
                    "Error en la Modificación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error // Icono de error para hacer el mensaje más claro
                );
            }
        }
        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ*****************************


        //******************************************** CODIGO HECHO POR JORGE AVILA***************************** 
        private void Btn_Cancelar_Click(object sender, EventArgs e)
        {
            try
            {
                sn.insertarBitacora(idUsuario, "Se presionó el botón cancelar en " + tabla, tabla, idAplicacion);
                // Mostrar un mensaje de confirmación antes de cancelar la operación actual
                DialogResult result = MessageBox.Show(
                    "Está a punto de cancelar los cambios no guardados.\n\n" +
                    "Cualquier dato ingresado se perderá.\n\n" +
                    "¿Desea realmente cancelar la operación?",
                    "Confirmación de Cancelación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, // Icono de advertencia
                    MessageBoxDefaultButton.Button2 // Opción "No" predeterminada
                );

                // Si el usuario selecciona "No", se cancela la operación de cancelación
                if (result == DialogResult.No)
                {
                    return;
                }

                // Reestablecer botones y deshabilitar el que no se necesita
                Btn_Modificar.Enabled = true;
                Btn_Guardar.Enabled = false;
                Btn_Cancelar.Enabled = false;
                Btn_Ingresar.Enabled = true;
                Btn_Eliminar.Enabled = true;
                Btn_Refrescar.Enabled = true;

                // Actualizar el DataGridView y los controles a su estado original
                actualizardatagriew();
                if (logic.TestRegistros(tabla) > 0)
                {
                    int i = 0;
                    int numCombo = 0;
                    foreach (Control componente in Controls)
                    {
                        if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                        {
                            if (componente is ComboBox)
                            {
                                if (modoCampoCombo[numCombo] == 1)
                                {
                                    componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                                }
                                else
                                {
                                    componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                                }
                                numCombo++;
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                            componente.Enabled = false;
                            i++;
                        }
                        if (componente is Button)
                        {
                            string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            if (var1 == "0")
                            {
                                componente.Text = "Desactivado";
                                componente.BackColor = Color.Red;
                            }
                            if (var1 == "1")
                            {
                                componente.Text = "Activado";
                                componente.BackColor = Color.Green;
                            }
                            componente.Enabled = false;
                        }
                    }
                }

                // Habilitar/deshabilitar según los permisos del usuario
                botonesYPermisosSinMensaje();
            }
            catch (Exception ex)
            {
                // Manejo de errores con un mensaje más profesional
                MessageBox.Show(
                    "Ocurrió un error durante el proceso de cancelación.\n\n" +
                    "Detalles del error: " + ex.Message + "\n\n" +
                    "Por favor, intente nuevamente o contacte al administrador del sistema.",
                    "Error en Cancelación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error // Icono de error
                );
            }
        }
        //******************************************** CODIGO HECHO POR JORGE AVILA ***************************** 



        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS***************************** 
        private void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!presionado)
                {
                    // Habilitar y deshabilitar botones según el estado de eliminación
                    Btn_Guardar.Enabled = false;
                    Btn_Modificar.Enabled = false;
                    Btn_Eliminar.Enabled = true;
                    Btn_Cancelar.Enabled = true;
                    Btn_Ingresar.Enabled = false;
                    presionado = true;
                }
                else
                {
                    // Mostrar un mensaje de confirmación antes de proceder con la eliminación
                    DialogResult respuestaEliminar = MessageBox.Show(
                        "Está a punto de eliminar permanentemente este registro del sistema.\n\n" +
                        "Esta operación no se puede deshacer. ¿Está seguro de que desea continuar?",
                        "Confirmación de Eliminación - " + nomForm,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2 // Opción "No" predeterminada
                    );

                    if (respuestaEliminar == DialogResult.Yes)
                    {
                        // Proceder con la eliminación
                        logic.nuevoQuery(crearDelete());
                        actualizardatagriew();

                        // Mostrar un mensaje de éxito tras la eliminación
                        MessageBox.Show(
                            "El registro ha sido eliminado correctamente.",
                            "Eliminación Exitosa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information

                        );

                        // Restablecer el estado de los botones
                        Btn_Modificar.Enabled = true;
                        Btn_Guardar.Enabled = false;
                        Btn_Cancelar.Enabled = false;
                        Btn_Eliminar.Enabled = true;
                        Btn_Ingresar.Enabled = true;
                        presionado = false;
                    }
                    else if (respuestaEliminar == DialogResult.No)
                    {
                        // Si el usuario cancela la operación de eliminación
                        MessageBox.Show(
                            "La eliminación ha sido cancelada.",
                            "Operación Cancelada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        // Mantener el estado de los botones
                        Btn_Guardar.Enabled = false;
                        Btn_Modificar.Enabled = false;
                        Btn_Eliminar.Enabled = true;
                        Btn_Cancelar.Enabled = true;
                        Btn_Ingresar.Enabled = false;
                        presionado = true;
                    }
                }

                // Habilitar/deshabilitar botones según los permisos del usuario
                botonesYPermisosSinMensaje(); ;
                presionado = true;
                sn.insertarBitacora(idUsuario, "Se actualizó el estado en " + tabla, tabla, idAplicacion);
            }
            catch (Exception ex)
            {
                // Manejo de errores con un mensaje más profesional
                MessageBox.Show(
                    "Ocurrió un error durante el proceso de eliminación del registro.\n\n" +
                    "Detalles del error: " + ex.Message + "\n\n" +
                    "Por favor, intente nuevamente o contacte al administrador del sistema.",
                    "Error en la Eliminación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS***************************** 

        //******************************************** CODIGO HECHO POR SEBASTIAN LETONA***************************** 
        private void Btn_Imprimir_Click(object sender, EventArgs e)
        {
            //DLL DE IMPRESION, FORMATO DE REPORTES.
            string llave = "";
            if (idRepo != "")
            {
                llave = logic.ObtenerIdReporte(idRepo);

            }
            else
            {
                MessageBox.Show("No se asignó ningún reporte....");
            }

            if (llave != "")
            {
                Clientes cl = new Clientes(llave);
                cl.Show();
            }
            else
            {
                MessageBox.Show("El Id Asignado es incorrecto");
            }

            //habilitar y deshabilitar según Usuario
            botonesYPermisosSinMensaje();
        }
        //******************************************** CODIGO HECHO POR SEBASTIAN LETONA***************************** 

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS***************************** 
        private void Btn_Refrescar_Click(object sender, EventArgs e)
        {
            try
            {
                // Refrescar la DataGridView y actualizar los controles
                actualizardatagriew();

                // Iterar sobre los controles y actualizar sus valores con los datos del DataGridView
                int i = 0;
                int numCombo = 0;
                foreach (Control componente in Controls)
                {
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        if (componente is ComboBox)
                        {
                            if (modoCampoCombo[numCombo] == 1)
                            {
                                componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                            numCombo++;
                        }
                        else
                        {
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        }

                        i++;
                    }
                    if (componente is Button)
                    {
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                        }
                        if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                        }
                        componente.Enabled = false;
                    }
                }

                // Habilitar y deshabilitar botones según permisos del usuario
                botonesYPermisosSinMensaje();

                // Mostrar mensaje de éxito al refrescar los datos
                MessageBox.Show(
                    "Los datos han sido actualizados correctamente.",
                    "Refrescado Exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                // Manejo de errores y mostrar un mensaje más profesional
                MessageBox.Show(
                    "Ocurrió un error al refrescar los datos.\n\n" +
                    "Detalles del error: " + ex.Message + "\n\n" +
                    "Por favor, intente nuevamente o contacte al administrador del sistema.",
                    "Error al Refrescar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS***************************** 

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ*****************************

        // Este método maneja el evento de clic en el botón "Anterior".
        // Permite al usuario navegar a la fila anterior en el DataGridView y actualizar los controles del formulario con los datos de la fila seleccionada.
        private void Btn_Anterior_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] Campos = logic.campos(tabla); // Obtiene los nombres de los campos de la tabla.

            // Obtiene el índice de la fila seleccionada actualmente.
            int fila = dataGridView1.SelectedRows[0].Index;

            // Si no estamos en la primera fila, permite moverse a la fila anterior.
            if (fila > 0)
            {
                dataGridView1.Rows[fila - 1].Selected = true; // Selecciona la fila anterior.
                dataGridView1.CurrentCell = dataGridView1.Rows[fila - 1].Cells[0]; // Establece la celda actual en la fila anterior.

                int numCombo = 0;
                // Recorre los controles del formulario y actualiza sus valores según los datos de la fila seleccionada.
                foreach (Control componente in Controls)
                {
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        if (componente is ComboBox)
                        {
                            // Si el componente es un ComboBox, utiliza lógica específica para recuperar el valor correcto.
                            if (modoCampoCombo[numCombo] == 1)
                            {
                                componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                            numCombo++;
                        }
                        else
                        {
                            // Para otros componentes, asigna directamente el valor de la celda correspondiente.
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        }
                        i++;
                    }
                    if (componente is Button)
                    {
                        // Si el componente es un botón, cambia su texto y color de fondo según el valor.
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                        }
                        if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                        }
                        componente.Enabled = false; // Deshabilita el botón.
                    }
                }
            }
        }

        // Este método maneja el evento de clic en el botón "Siguiente".
        // Permite al usuario navegar a la siguiente fila en el DataGridView y actualizar los controles del formulario con los datos de la fila seleccionada.
        private void Btn_Siguiente_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] Campos = logic.campos(tabla); // Obtiene los nombres de los campos de la tabla.

            // Obtiene el índice de la fila seleccionada actualmente.
            int fila = dataGridView1.SelectedRows[0].Index;

            // Si no estamos en la última fila, permite moverse a la fila siguiente.
            if (fila < dataGridView1.Rows.Count - 1)
            {
                dataGridView1.Rows[fila + 1].Selected = true; // Selecciona la siguiente fila.
                dataGridView1.CurrentCell = dataGridView1.Rows[fila + 1].Cells[0]; // Establece la celda actual en la siguiente fila.

                int numCombo = 0;
                // Recorre los controles del formulario y actualiza sus valores según los datos de la fila seleccionada.
                foreach (Control componente in Controls)
                {
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        if (componente is ComboBox)
                        {
                            // Si el componente es un ComboBox, utiliza lógica específica para recuperar el valor correcto.
                            if (modoCampoCombo[numCombo] == 1)
                            {
                                componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                            numCombo++;
                        }
                        else
                        {
                            // Para otros componentes, asigna directamente el valor de la celda correspondiente.
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        }
                        i++;
                    }
                    if (componente is Button)
                    {
                        // Si el componente es un botón, cambia su texto y color de fondo según el valor.
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                        }
                        if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                        }
                        componente.Enabled = false; // Deshabilita el botón.
                    }
                }
            }
        }

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ*****************************




        //******************************************** CODIGO HECHO POR JOEL LOPEZ***************************** 

        // Este método maneja el evento de clic del botón "Flecha Fin" para moverse al último registro visible en el DataGridView.
        private void Btn_FlechaFin_Click(object sender, EventArgs e)
        {
            // Selecciona la penúltima fila (contando desde cero) en el DataGridView y la establece como la fila actual.
            dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0];

            int i = 0;
            // Obtiene los nombres de las columnas de la tabla en la base de datos.
            string[] Campos = logic.campos(tabla);

            // Obtiene el índice de la fila actualmente seleccionada en el DataGridView.
            int fila = dataGridView1.SelectedRows[0].Index;
            if (fila < dataGridView1.Rows.Count - 1)
            {
                // Vuelve a seleccionar la penúltima fila para asegurarse de que es la fila correcta.
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0];

                int numCombo = 0;
                // Itera sobre todos los controles en el formulario.
                foreach (Control componente in Controls)
                {
                    // Si el control es un TextBox, DateTimePicker o ComboBox, actualiza su valor con el valor correspondiente de la celda seleccionada en el DataGridView.
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        if (componente is ComboBox)
                        {
                            // Para los ComboBox, se verifica si el campo es clave externa (modoCampoCombo) y se obtiene el valor adecuado.
                            if (modoCampoCombo[numCombo] == 1)
                            {
                                componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                            numCombo++;
                        }
                        else
                        {
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        }
                        i++;
                    }
                    // Si el control es un botón, se actualiza su estado dependiendo del valor de la celda en la fila seleccionada.
                    if (componente is Button)
                    {
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                        }
                        if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                        }
                        componente.Enabled = false;
                    }
                }
            }
        }

        // Este método maneja el evento de clic del botón "Flecha Inicio" para moverse al primer registro en el DataGridView.
        private void Btn_FlechaInicio_Click(object sender, EventArgs e)
        {
            // Selecciona la primera fila en el DataGridView y la establece como la fila actual.
            dataGridView1.Rows[0].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];

            int i = 0;
            // Obtiene los nombres de las columnas de la tabla en la base de datos.
            string[] Campos = logic.campos(tabla);

            int fila = dataGridView1.SelectedRows[0].Index;
            if (fila < dataGridView1.Rows.Count - 1)
            {
                // Vuelve a seleccionar la primera fila para asegurarse de que es la fila correcta.
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];

                int numCombo = 0;
                // Itera sobre todos los controles en el formulario.
                foreach (Control componente in Controls)
                {
                    // Si el control es un TextBox, DateTimePicker o ComboBox, actualiza su valor con el valor correspondiente de la celda seleccionada en el DataGridView.
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        if (componente is ComboBox)
                        {
                            // Para los ComboBox, se verifica si el campo es clave externa (modoCampoCombo) y se obtiene el valor adecuado.
                            if (modoCampoCombo[numCombo] == 1)
                            {
                                componente.Text = logic.llaveCampoRev(tablaCombo[numCombo], campoCombo[numCombo], dataGridView1.CurrentRow.Cells[i].Value.ToString());
                            }
                            else
                            {
                                componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                            }
                            numCombo++;
                        }
                        else
                        {
                            componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        }
                        i++;
                    }
                    // Si el control es un botón, se actualiza su estado dependiendo del valor de la celda en la fila seleccionada.
                    if (componente is Button)
                    {
                        string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                        if (var1 == "0")
                        {
                            componente.Text = "Desactivado";
                            componente.BackColor = Color.Red;
                        }
                        if (var1 == "1")
                        {
                            componente.Text = "Activado";
                            componente.BackColor = Color.Green;
                        }
                        componente.Enabled = false;
                    }
                }
            }
        }
        //******************************************** CODIGO HECHO POR JOEL LOPEZ*****************************

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS***************************** 

        // Este método maneja el evento de clic del botón de Ayuda para mostrar el archivo de ayuda HTML asociado.
        private void Btn_Ayuda_Click(object sender, EventArgs e)
        {
            try
            {
                // Intenta abrir el archivo de ayuda HTML utilizando la ruta y el índice proporcionados.
                Help.ShowHelp(this, AsRuta, AsIndice);
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje al usuario y registra el error en la consola.
                MessageBox.Show("Ocurrió un error al abrir la ayuda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error al abrir la ayuda: " + ex.ToString());
                // Opcional: podrías registrar el error en un archivo de log.
            }

            botonesYPermisosSinMensaje(); // Actualiza los permisos de los botones en función del usuario.
        }
        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS*****************************


        //******************************************** CODIGO HECHO POR ANIKA ESCOTO *****************************

        // Este método maneja el evento de clic del botón "Salir" para gestionar el cierre del formulario, considerando si hay operaciones pendientes.
        private void Btn_Salir_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica si se está en medio de una operación de guardado y se quiere salir sin finalizar.
                if (Btn_Guardar.Enabled == true && Btn_Cancelar.Enabled == true && Btn_Eliminar.Enabled == false && Btn_Modificar.Enabled == false && Btn_Ingresar.Enabled == false)
                {
                    foreach (Control componente in Controls)
                    {
                        // Si hay un TextBox con datos, pregunta al usuario si desea guardar antes de salir.
                        if (componente.Text != "" && componente is TextBox)
                        {
                            DialogResult respuestaGuardar = MessageBox.Show(
                                "Se ha detectado una operación de guardado en curso.\n\n" +
                                "¿Desea guardar los cambios antes de salir?",
                                "Confirmación de Salida - " + nomForm,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Warning
                            );

                            // Si el usuario elige "Yes", se guarda y luego se cierra el formulario.
                            if (respuestaGuardar == DialogResult.Yes)
                            {
                                guardadoforsozo();
                                cerrar.Visible = false;
                            }
                            // Si el usuario elige "No", se cierra el formulario sin guardar.
                            else if (respuestaGuardar == DialogResult.No)
                            {
                                cerrar.Visible = false;
                            }
                            // Si el usuario elige "Cancel", se cancela la salida y permanece en el formulario.
                            else if (respuestaGuardar == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                    }
                }

                // Verifica si se está en medio de una operación de modificación y se quiere salir sin finalizar.
                if (Btn_Modificar.Enabled == true && Btn_Guardar.Enabled == true && Btn_Cancelar.Enabled == true && Btn_Ingresar.Enabled == false)
                {
                    foreach (Control componente in Controls)
                    {
                        // Si hay un TextBox con datos, pregunta al usuario si desea regresar y finalizar la operación antes de salir.
                        if (componente.Text != "" && componente is TextBox)
                        {
                            DialogResult respuestaModificar = MessageBox.Show(
                                "Se ha detectado una operación de modificación en curso.\n\n" +
                                "¿Desea regresar y finalizar la operación antes de salir?",
                                "Confirmación de Salida - " + nomForm,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Warning
                            );

                            // Si el usuario elige "Yes", se cancela la salida y permanece en el formulario.
                            if (respuestaModificar == DialogResult.Yes)
                            {
                                return;
                            }
                            // Si el usuario elige "No", se cierra el formulario sin finalizar la modificación.
                            else if (respuestaModificar == DialogResult.No)
                            {
                                cerrar.Visible = false;
                            }
                            // Si el usuario elige "Cancel", se cancela la salida y permanece en el formulario.
                            else if (respuestaModificar == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                    }
                }

                // Verifica si se está en medio de una operación de eliminación y se quiere salir sin finalizar.
                if (Btn_Eliminar.Enabled == true && Btn_Cancelar.Enabled == true && Btn_Modificar.Enabled == false && Btn_Guardar.Enabled == false && Btn_Ingresar.Enabled == false)
                {
                    foreach (Control componente in Controls)
                    {
                        // Si hay un TextBox con datos, pregunta al usuario si desea regresar y finalizar la operación antes de salir.
                        if (componente.Text != "" && componente is TextBox)
                        {
                            DialogResult respuestaEliminar = MessageBox.Show(
                                "Se ha detectado una operación de eliminación en curso.\n\n" +
                                "¿Desea regresar y finalizar la operación antes de salir?",
                                "Confirmación de Salida - " + nomForm,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Warning
                            );

                            // Si el usuario elige "Yes", se cancela la salida y permanece en el formulario.
                            if (respuestaEliminar == DialogResult.Yes)
                            {
                                return;
                            }
                            // Si el usuario elige "No", se cierra el formulario sin finalizar la eliminación.
                            else if (respuestaEliminar == DialogResult.No)
                            {
                                cerrar.Visible = false;
                            }
                            // Si el usuario elige "Cancel", se cancela la salida y permanece en el formulario.
                            else if (respuestaEliminar == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                    }
                }

                // Confirma finalmente si el usuario desea salir del formulario si no hay operaciones pendientes.
                DialogResult confirmacionFinal = MessageBox.Show(
                    "¿Está seguro de que desea salir del formulario?",
                    "Confirmación de Salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // Si el usuario confirma, cierra el formulario.
                if (confirmacionFinal == DialogResult.Yes)
                {
                    cerrar.Visible = false;
                }
                else
                {
                    return; // Si el usuario decide quedarse, se cancela la salida.
                }
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje al usuario con los detalles del error.
                MessageBox.Show(
                    "Ocurrió un error al intentar salir del formulario.\n\n" +
                    "Detalles del error: " + ex.Message + "\n\n" +
                    "Por favor, intente nuevamente o contacte al administrador del sistema.",
                    "Error al Salir",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        //******************************************** CODIGO HECHO POR ANIKA ESCOTO *****************************

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS *****************************

        // Este método configura los botones del formulario en función de los permisos del usuario, sin mostrar mensajes.
        public void botonesYPermisosSinMensaje()
        {
            try
            {
                // Lista de nombres de permisos que corresponden a las acciones posibles.
                string[] permisosText = { "INGRESAR", "BUSCAR", "MODIFICAR", "ELIMINAR", "IMPRIMIR" };
                // Lista de botones en el formulario que se habilitarán o deshabilitarán según los permisos del usuario.
                Button[] botones = { Btn_Ingresar, Btn_Consultar, Btn_Modificar, Btn_Eliminar, Btn_Imprimir };

                for (int i = 0; i < permisosText.Length; i++)
                {
                    // Consulta el ID del usuario actual.
                    string idUsuario1 = logic.ObtenerIdUsuario(idUsuario);
                    sentencia sen = new sentencia();
                    // Verifica si el usuario tiene permiso para la acción correspondiente.
                    bool tienePermiso = sn.consultarPermisos(idUsuario1, idAplicacion, i + 1);

                    // Si el botón existe (no es nulo), se habilita o deshabilita según los permisos del usuario.
                    if (botones[i] != null)
                    {
                        botones[i].Enabled = tienePermiso;
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, se muestra un mensaje en la consola con los detalles del error.
                Console.WriteLine("Error al configurar los botones y permisos: " + ex.Message);
                // Opcionalmente, se podría registrar el error en un archivo de log.
            }
        }

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS *****************************


        //******************************************** CODIGO HECHO POR JOSUE CACAO *****************************

        // Este método configura los botones del formulario basándose en los permisos del usuario y muestra un mensaje con los botones habilitados y deshabilitados.
        public void botonesYPermisos()
        {
            try
            {
                // Definición de los nombres de los permisos y sus correspondientes botones en el formulario.
                string[] permisosText = { "INGRESAR", "BUSCAR", "MODIFICAR", "ELIMINAR", "IMPRIMIR" };
                Button[] botones = { Btn_Ingresar, Btn_Consultar, Btn_Modificar, Btn_Eliminar, Btn_Imprimir };

                string botonesPermitidos = "";   // Variable para almacenar los botones que el usuario tiene permiso de usar.
                string botonesNoPermitidos = ""; // Variable para almacenar los botones que el usuario no tiene permiso de usar.

                for (int i = 0; i < permisosText.Length; i++)
                {
                    // Obtiene el ID del usuario actual.
                    string idUsuario1 = logic.ObtenerIdUsuario(idUsuario);
                    sentencia sen = new sentencia();
                    // Consulta si el usuario tiene el permiso correspondiente.
                    bool tienePermiso = sn.consultarPermisos(idUsuario1, idAplicacion, i + 1);

                    if (botones[i] != null)
                    {
                        botones[i].Enabled = tienePermiso; // Habilita o deshabilita el botón según el permiso.

                        if (botones[i].Enabled)
                        {
                            // Si el botón está habilitado, se agrega a la lista de botones permitidos.
                            botonesPermitidos += permisosText[i] + ", ";
                        }
                        else
                        {
                            // Si el botón está deshabilitado, se agrega a la lista de botones no permitidos.
                            botonesNoPermitidos += permisosText[i] + ", ";
                        }
                    }
                    else
                    {
                        // Si el botón no se encuentra, se agrega a la lista de botones no permitidos.
                        botonesNoPermitidos += permisosText[i] + ", ";
                    }
                }

                // Formateo del mensaje final que se mostrará al usuario.
                string mensaje = "";

                if (!string.IsNullOrEmpty(botonesPermitidos))
                {
                    mensaje += "Botones permitidos: " + botonesPermitidos.TrimEnd(',', ' ') + ".\n";
                }

                if (!string.IsNullOrEmpty(botonesNoPermitidos))
                {
                    mensaje += "Botones no permitidos: " + botonesNoPermitidos.TrimEnd(',', ' ') + ".";
                }

                if (!string.IsNullOrEmpty(mensaje))
                {
                    // Muestra un único MessageBox con el resumen de los botones permitidos y no permitidos.
                    MessageBox.Show(mensaje);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, mostrando un mensaje en caso de que algo salga mal al configurar los botones.
                MessageBox.Show("Error al configurar los botones y permisos: " + ex.Message);
            }
        }

        //******************************************** CODIGO HECHO POR JOSUE CACAO *****************************

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ *****************************

        // Este método maneja el evento de clic del botón "Guardar", asegurando que los datos sean válidos y guardándolos en la base de datos.
        private void Btn_Guardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Muestra un mensaje de confirmación antes de proceder con el guardado.
                DialogResult result = MessageBox.Show(
                    "Está a punto de guardar un nuevo registro en el sistema.\n\n" +
                    "Asegúrese de que toda la información ingresada sea correcta.\n\n" +
                    "¿Desea continuar con el guardado?",
                    "Confirmación de Guardado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // Si el usuario selecciona "No", se cancela la operación.
                if (result == DialogResult.No)
                {
                    return;
                }

                // Verifica si todos los campos están llenos antes de proceder.
                bool lleno = true;

                foreach (Control componente in Controls)
                {
                    if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                    {
                        if (string.IsNullOrEmpty(componente.Text))
                        {
                            lleno = false;
                            break; // Si encuentra un campo vacío, sale del bucle.
                        }
                    }
                }

                // Si hay campos vacíos, muestra un mensaje de advertencia y no guarda el registro.
                if (!lleno)
                {
                    MessageBox.Show(
                        "Por favor, complete todos los campos antes de guardar.\n\n" +
                        "El registro no se puede guardar mientras existan campos vacíos.",
                        "Campos Vacíos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // Lista para almacenar las consultas SQL que se ejecutarán.
                List<string> queries = new List<string>();

                // Dependiendo del valor de "activar", se realiza una actualización o inserción.
                switch (activar)
                {
                    case 1: // Actualizar
                        string clavePrimariaPrincipal = logic.ObtenerClavePrimaria(tabla);
                        string queryPrincipal = crearUpdate(tabla, clavePrimariaPrincipal, null);
                        queries.Add(queryPrincipal); // Añade la consulta principal a la lista.

                        // Itera sobre las tablas adicionales para actualizar registros relacionados.
                        foreach (string tablaAdicional in listaTablasAdicionales)
                        {
                            if (!string.IsNullOrEmpty(tablaAdicional))
                            {
                                string clavePrimariaAdicional = logic.ObtenerClavePrimaria(tablaAdicional);
                                string claveForaneaAdicional = logic.ObtenerClaveForanea(tablaAdicional, tabla);
                                string queryAdicional = crearUpdate(tablaAdicional, clavePrimariaAdicional, claveForaneaAdicional);

                                if (!string.IsNullOrEmpty(queryAdicional))
                                {
                                    queries.Add(queryAdicional); // Añade la consulta adicional a la lista.
                                }
                            }
                        }

                        // Ejecuta las consultas para actualizar datos en múltiples tablas.
                        logic.insertarDatosEnMultiplesTablas(queries);
                        MessageBox.Show("El registro ha sido actualizado correctamente en todas las tablas.", "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sn.insertarBitacora(idUsuario, "Actualizó registros en múltiples tablas", tabla, idAplicacion);
                        break;

                    case 2: // Insertar
                        string queryPrimeraTabla = crearInsert(tabla);
                        logic.nuevoQuery(queryPrimeraTabla); // Inserta el nuevo registro en la tabla principal.
                        sn.insertarBitacora(idUsuario, "Se insertó en " + tabla, tabla, idAplicacion);

                        string ultimoIdPrimeraTabla = logic.lastID(tabla); // Obtiene el último ID insertado en la tabla principal.

                        // Itera sobre las tablas adicionales para insertar registros relacionados.
                        foreach (string tablaAdicional in listaTablasAdicionales)
                        {
                            if (!string.IsNullOrEmpty(tablaAdicional))
                            {
                                List<(string nombreColumna, bool esAutoIncremental, bool esClaveForanea, bool esTinyInt)> columnasAdicionales = logic.obtenerColumnasYPropiedadesLogica(tablaAdicional);

                                List<string> valoresTablaAdicional = new List<string>();
                                int pos = 1;

                                // Construye las consultas para las tablas adicionales.
                                foreach (var columna in columnasAdicionales)
                                {
                                    string valorCampo;

                                    if (columna.esAutoIncremental)
                                    {
                                        continue;
                                    }
                                    else if (columna.esTinyInt)
                                    {
                                        valorCampo = "1"; // Asigna 1 para campos de tipo TinyInt.
                                    }
                                    else if (columna.esClaveForanea)
                                    {
                                        valorCampo = ultimoIdPrimeraTabla.ToString(); // Usa el ID de la tabla principal como clave foránea.
                                    }
                                    else
                                    {
                                        valorCampo = obtenerDatoCampos(pos); // Obtiene el valor del campo.
                                    }

                                    valoresTablaAdicional.Add($"'{valorCampo}'");
                                    pos++;
                                }

                                // Construye las consultas de inserción para las tablas adicionales.
                                string camposQuery = string.Join(", ", columnasAdicionales.Where(c => !c.esAutoIncremental).Select(c => c.nombreColumna));
                                string valoresQuery = string.Join(", ", valoresTablaAdicional);
                                string queryAdicional = $"INSERT INTO {tablaAdicional} ({camposQuery}) VALUES ({valoresQuery});";
                                queries.Add(queryAdicional);
                            }
                        }

                        // Ejecuta las consultas para insertar datos en múltiples tablas.
                        logic.insertarDatosEnMultiplesTablas(queries);
                        MessageBox.Show("El registro ha sido guardado correctamente.", "Guardado Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    default:
                        MessageBox.Show("Opción no válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                // Actualiza la vista de datos y deshabilita los campos después de la operación.
                actualizardatagriew();
                deshabilitarcampos_y_botones();
                botonesYPermisosSinMensaje();
            }
            catch (Exception ex)
            {
                // Manejo de errores, muestra un mensaje al usuario con los detalles del error.
                MessageBox.Show(
                    "Ocurrió un error durante el guardado del registro.\n\n" +
                    "Detalles del error: " + ex.Message + "\n\n" +
                    "Por favor, intente nuevamente o contacte al administrador del sistema.",
                    "Error en el Guardado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ *****************************



        // Aquí está la función crearInsertFactura

        //******************************************** CODIGO HECHO POR MATY MANCILLA *****************************

        // Este método maneja el evento de clic en una celda del DataGridView.
        // Cuando se hace clic en una celda, se actualizan los controles del formulario (TextBox, DateTimePicker, ComboBox, Button)
        // con los valores correspondientes de la fila seleccionada en el DataGridView.
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = 0; // Índice para recorrer las celdas del DataGridView.
            foreach (Control componente in Controls)
            {
                // Si el control es un TextBox, DateTimePicker o ComboBox, se actualiza su texto con el valor de la celda correspondiente.
                if (componente is TextBox || componente is DateTimePicker || componente is ComboBox)
                {
                    componente.Text = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                    i++; // Incrementa el índice para pasar a la siguiente celda.
                }

                // Si el control es un Button, se actualiza su texto y color de fondo según el valor de la celda (0 o 1).
                if (componente is Button)
                {
                    string var1 = dataGridView1.CurrentRow.Cells[i].Value.ToString();
                    if (var1 == "0")
                    {
                        componente.Text = "Desactivado";
                        componente.BackColor = Color.Red;
                    }
                    if (var1 == "1")
                    {
                        componente.Text = "Activado";
                        componente.BackColor = Color.Green;
                    }
                }
            }
        }
        //******************************************** CODIGO HECHO POR MATY MANCILLA *****************************

        // Este método es un placeholder y no tiene ninguna implementación. Es posible que esté destinado a futuras funcionalidades.
        private void Contenido_Click(object sender, EventArgs e)
        {
            // Código a implementar
        }
        //******************************************** CODIGO HECHO POR EMANUEL BARAHONA *****************************

        // Este método maneja el evento de clic en el botón de ayuda.
        // Intenta abrir un archivo de ayuda (.chm). Si no se encuentra el archivo, muestra un mensaje de error.
        private void Btn_Ayuda_Click_1(object sender, EventArgs e)
        {
            // Construye la ruta completa del archivo de ayuda
            string helpFilePath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\AyudaHTML\AyudaNavegador.chm";

            // Normaliza la ruta para obtenerla de forma absoluta
            helpFilePath = System.IO.Path.GetFullPath(helpFilePath);

            // Verifica si el archivo de ayuda existe antes de intentar abrirlo
            if (System.IO.File.Exists(helpFilePath))
            {
                Help.ShowHelp(this, helpFilePath, "AyudaNav.html");
            }
            else
            {
                // Muestra un mensaje de error si el archivo de ayuda no se encuentra
                MessageBox.Show("No se encontró el archivo de ayuda: " + helpFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Este método maneja el evento de clic en un botón para obtener más ayuda.
        // Verifica si existe una tabla de ayuda en la base de datos y, si no hay errores, abre un formulario de ayuda.
        private void Btn_MasAyuda_Click(object sender, EventArgs e)
        {
            // Verifica la existencia de la tabla de ayuda
            string AyudaOK = logic.TestTabla("ayuda");
            if (AyudaOK == "")
            {
                // Si la tabla existe sin problemas, abre el formulario de ayuda
                Ayudas nuevo = new Ayudas();
                nuevo.Show();
            }
            else
            {
                // Si hay un problema con la tabla, muestra un mensaje de advertencia
                DialogResult validacion = MessageBox.Show(AyudaOK + " \n Solucione este error para continuar...", "Verificación de requisitos", MessageBoxButtons.OK);
                if (validacion == DialogResult.OK)
                {
                    // Acción adicional si el usuario presiona OK (actualmente no implementada)
                }
            }
        }
        //******************************************** CODIGO HECHO POR EMANUEL BARAHONA *****************************

        // Este método es un placeholder y no tiene ninguna implementación. Es posible que esté destinado a futuras funcionalidades.
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Código a implementar
        }

        // Este método es un placeholder y no tiene ninguna implementación. Es posible que esté destinado a futuras funcionalidades.
        private void LblTabla_Click(object sender, EventArgs e)
        {
            // Código a implementar
        }

        //******************************************** CODIGO HECHO POR JOSUE CACAO *****************************

        // Este método maneja el evento de clic en un botón (button7) y oculta el formulario actual.
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide(); // Oculta el formulario actual.
        }

        // Este método maneja el evento de clic en el botón de imprimir.
        // Obtiene la ruta del reporte asociado a la aplicación actual y lo muestra en un visor de reportes.
        private void Btn_Imprimir_Click_1(object sender, EventArgs e)
        {
            // Se crea una instancia del controlador de reportería.
            Capa_Controlador_Reporteria.Controlador controlador = new Capa_Controlador_Reporteria.Controlador();

            // Se obtiene el ID de la aplicación.
            ObtenerIdAplicacion(idAplicacion);

            // Se consulta la ruta del reporte usando el ID de la aplicación.
            string ruta = controlador.queryRuta(idAplicacion);

            // Si la ruta es válida, se abre el visor de reportes con la ruta obtenida.
            if (!string.IsNullOrEmpty(idAplicacion))
            {
                Capa_Vista_Reporteria.visualizar visualizar = new Capa_Vista_Reporteria.visualizar(ruta);
                visualizar.ShowDialog(); // Muestra el reporte en un diálogo modal.
            }
        }

        // Este método maneja el evento de clic en el botón principal de reportes.
        // Abre el menú de reportería.
        private void btn_Reportes_Principal_Click(object sender, EventArgs e)
        {
            menu_reporteria reportes = new menu_reporteria();

            reportes.Show(); // Muestra el formulario del menú de reportes.
        }

        //******************************************** CODIGO HECHO POR JOSUE CACAO *****************************

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ *****************************

        // Este método maneja el evento de clic en el botón de consulta.
        // Verifica los permisos del usuario para determinar si muestra una consulta inteligente o simple.
        private void Btn_Consultar_Click(object sender, EventArgs e)
        {
            // Se obtiene el ID del usuario.
            string idUsuario1 = logic.ObtenerIdUsuario(idUsuario);
            //DLL DE CONSULTAS

            ConsultaSimple nueva = new ConsultaSimple(tabla);
            nueva.Show();

            // Habilita o deshabilita botones según los permisos del usuario.
            botonesYPermisosSinMensaje();
        }

        //******************************************** CODIGO HECHO POR BRAYAN HERNANDEZ *****************************


        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS *****************************

        // Este método maneja el evento de clic en el botón principal de reportes.
        // Abre el menú de reportería.
        private void btn_Reportes_Principal_Click_1(object sender, EventArgs e)
        {
            // Se crea una instancia del formulario de menú de reportería.
            menu_reporteria reportes = new menu_reporteria();

            // Muestra el formulario del menú de reportería.
            reportes.Show();
        }

        // Este método maneja el evento de clic en el botón de ayuda (button1).
        // Abre el formulario de ayudas.
        private void button1_Click(object sender, EventArgs e)
        {
            // Se crea una instancia del formulario de ayudas.
            Ayudas ayudas = new Ayudas();

            // Muestra el formulario de ayudas.
            ayudas.Show();
        }

        //******************************************** CODIGO HECHO POR VICTOR CASTELLANOS *****************************
    }
}