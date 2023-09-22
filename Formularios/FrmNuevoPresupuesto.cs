using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria_2._0.Formularios
{
    public partial class FrmNuevoPresupuesto : Form
    {
        private Presupuesto nuevo;
        public FrmNuevoPresupuesto()
        {
            InitializeComponent();
            nuevo =  new Presupuesto();
        }

        private void FrmNuevoPresupuesto_Load(object sender, EventArgs e)
        {
            TxtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCliente.Text = "CONSUMIDOR FINAL";
            txtDescuento.Text = "0";
            ProximoPresupuesto();
            CargarProductosCombo();
        }

        private void CargarProductosCombo()
        {
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = @"Data Source=localhost;Initial Catalog=Carpinteria_2023;Integrated Security=True";
            conexion.Open();
            SqlCommand comando = new SqlCommand("SP_CONSULTAR_PRODUCTOS", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            conexion.Close();

            CboPruductos.DataSource = tabla;
            CboPruductos.DisplayMember = "n_producto";
            CboPruductos.ValueMember = "id_producto";
        }

        private void ProximoPresupuesto()
        {
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = @"Data Source=localhost;Initial Catalog=Carpinteria_2023;Integrated Security=True";
            conexion.Open();

            SqlCommand comando = new SqlCommand("SP_PROXIMO_ID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter("@next", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            comando.Parameters.Add(param);
            comando.ExecuteNonQuery();
            int next = Convert.ToInt32(param.Value);

            LblPresupuesto.Text += next.ToString();
            conexion.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            //valido que alla algun producto
            if(CboPruductos.Text == "")
            {
                MessageBox.Show("Debe elegir un producto!", "ERROR",MessageBoxButtons.OK);
            }

            //valido que alla cantidad
            if (TxtCantidad.Text == "" || int.Parse(TxtCantidad.Text) == 0 )
            {
                MessageBox.Show("Debe poner una cantidad de productos a comprar!", "ERROR", MessageBoxButtons.OK);
            }

            //valido que el datarowview no se repita
            foreach (DataGridViewRow row in DgvDetalles.Rows)
            {
                if (row.Cells["ColProducto"].Value.ToString().Equals(CboPruductos.Text))
                {
                    MessageBox.Show("Este producto ya esta presupuestado!");
                    return;
                }
            }

            DataRowView item = CboPruductos.SelectedItem as DataRowView;

            int prod = Convert.ToInt32(item.Row.ItemArray[0]);
            string nom = Convert.ToString(item.Row.ItemArray[1]);
            double precio = Convert.ToDouble(item.Row.ItemArray[2]);
            int cant = Convert.ToInt32(TxtCantidad.Text);

            Producto p = new Producto(prod, nom, precio);

            DetallePresupuesto detalle = new DetallePresupuesto(p, cant);

            nuevo.AgregarDetalle(detalle);

            DgvDetalles.Rows.Add(new object[] { prod, nom, precio, cant });

            CalcularTotales();
        }

        private void DgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(DgvDetalles.CurrentCell.ColumnIndex == 4)
            {
                nuevo.QuitarDetalle(DgvDetalles.CurrentRow.Index);
                DgvDetalles.Rows.Remove(DgvDetalles.CurrentRow);
                CalcularTotales();
            }
        }

        private void CalcularTotales()
        {
            txtSubTotal.Text = nuevo.CalcularTotal().ToString();
            double desc = nuevo.CalcularTotal() * Convert.ToDouble(txtDescuento.Text) / 100;
            txtTotal.Text = (nuevo.CalcularTotal() - desc).ToString();
        }
    }
}
