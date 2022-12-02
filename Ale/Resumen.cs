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
    public partial class Resumen : Form
    {
        public Resumen()
        {
            InitializeComponent();
        }

        int semana = 0;
        int semanaoffset = -1;
        int cargo = 0;
        int año = 2022;
        double falta = 0;
        
        private void updateDate()
        {

            
            loadGastos();
            loadPedidos();
            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                total = total + Convert.ToDouble(row.Cells[2].Value);
            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                total = total - Convert.ToDouble(row.Cells[2].Value);

            }
            label6.Text = "Total: " + total.ToString()+"("+falta.ToString()+")";
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

        private void loadGastos()
        {
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
            conn.Open();
            DateTime fechaHasta = dateTimePicker1.Value;
            fechaHasta = fechaHasta.AddDays(7);
            String query = "select * from gasto where Fecha >= '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "' and Fecha < '" + fechaHasta.Year + "/" + fechaHasta.Month + "/" + fechaHasta.Day + "'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView2.DataSource = dset.Tables[0];
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].HeaderText = "Descripción";
                dataGridView2.Columns[2].HeaderText = "Valor";
                dataGridView2.Columns[3].Visible = false;


            }
            conn.Close();
            double total = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                total = total + Convert.ToDouble(row.Cells[2].Value);

            }
            label5.Text = "SubTotal: " + total.ToString();
        }
        private void loadPedidos()
        {
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
            conn.Open();
            falta = 0;
            DateTime fechaHasta = dateTimePicker1.Value;
            fechaHasta = fechaHasta.AddDays(7);
            String query = "SELECT pedido.idPedido, cliente.idCliente, cliente.Nombre, cliente.Telefono, cliente.Direccion, sum(cantidad) as 'Cant', sum(cantidad*valor) as 'Total', pago  FROM pedido join cliente on pedido.idcliente=cliente.idcliente join pedidoRenglon on pedidoRenglon.idPedido = pedido.idPedido join producto p on p.idProducto = pedidoRenglon.idProducto inner join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on p.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = p.idProducto and pr.fechaDesde = maxprec2.fec where Fecha >= '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "' and Fecha < '" + fechaHasta.Year + "/" + fechaHasta.Month + "/" + fechaHasta.Day + "'  group by idPedido";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];

                
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if(!Convert.ToBoolean(row.Cells[7].Value))
                        falta = falta - Convert.ToDouble(row.Cells[6].Value);

                }


            }
            conn.Close();





            conn.Open();
            query = "SELECT concat(unidad,' ',categoria.nombre,' ', p.nombre) as 'Producto', sum(cantidad) as 'Cantidad', sum(cantidad*valor) as 'SubTotal' FROM pedido join pedidoRenglon on pedidoRenglon.idPedido = pedido.idPedido join producto p on p.idProducto = pedidorenglon.idProducto join categoria on categoria.idCategoria=p.idCategoria inner join (select max(fechaDesde) as fec, idProducto from precio group by precio.idProducto) maxprec2 on p.idProducto = maxprec2.idProducto inner join precio  pr on pr.idProducto = p.idProducto and pr.fechaDesde = maxprec2.fec where Fecha >= '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "' and Fecha < '" + fechaHasta.Year + "/" + fechaHasta.Month + "/" + fechaHasta.Day + "' group by p.idProducto";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];


            }
            conn.Close();
            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                total = total + Convert.ToDouble(row.Cells[2].Value);

            }
            label3.Text = "SubTotal: " + total.ToString()+ "("+falta.ToString()+")";
            dataGridView1.Columns[0].Width = 120;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 40;
            

        }
        private void Resumen_Load(object sender, EventArgs e)
        {

            DateTime theDate = GetDateFromWeekNumberAndDayOfWeek(semana, 1, año);
            DateTime date = Form1.semanaa;
            dateTimePicker1.Value = date;
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
