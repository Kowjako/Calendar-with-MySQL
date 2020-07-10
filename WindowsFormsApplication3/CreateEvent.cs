using Calendar.NET;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class CreateEvent : MetroFramework.Forms.MetroForm
    {
        public CreateEvent()
        {
            InitializeComponent();
        }
        private Color color;
        public IEvent tmpevent;
        private void button1_Click(object sender, EventArgs e)
        {
            tmpevent = new CustomEvent
            {
                IgnoreTimeComponent = false,
                EventText = textBox1.Text,
                Date = new DateTime(dtp1.Value.Year, dtp1.Value.Month, dtp1.Value.Day, Convert.ToInt32(textBox2.Text.Substring(0, 2)),
                Convert.ToInt32(textBox2.Text.Substring(3, 2)), 0),
                EventLengthInHours = float.Parse(textBox3.Text, CultureInfo.InvariantCulture.NumberFormat),
                RecurringFrequency = RecurringFrequencies.None,
                EventFont = new Font("Verdana", 11, FontStyle.Regular),
                Enabled = true,
                EventColor = Color.FromArgb(color.R, color.G, color.B),
                EventTextColor = Color.Black
            };
            DataBase db = new DataBase();
            MySqlCommand command = new MySqlCommand("INSERT INTO `events` (`text`, `year`, `month`, `day`, `hour`, `minute`, `length`, `color`) VALUES (@uText, @uYear, @uMonth, @uDay, @uHour, @uMin, @uDur, @uColor)", db.getConnection());
            command.Parameters.Add("@uText", MySqlDbType.VarChar).Value = tmpevent.EventText;
            command.Parameters.Add("@uYear", MySqlDbType.VarChar).Value = dtp1.Value.Year;
            command.Parameters.Add("@uMonth", MySqlDbType.VarChar).Value = dtp1.Value.Month;
            command.Parameters.Add("@uDay", MySqlDbType.VarChar).Value = dtp1.Value.Day;
            command.Parameters.Add("@uHour", MySqlDbType.VarChar).Value = Convert.ToInt32(textBox2.Text.Substring(0, 2));
            command.Parameters.Add("@uMin", MySqlDbType.VarChar).Value = Convert.ToInt32(textBox2.Text.Substring(3, 2));
            command.Parameters.Add("@uDur", MySqlDbType.VarChar).Value = tmpevent.EventLengthInHours;
            command.Parameters.Add("@uColor", MySqlDbType.VarChar).Value = tmpevent.EventColor.Name;
            db.startConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Event added to datebase!");
            }
            db.closeConnection();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            color = colorDialog1.Color;
        }
    }
}
