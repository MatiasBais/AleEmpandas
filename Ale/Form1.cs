using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Ale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int semana = 0;
        int semanaoffset = -1;
        int cargo = 0;
        int año = 2022;
        public static int idcliente = 0;
        public static String option = "cancelar";
        public static int idNuevoPedido = 0; MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        public static DateTime semanaa;

        private void buttonCat_Click(object sender, EventArgs e)
        {
            Categorias cat = new Categorias();
            cat.ShowDialog();
        }

        private void buttonProductos_Click(object sender, EventArgs e)
        {
            Productos prod = new Productos();
            prod.ShowDialog();
        }

        private void buttonClientes_Click(object sender, EventArgs e)
        {
            Clientes cli = new Clientes();
            cli.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Categorias cat = new Categorias();
            cat.ShowDialog();
        }

        private void productosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Productos prod = new Productos();
            prod.ShowDialog();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clientes cli = new Clientes();
            cli.ShowDialog();
        }

        static DateTime GetDateFromWeekNumberAndDayOfWeek(int weekNumber, int dayOfWeek, int año)
        {
            DateTime jan1 = new DateTime(año, 1, 1);
            int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekNumber;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstMonday.AddDays(weekNum * 7 + dayOfWeek - 1);
            return result;
        }

        private void updateDate()
        {
            cargo = 0;

            DateTime theDate = GetDateFromWeekNumberAndDayOfWeek(semana, 1, año);
            dateTimePicker1.Value = theDate;
            loadPedidos();
            cargo=1;
        }

        private void loadPedidos()
        {

            DateTime fechaHasta = dateTimePicker1.Value;
            fechaHasta = fechaHasta.AddDays(7);
            conn.Open();
            String query = "SELECT pedido.idPedido, cliente.idCliente, cliente.Nombre, cliente.Telefono, cliente.Direccion, sum(cantidad) as 'Cant', sum(cantidad*valor) as 'Total', pago, entregado as 'Entregado'  FROM pedido join cliente on pedido.idcliente=cliente.idcliente join pedidoRenglon on pedidoRenglon.idPedido = pedido.idPedido join producto p on p.idProducto = pedidoRenglon.idProducto inner join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on p.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = p.idProducto and pr.fechaDesde = maxprec2.fec where Fecha >= '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "' and Fecha < '" + fechaHasta.Year + "/" + fechaHasta.Month + "/" + fechaHasta.Day + "' and cliente.nombre like '"+textBoxCienteBuscar.Text+"%' group by idPedido";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].HeaderText = "Cliente";
                dataGridView1.Columns[3].HeaderText = "Teléfono";
                dataGridView1.Columns[4].HeaderText = "Dirección";
                dataGridView1.Columns[7].HeaderText = "Pagó?";

                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 45;
                dataGridView1.Columns[4].Width = 90;
                dataGridView1.Columns[5].Width = 30;
                dataGridView1.Columns[6].Width = 30;
                dataGridView1.Columns[7].Width = 35;
                dataGridView1.Columns[8].Width = 60;


            }
            conn.Close();
            conn.Open();
            int entregado = 2;
            if (radioButton2.Checked)
                entregado = 1;
            query = "SELECT concat(unidad,' ',categoria.nombre,' ', p.nombre) as 'Producto', sum(cantidad) as 'Cantidad', sum(cantidad*valor) as 'SubTotal' FROM pedido join pedidoRenglon on pedidoRenglon.idPedido = pedido.idPedido join producto p on p.idProducto = pedidorenglon.idProducto join categoria on categoria.idCategoria=p.idCategoria inner join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on p.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = p.idProducto and pr.fechaDesde = maxprec2.fec where Fecha >= '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "' and Fecha < '" + fechaHasta.Year + "/" + fechaHasta.Month + "/" + fechaHasta.Day + "' and entregado !=" + entregado + " group by p.idProducto  order by p.nombre desc,unidad desc";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView2.DataSource = dset.Tables[0];
                dataGridView2.Columns[0].Width = 120;
                dataGridView2.Columns[1].Width = 40;
                dataGridView2.Columns[2].Width = 40;


            }
            conn.Close();





        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            semanaoffset = Convert.ToInt32(numericUpDown1.Value);
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            semana = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW) + semanaoffset;
            updateDate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = semanaoffset;
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            semana = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW) + semanaoffset;
            // Displays the number of the current week relative to the beginning of the year.
            updateDate();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SelectCliente scl = new SelectCliente();
            scl.ShowDialog();
            if (option == "aceptar" && idcliente != 0)
            {

                conn.Open();
                String query = "insert into pedido(idCliente, fecha) values(" + idcliente+", '"+ + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "')";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                idNuevoPedido = Convert.ToInt32(cmd.LastInsertedId);
                conn.Close();



                NuevoPedido nuevopedido = new NuevoPedido();
                nuevopedido.ShowDialog();
            }

            idcliente = 0;
            option = "cancelar";
            loadPedidos();
        }

        private void textBoxCienteBuscar_TextChanged(object sender, EventArgs e)
        {
            loadPedidos();
        }

        private void nuevoPedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectCliente scl = new SelectCliente();
            scl.ShowDialog();
            if (option == "aceptar" && idcliente != 0)
            {

                conn.Open();
                String query = "insert into pedido(idCliente) values(" + idcliente + ")";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                idNuevoPedido = Convert.ToInt32(cmd.LastInsertedId);
                conn.Close();



                NuevoPedido nuevopedido = new NuevoPedido();
                nuevopedido.ShowDialog();
            }

            idcliente = 0;
            option = "cancelar";
            loadPedidos();
        }

        private void gastosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Insumos gastos = new Insumos();
            gastos.ShowDialog();

        }

        private void resumenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Resumen res = new Resumen();
            res.ShowDialog();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            updateDate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            updateDate();
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            
        }
        private void change() {
            int pago = 0;
            if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value))
                pago = 1;
            int entregado = 0;
            if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value))
                entregado = 1;
            try
            {
                conn.Open();
                string query = "update pedido set pago=" + pago + ", entregado=" + entregado + " where idPedido=" + dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value;
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
            updateDate();
        
        
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

                case Keys.F2:
                    if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value))
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value = 0;
                    else
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value = 1;
                    change();
                    break;
                case Keys.F3:
                    if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value))
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value = 0;
                    else
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value = 1;
                    change();
                    break;
                case Keys.F4:
                    if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value))
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value = 0;
                    else
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[8].Value = 1;
                    if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value))
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value = 0;
                    else
                        dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[7].Value = 1;
                    change();
                    break;

                default:
                    
                    break;
            }

            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idNuevoPedido = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value);

            NuevoPedido nuevopedido = new NuevoPedido();
            nuevopedido.ShowDialog();
            loadPedidos();
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            semanaa = dateTimePicker1.Value;
        }
    }
}
