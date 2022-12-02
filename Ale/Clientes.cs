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
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
        }MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        int currentId = 0;


        private void Clientes_Load(object sender, EventArgs e)
        {
            loadClientes();
        }

        private void loadClientes() {
            conn.Open();
            String query = "select * from cliente where nombre like '" + textBoxBuscar.Text + "%' and estado='habilitado'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Nombre";
                dataGridView1.Columns[2].HeaderText = "Telefono";
                dataGridView1.Columns[3].HeaderText = "Direccion";
                dataGridView1.Columns[4].Visible = false;



            }
            conn.Close();
        
        
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String nombre = textBoxNombre.Text;
            String telefono = textBoxTel.Text;
            String direccion = textBoxDire.Text;
            String query = "insert into cliente(nombre, telefono, direccion) values ('" + nombre + "', '" + telefono + "', '" + direccion + "')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadClientes();
            textBoxNombre.Clear();
            textBoxDire.Clear();
            textBoxTel.Clear();
        }

        private void textBoxBuscar_TextChanged(object sender, EventArgs e)
        {
            loadClientes();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxNombre.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
            currentId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            textBoxTel.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
            textBoxDire.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString();
            buttonAgregar.Enabled = false;
            buttonCancelar.Enabled = true;
            buttonEliminar.Enabled = true;
            buttonModificar.Enabled = true;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            textBoxNombre.Clear();
            textBoxDire.Clear();
            textBoxTel.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "update cliente set nombre = '" + textBoxNombre.Text + "', telefono='"+textBoxTel.Text+"', direccion ='"+textBoxDire.Text+"' where idCliente = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadClientes();

            textBoxNombre.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;


            textBoxNombre.Clear();
            textBoxDire.Clear();
            textBoxTel.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "update cliente set estado='no' where idCliente = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            loadClientes();


            textBoxNombre.Clear();
            textBoxDire.Clear();
            textBoxTel.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }
    }
}
