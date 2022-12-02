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
    public partial class NuevoPedido : Form
    {
        public NuevoPedido()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        int currentId = 0;
        private void NuevoPedido_Load(object sender, EventArgs e)
        {
            loadRenglones();
            loadProductos();
            textBoxCantidad.Text = "1";
        }

        private void loadRenglones() {
            conn.Open();
            String query2 = "select cliente.nombre from pedido join cliente on pedido.idcliente=cliente.idcliente where pedido.idPedido =" + Form1.idNuevoPedido;
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query2, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);
                dataGridView1.DataSource = dset.Tables[0];

                labelNombre.Text = "Pedido Para: " + dataGridView1.Rows[0].Cells[0].Value.ToString();






            }
            conn.Close();
            
            
            
            
            
            conn.Open();
            String query = "select idPedidoRenglon, pedido.idPedido, producto.idProducto, concat(categoria.nombre, ' ', producto.nombre, ' ', unidad) as 'Producto', cantidad, valor as 'Precio por Unidad', valor*cantidad as 'Subtotal' from pedidoRenglon join pedido on pedido.idPedido = pedidoRenglon.idPedido join cliente on cliente.idCliente = pedido.idCliente join producto on producto.idProducto = pedidorenglon.idProducto join categoria on categoria.idCategoria = producto.idCategoria join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on producto.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = producto.idProducto and pr.fechaDesde = maxprec2.fec where pedido.idPedido ="+ Form1.idNuevoPedido + " order by producto.nombre desc";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].HeaderText = "Producto";
                dataGridView1.Columns[4].HeaderText = "Cantidad";
                dataGridView1.Columns[3].Width = 190;
                dataGridView1.Columns[4].Width = 60;
                dataGridView1.Columns[5].Width = 60;
                dataGridView1.Columns[6].Width = 60;



            }
            conn.Close();

            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                total = total + Convert.ToDouble(row.Cells[6].Value);
                
            }
            labelTotal.Text = "Total: " + total.ToString();
        
        }

        private void loadProductos() {

            conn.Open();
            String query = "select p.idProducto, concat(cat.nombre, ' ', p.nombre, ' ', unidad, ' ', valor) as 'Producto' from producto p inner join categoria cat on cat.idCategoria = p.idCategoria inner join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on p.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = p.idProducto and pr.fechaDesde = maxprec2.fec where p.estado='habilitado' order by p.nombre desc";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataTable dset = new DataTable();

                adpt.Fill(dset);

                comboBoxProducto.DataSource = dset;
                comboBoxProducto.ValueMember = "idProducto";
                comboBoxProducto.DisplayMember = "Producto";



            }
            conn.Close();






        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String cantidad = textBoxCantidad.Text;
            String idProducto = comboBoxProducto.SelectedValue.ToString();

            String query = "insert into pedidorenglon(cantidad, idProducto, idPedido) values ('" + cantidad + "' , '" + idProducto + "', '" + Form1.idNuevoPedido + "')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            loadRenglones();
            textBoxCantidad.Text = "1";
        }

        private void textBoxCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxCantidad.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[4].Value.ToString();
            comboBoxProducto.SelectedValue = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value;
            currentId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            buttonAgregar.Enabled = false;
            buttonCancelar.Enabled = true;
            buttonEliminar.Enabled = true;
            buttonModificar.Enabled = true;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {

            textBoxCantidad.Text = "1";
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "update pedidorenglon set cantidad = '" + textBoxCantidad.Text + "', idProducto = '" + comboBoxProducto.SelectedValue.ToString() +"' where idPedidoRenglon = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            loadRenglones();


            textBoxCantidad.Text = "1";
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "delete from pedidoRenglon where idPedidoRenglon = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            loadRenglones();


            textBoxCantidad.Text = "1";
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxCantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (buttonAgregar.Enabled) {
                if (e.KeyCode == Keys.Enter)
                {
                    conn.Open();
                    String cantidad = textBoxCantidad.Text;
                    String idProducto = comboBoxProducto.SelectedValue.ToString();

                    String query = "insert into pedidorenglon(cantidad, idProducto, idPedido) values ('" + cantidad + "' , '" + idProducto + "', '" + Form1.idNuevoPedido + "')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    loadRenglones();
                    textBoxCantidad.Text = "1";
                
                }
            
            }
        }

        private void NuevoPedido_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
