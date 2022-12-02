using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Ale
{
    public partial class Productos : Form
    {
        public Productos()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        int currentId = 0;

        private void textBoxPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Productos_Load(object sender, EventArgs e)
        {
            loadCategorias();
            loadProductos();
        }

        private void loadProductos() {

            conn.Open();
            String query = "select p.idProducto, p.idCategoria, cat.nombre, p.nombre,unidad, valor from producto p inner join categoria cat on cat.idCategoria = p.idCategoria inner join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on p.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = p.idProducto and pr.fechaDesde = maxprec2.fec where p.nombre like '"+textBoxBuscar.Text+"%' and p.estado='habilitado'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataTable dset = new DataTable();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].HeaderText = "Categoría";
                dataGridView1.Columns[3].HeaderText = "Producto";
                dataGridView1.Columns[4].HeaderText = "Unidad";
                dataGridView1.Columns[5].HeaderText = "Precio";



            }
            conn.Close();
        
        }

        private void loadCategorias() {
            
            conn.Open();
            String query = "select * from categoria where estado='habilitado'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataTable dset = new DataTable();

                adpt.Fill(dset);

                comboBoxCategorias.DataSource = dset;
                comboBoxCategorias.ValueMember = "idCategoria";
                comboBoxCategorias.DisplayMember = "nombre";



            }
            conn.Close();
        
        }

        private void buttonCat_Click(object sender, EventArgs e)
        {
            Categorias cat = new Categorias();
            cat.ShowDialog();
            loadCategorias();
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String nombre = textBoxNombre.Text;
            Double precio = Convert.ToDouble(textBoxPrecio.Text);
            int idcat = Convert.ToInt32(comboBoxCategorias.SelectedValue);
            int unidad = Convert.ToInt32(textBoxUnidad.Text);
            String query = "insert into producto(nombre, idCategoria, unidad) values ('"+nombre+"', "+idcat+", '"+ unidad+"');";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            int lastid = Convert.ToInt32(cmd.LastInsertedId);
            query = "insert into precio(idProducto, valor) values ("+lastid+", "+precio+")";
            MySqlCommand cmd2 = new MySqlCommand(query, conn);
            cmd2.ExecuteNonQuery();

            conn.Close();
            loadProductos();
            textBoxNombre.Clear();
            textBoxUnidad.Clear();
            textBoxPrecio.Clear();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxNombre.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString();
            currentId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            comboBoxCategorias.SelectedValue = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value;
            textBoxUnidad.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[4].Value.ToString();
            textBoxPrecio.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[5].Value.ToString();


            buttonAgregar.Enabled = false;
            buttonCancelar.Enabled = true;
            buttonEliminar.Enabled = true;
            buttonModificar.Enabled = true;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            textBoxNombre.Clear();

            textBoxUnidad.Clear();
            textBoxPrecio.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String nombre = textBoxNombre.Text;
            Double precio = Convert.ToDouble(textBoxPrecio.Text);
            int idcat = Convert.ToInt32(comboBoxCategorias.SelectedValue);
            int unidad = Convert.ToInt32(textBoxUnidad.Text);
            string query = "update producto set nombre='" + nombre + "', idCategoria='" + idcat + "', unidad='" + unidad + "' where idProducto="+currentId+";insert into precio(idProducto, valor) values (" + currentId + ", " + precio + ")";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            loadProductos();


            textBoxNombre.Clear();

            textBoxUnidad.Clear();
            textBoxPrecio.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;


        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {


            conn.Open();
            string query = "update producto set estado='no' where idProducto = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();


            loadProductos();

            textBoxNombre.Clear();

            textBoxUnidad.Clear();
            textBoxPrecio.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;

        }

        private void textBoxBuscar_TextChanged(object sender, EventArgs e)
        {
            loadProductos();
        }

        private void textBoxUnidad_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

