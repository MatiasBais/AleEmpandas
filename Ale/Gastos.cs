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
    public partial class Insumos : Form
    {
        public Insumos()
        {
            InitializeComponent();
        }
        int semana = 0;
        int semanaoffset = -1;
        int cargo = 0;
        int año = 2022;
        private void updateDate()
        {

            
            loadGastos();
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

        MySqlConnection conn = new MySqlConnection("server=127.0.0.1;uid=root;database=ale;SslMode = 0;");
        int currentId = 0;
        private void textBoxTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void Insumos_Load(object sender, EventArgs e)
        {
            DateTime theDate = GetDateFromWeekNumberAndDayOfWeek(semana, 1, año);
            dateTimePicker1.Value = Form1.semanaa;
            loadGastos();
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

        private void loadGastos() {
            conn.Open();
            DateTime fechaHasta = dateTimePicker1.Value;
            fechaHasta = fechaHasta.AddDays(7);
            String query = "select * from gasto where Fecha >= '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "' and Fecha < '" + fechaHasta.Year + "/" + fechaHasta.Month + "/" + fechaHasta.Day + "'";
            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Descripción";
                dataGridView1.Columns[2].HeaderText = "Valor";
                dataGridView1.Columns[3].Visible = false;


            }
            conn.Close();
            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                total = total + Convert.ToDouble(row.Cells[2].Value);

            }
            labelTotal.Text = "Total: " + total.ToString();

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

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            conn.Open();
            String desc = textBoxDescripcion.Text;
            String valor = textBoxValor.Text;
            String query = "insert into gasto(descripcion, valor, fecha) values ('" + desc + "', '" + valor + "', '" + dateTimePicker1.Value.Year + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day  +"')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadGastos();
            textBoxDescripcion.Clear();
            textBoxValor.Clear();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxDescripcion.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
            textBoxValor.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
            currentId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            buttonAgregar.Enabled = false;
            buttonCancelar.Enabled = true;
            buttonEliminar.Enabled = true;
            buttonModificar.Enabled = true;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "update gasto set descripcion = '" + textBoxDescripcion.Text + "', valor='"+textBoxValor.Text+"' where idGasto = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadGastos();

            textBoxDescripcion.Clear();
            textBoxValor.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "delete from gasto where idGasto = " + currentId;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            loadGastos();

            textBoxDescripcion.Clear();
            textBoxValor.Clear();
            currentId = 0;
            buttonAgregar.Enabled = true;
            buttonCancelar.Enabled = false;
            buttonEliminar.Enabled = false;
            buttonModificar.Enabled = false;
        }
    }
}
