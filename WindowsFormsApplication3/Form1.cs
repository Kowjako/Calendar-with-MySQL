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
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
            calendar1.CalendarDate = DateTime.Now;
            calendar1.CalendarView = CalendarViews.Month;
            calendar1.ShowEventTooltips = false;
        }
        private void getEventFromSQL()
        {
            IEvent tmpevent;
            DataBase db = new DataBase();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `events`", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(dt);
            DataRow[] rows = dt.Select();
            for (int i = 0; i < rows.Length; i++)
            {
                string text = rows[i]["text"].ToString();
                string year = rows[i]["year"].ToString();
                string month = rows[i]["month"].ToString();
                string day = rows[i]["day"].ToString();
                string hour = rows[i]["hour"].ToString();
                string min = rows[i]["minute"].ToString();
                string dur = rows[i]["length"].ToString();
                string color = rows[i]["color"].ToString();
                int hash = color.GetHashCode();
                int r = (hash & 0xFF0000) >> 16;
                int g = (hash & 0x00FF00) >> 8;
                int b = hash & 0x0000FF;
                tmpevent = new CustomEvent
                {
                    IgnoreTimeComponent = false,
                    EventText = text,
                    Date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day), Convert.ToInt32(hour),
                    Convert.ToInt32(min), 0),
                    EventLengthInHours = float.Parse(dur, CultureInfo.InvariantCulture.NumberFormat),
                    RecurringFrequency = RecurringFrequencies.None,
                    EventFont = new Font("Verdana", 11, FontStyle.Regular),
                    Enabled = true,
                    EventColor = Color.FromArgb(r, g, b),
                    EventTextColor = Color.Black
                };
                calendar1.AddEvent(tmpevent);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            getEventFromSQL();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateEvent form1 = new CreateEvent();
            if (form1.ShowDialog() != DialogResult.OK)
                calendar1.AddEvent(form1.tmpevent);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (calendar1.CalendarView == CalendarViews.Month)
                calendar1.CalendarView = CalendarViews.Day;
            else calendar1.CalendarView = CalendarViews.Month;
        }
    }
}
