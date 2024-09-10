using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capa_Controlador_Consulta;
using System.Data.Odbc;
using System.Data;

namespace Capa_Vista_Consulta
{
    public partial class ConsultaInteligente : Form
    {
        consultaControlador csControlador = new consultaControlador();
        string tablabusqueda;
        private string[] datos;
        private string[] tipos;
        private string consultaSeleccionada;
        public ConsultaInteligente()
        {
            InitializeComponent();
            string BD = "consultasBD";
            tipos = new string[] { "nombre_consulta", "tipo_consulta", "consulta_SQLE", "consulta_estatus" };
            csControlador.CargarTablas(cboTabla, BD);
            csControlador.CargarTablas(cboEditarTabla, BD);
            csControlador.CargarTablas(cboTabla, BD);
            cboTabla.SelectedIndexChanged += new EventHandler(cboTabla_SelectedIndexChanged);
            cboEditarTabla.SelectedIndexChanged += new EventHandler(cboTablaEditar_SelectedIndexChanged);
            gbCondiciones.Enabled = false;
            gbOrdenar.Enabled = false;
            gbListadoConsultas.Enabled = false;
            gbEditarLogica.Enabled = false;
            gbEditarOrden.Enabled = false;
            csControlador.obtenerNombresConsultas(txtEditarNombreConsulta);
            txtEditarNombreConsulta.SelectedIndexChanged += new EventHandler(cboConsultas_SelectedIndexChanged);

        }
        string consulta = "";
        string tabla = "tbl_consultaInteligente";
        private void cboTabla_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verifica si se ha seleccionado una tabla
            if (!string.IsNullOrEmpty(cboTabla.Text))
            {
                // Llama al método para llenar el segundo ComboBox con las columnas de la tabla seleccionada
                csControlador.obtenerColumbasPorTabla(cboCampos, cboTabla.Text);
            }
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //boton agregar, consulta simple
            // Datos que se van a procesar
            datos = new string[] { txtNombreConsulta.Text, "0", cboTabla.Text, chbTodosCampos.Text, txtQueryFinal.Text, "1" };

            // Inicializamos la variable para los campos que se mostrarán en el query
            string camposSeleccionados;

            // Verificamos si el CheckBox de 'Todos los campos' está marcado
            if (chbTodosCampos.Checked)
            {
                // Si está marcado, mostramos "Todos los campos"
                camposSeleccionados = "*";
            }
            else
            {
                // Si no está marcado, verificamos si hay un campo seleccionado en el ComboBox
                if (!string.IsNullOrEmpty(cboCampos.Text))
                {
                    // Si hay un campo seleccionado, lo mostramos
                    camposSeleccionados = cboCampos.Text;
                }
                else
                {
                    // Si no hay campo seleccionado, dejamos un valor vacío o un mensaje de advertencia
                    MessageBox.Show("Debe seleccionar o un campo o todos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtQuery.Text))
            {
                // Si txtQuery está vacío, asignamos el valor inicial
                txtQuery.Text = camposSeleccionados + " + " + txtAlias.Text + " + ";
                datos[4] = txtQuery.Text;
            }
            else
            {
                // Si txtQuery ya tiene texto, agregamos el nuevo valor sin repetir el nombre de la consulta y el tipo
                txtQuery.Text += Environment.NewLine + camposSeleccionados + " + " + txtAlias.Text + " + ";
                datos[4] += txtQuery.Text;
            }

            // Procesar los datos en la tabla correspondiente
            csControlador.ingresar(tipos, datos, "tbl_consultaInteligente");
        }

        private void ConsultaInteligente_Load(object sender, EventArgs e)
        {

        }

        private void creciónToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void chbEditarDescendente_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnEditarLogico_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregarComparacion_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCancelarSimple_Click(object sender, EventArgs e)
        {
            txtNombreConsulta.Clear();
            cboTabla.ResetText();
            cboCampos.ResetText();
            txtAlias.Clear();
            chbTodosCampos.Checked = false;
        }

        private void btnCancelarLogica_Click(object sender, EventArgs e)
        {
            cboLogico.ResetText();
            cboLogicoCampo.ResetText();
            txtValorLogico.Clear();
        }

        private void btnCancelarComparacion_Click(object sender, EventArgs e)
        {
            cboComparador.ResetText();
            cboComparadorCampo.ResetText();
            txtValorComparador.Clear();
        }

        private void btnCancelarOrden_Click(object sender, EventArgs e)
        {
            cboOrdenar.ResetText();
            cboOrdenarCampo.ResetText();
            chbOrdenAscendente.Checked = false;
            chbOrdenDescendente.Checked = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNombreConsulta.Clear();
            cboTabla.ResetText();
            cboCampos.ResetText();
            txtAlias.Clear();
            chbTodosCampos.Checked = false;
            cboLogico.ResetText();
            cboLogicoCampo.ResetText();
            txtValorLogico.Clear();
            cboComparador.ResetText();
            cboComparadorCampo.ResetText();
            txtValorComparador.Clear();
            cboOrdenar.ResetText();
            cboOrdenarCampo.ResetText();
            chbOrdenAscendente.Checked = false;
            chbOrdenDescendente.Checked = false;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {

            // Procesar los datos en la tabla correspondiente
            csControlador.ingresarQuery(tipos, datos, tabla);

        }































        //SALVADOR MARTÍNEZ // PESTAÑA DE EDITAR

        private void cboTablaEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verifica si se ha seleccionado una tabla
            if (!string.IsNullOrEmpty(cboEditarTabla.Text))
            {
                // Llama al método para llenar el segundo ComboBox con las columnas de la tabla seleccionada
                csControlador.obtenerColumbasPorTabla(cboEditarCampo, cboEditarTabla.Text);
            }
        }

        private void btnEditarSimple_Click(object sender, EventArgs e)
        {
            //boton agregar, consulta simple
            // Datos que se van a procesar
            datos = new string[] { txtEditarNombreConsulta.Text, "0", cboEditarTabla.Text, chbTodosCampos.Text, txtQueryEditadoFinal.Text, "1" };

            // Inicializamos la variable para los campos que se mostrarán en el query
            string camposSeleccionados;

            // Verificamos si el CheckBox de 'Todos los campos' está marcado
            if (chbEditarTodosCampos.Checked)
            {
                // Si está marcado, mostramos "Todos los campos"
                camposSeleccionados = "*";
            }
            else
            {
                // Si no está marcado, verificamos si hay un campo seleccionado en el ComboBox
                if (!string.IsNullOrEmpty(cboEditarCampo.Text))
                {
                    // Si hay un campo seleccionado, lo mostramos
                    camposSeleccionados = cboEditarCampo.Text;
                }
                else
                {
                    // Si no hay campo seleccionado, dejamos un valor vacío o un mensaje de advertencia
                    MessageBox.Show("Debe seleccionar o un campo o todos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtQueryEditado.Text))
            {
                // Si txtQuery está vacío, asignamos el valor inicial
                txtQueryEditado.Text = camposSeleccionados + " + " + txtEditarAlias.Text + " + ";
                datos[4] = txtQueryEditado.Text;
            }
            else
            {
                // Si txtQuery ya tiene texto, agregamos el nuevo valor sin repetir el nombre de la consulta y el tipo
                txtQueryEditado.Text += Environment.NewLine + camposSeleccionados + " + " + txtEditarAlias.Text + " + ";
                datos[4] += txtQueryEditado.Text;
            }

            // Procesar los datos en la tabla correspondiente
            csControlador.ingresar(tipos, datos, "tbl_consultaInteligente");
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Asegúrate de que consultaSeleccionada tiene un valor válido
            if (string.IsNullOrEmpty(consultaSeleccionada))
            {
                MessageBox.Show("Debe seleccionar una consulta para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Actualiza datos con la consulta seleccionada
            datos[0] = consultaSeleccionada; // Suponiendo que datos[0] es el nombre de la consulta

            // Procesar los datos en la tabla correspondiente
            csControlador.actualizarQuery(tipos, datos, tabla);

        }

        private void cboConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            /// Captura el nombre de la consulta seleccionada desde el ComboBox
            consultaSeleccionada = txtEditarNombreConsulta.SelectedItem.ToString();

        }



    }
}

////////// FIN /////////