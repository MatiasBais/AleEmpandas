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
    public partial class SelectCliente : Form
    {
        public SelectCliente()
        {
            InitializeComponent();
        }
        MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        
        private void SelectCliente_Load(object sender, EventArgs e)
        {
            
            conn.Open();
            String query = "select * from cliente where estado='habilitado'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataTable dset = new DataTable();

                adpt.Fill(dset);

                comboBox1.DataSource = dset;
                comboBox1.DisplayMember = "nombre";
                comboBox1.ValueMember = "idCliente";


            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                String aa = comboBox1.SelectedItem.ToString();
                Form1.idcliente = Convert.ToInt32(comboBox1.SelectedValue);
                Form1.option = "aceptar";
                this.Close();
               
            }
            catch {
                try
                {
                    
                    conn.Open();
                    String query = "insert into cliente(nombre) values ('" + comboBox1.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    int id = Convert.ToInt32(cmd.LastInsertedId);
                    conn.Close();
                    Form1.idcliente = id;
                    Form1.option = "aceptar";
                    this.Close();
                }
                catch (Exception error) {
                    MessageBox.Show(error.ToString());
                }
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.idcliente = 0;
            Form1.option = "cancelar";
            this.Close();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                try
                {

                    String aa = comboBox1.SelectedItem.ToString();
                    Form1.idcliente = Convert.ToInt32(comboBox1.SelectedValue);
                    Form1.option = "aceptar";
                    this.Close();

                }
                catch
                {
                    try
                    {

                        conn.Open();
                        String query = "insert into cliente(nombre) values ('" + comboBox1.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.ExecuteNonQuery();
                        int id = Convert.ToInt32(cmd.LastInsertedId);
                        conn.Close();
                        Form1.idcliente = id;
                        Form1.option = "aceptar";

                        this.Close();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.ToString());
                    }
                }

                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
