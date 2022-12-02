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
    public partial class Categorias : Form
    {
        public Categorias()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        int currentId = 0;


        private void Categorias_Load(object sender, EventArgs e)
        {
            loadCategorias();
        }

        private void loadCategorias()
        {
            conn.Open();
            String query = "select * from categoria where nombre like '"+textBoxBuscar.Text +"%' and estado='habilitado'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Categoría";
                


            }
            conn.Close();
        }

        private void textBoxBuscar_TextChanged(object sender, EventArgs e)
        {
            loadCategorias();
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String nombre = textBoxNombre.Text;
            String query = "insert into categoria(nombre) values ('" + nombre + "')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadCategorias();
            textBoxNombre.Clear();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxNombre.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
            currentId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            buttonAgregar.Enabled = false;
            buttonCancelar.Enabled = true;
            buttonEliminar.Enabled = true;
            buttonModificar.Enabled = true;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {

            textBoxNombre.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "update categoria set nombre = '" + textBoxNombre.Text + "' where idCategoria = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadCategorias();

            textBoxNombre.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "update categoria set estado='no' where idCategoria = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadCategorias();


            textBoxNombre.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

    }
}
