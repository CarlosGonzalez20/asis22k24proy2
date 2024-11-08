using System;
using System.Data;
using System.Windows.Forms;
using Capa_Controlador_Movimientos;

namespace Capa_Vista_Movimientos
{
    public partial class frm_movimiento_bancario : Form
    {
        private DataTable tiposDeCambio;
        private Controlador Capa_Controlador_Movimientos = new Controlador();

        public frm_movimiento_bancario()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Llenar el ComboBox con las opciones de estado
            cboEstado.Items.Add("Activo");
            cboEstado.Items.Add("Inactivo");

            // Establecer "Activo" como valor predeterminado
            if (cboEstado.Items.Count > 0)
            {
                cboEstado.SelectedIndex = 0; // Establecer "Activo" como seleccionado por defecto
            }

            CargarDatos();
        }

        private void frm_movimiento_bancario_Load(object sender, EventArgs e)
        {
            // No es necesario hacer nada adicional aquí, ya que todo ya se está manejando en el constructor
        }

        private void CargarDatos()
        {
            try
            {
                tiposDeCambio = Capa_Controlador_Movimientos.ObtenerMovi();
                Dgv_Mov.DataSource = tiposDeCambio;
                Dgv_Mov.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para agregar un nuevo movimiento bancario
        private void label1_Click(object sender, EventArgs e)
        {
            // Lógica adicional si es necesaria
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener los valores de los campos
                DateTime fecha = dtpFecha.Value;
                string tipo = txtTipo.Text;
                decimal monto = decimal.Parse(txtMonto.Text);
                string descripcion = txtDescripcion.Text;
                string metodoPago = txtMetodoPago.Text;

                // Asegurarse de que hay un valor seleccionado en el ComboBox
                if (cboEstado.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un estado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convertir el valor seleccionado del ComboBox a un valor booleano
                bool estado = cboEstado.SelectedItem.ToString() == "Activo"; // "Activo" se convierte en true, "Inactivo" en false

                // Llamar al controlador para insertar el movimiento
                bool exito = Capa_Controlador_Movimientos.InsertarMovimiento(fecha, tipo, monto, descripcion, metodoPago, estado);

                if (exito)
                {
                    MessageBox.Show("Movimiento bancario agregado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();  // Refrescar la lista de movimientos
                }
                else
                {
                    MessageBox.Show("Error al agregar el movimiento bancario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Asegúrate de que un registro esté seleccionado en el DataGridView
                if (Dgv_Mov.SelectedRows.Count > 0)
                {
                    // Obtener el ID del movimiento seleccionado
                    int movimientoId = Convert.ToInt32(Dgv_Mov.SelectedRows[0].Cells["CuentaID"].Value);  // Asegúrate de que "CuentaID" es la columna con el ID del movimiento

                    // Llamar al controlador para "eliminar" (actualizar el estado a 0)
                    bool exito = Capa_Controlador_Movimientos.EliminarMovimiento(movimientoId);

                    if (exito)
                    {
                        MessageBox.Show("Movimiento bancario desactivado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();  // Refrescar la lista de movimientos
                    }
                    else
                    {
                        MessageBox.Show("Error al desactivar el movimiento bancario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor seleccione un movimiento para desactivar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}