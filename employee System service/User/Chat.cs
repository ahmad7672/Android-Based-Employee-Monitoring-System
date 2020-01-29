using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace PCBC
{
    public partial class Chat : Form
    {
        string id;
        Timer timer = new Timer();
        public Chat(string id1)
        {
            id = id1;
            InitializeComponent();
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        public Chat()
        {
            string text = GetMACAddress();
            string numbers = Regex.Replace(text, @"[^\d]+", "");
            string ne = numbers.Substring(numbers.Length - 3, 3);

            string all = numbers.Remove(numbers.Length - 3);
            int num = Convert.ToInt32(all);
            int sum = 0;
            while (num != 0)
            {
                sum += num % 10;
                num /= 10;
            }

            id= sum + ne;
            InitializeComponent();
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlCommand cmd= new SqlCommand("Update table1 Set AChat='Yes' where Id='" + id + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = label2.Text;
            label2.Text = label3.Text;
            label3.Text = "ME  :-";

            textBox1.Text = textBox2.Text;
            textBox2.Text = textBox3.Text;
            textBox3.Text = textBox4.Text;


            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            string s = "Insert into Chat (ToUser, Uid, UserMsg, DT) values ('1','" + id + "','" + textBox4.Text + "','" + DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/') + " " + DateTime.Now.ToString("HH:mm") + "')";
            SqlCommand cmd = new SqlCommand(s,con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();



            textBox4.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            string s = "SELECT Top 3 chat,achat FROM table1 WHERE Id='" + id + "' Order by DT desc";
            SqlDataAdapter da = new SqlDataAdapter(s, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int rows = ds.Tables[0].Rows.Count;
            if (rows == 3)
            {
                string who = ds.Tables[0].Rows[0][0].ToString();
                if (who == "1")
                {
                    label3.Text = "ME  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    label3.Text = "Admin  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][0].ToString();
                }

                who = ds.Tables[0].Rows[1][0].ToString();
                if (who == "1")
                {
                    label2.Text = "ME  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][2].ToString();
                }
                else
                {
                    label2.Text = "Admin  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][0].ToString();
                }

                who = ds.Tables[0].Rows[2][0].ToString();
                if (who == "1")
                {
                    label1.Text = "ME  :-";
                    textBox1.Text = ds.Tables[0].Rows[2][2].ToString();
                }
                else
                {
                    label1.Text = "Admin  :-";
                    textBox1.Text = ds.Tables[0].Rows[2][0].ToString();
                }
                label3.Visible = true;
                textBox3.Visible = true;

                label2.Visible = true;
                textBox2.Visible = true;

                label1.Visible = true;
                textBox1.Visible = true;
            }
            else if (rows == 2)
            {
                string who = ds.Tables[0].Rows[0][0].ToString();
                if (who == "1")
                {
                    label3.Text = "ME  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    label3.Text = "Admin  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][0].ToString();
                }

                who = ds.Tables[0].Rows[1][0].ToString();
                if (who == "1")
                {
                    label2.Text = "ME  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][2].ToString();
                }
                else
                {
                    label2.Text = "Admin  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][0].ToString();
                }
                label3.Visible = true;
                textBox3.Visible = true;

                label2.Visible = true;
                textBox2.Visible = true;

                label1.Visible = false;
                textBox1.Visible = false;
            }
            else if (rows == 1)
            {
                string who = ds.Tables[0].Rows[0][0].ToString();
                if (who == "1")
                {
                    label3.Text = "ME  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    label3.Text = "Admin  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][0].ToString();
                }
                label3.Visible = true;
                textBox3.Visible = true;

                label1.Visible = false;
                textBox1.Visible = false;
                label2.Visible = false;
                textBox2.Visible = false;
            }
            timer.Tick += new EventHandler(timer1_Tick); // Everytime timer ticks, timer_Tick will be called
            timer.Interval = (1000) * (10);              // Timer will tick evert second
            timer.Enabled = true;                       // Enable the timer
            timer.Start(); 
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            string s = "SELECT Top 3 chat,achat FROM table1 WHERE Id='" + id + "' Order by DT desc";
            SqlDataAdapter da = new SqlDataAdapter(s, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int rows = ds.Tables[0].Rows.Count;
            if (rows == 3)
            {
                string who = ds.Tables[0].Rows[0][0].ToString();
                if (who == "1")
                {
                    label3.Text = "ME  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    label3.Text = "Admin  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][0].ToString();
                }

                who = ds.Tables[0].Rows[1][0].ToString();
                if (who == "1")
                {
                    label2.Text = "ME  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][2].ToString();
                }
                else
                {
                    label2.Text = "Admin  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][0].ToString();
                }

                who = ds.Tables[0].Rows[2][0].ToString();
                if (who == "1")
                {
                    label1.Text = "ME  :-";
                    textBox1.Text = ds.Tables[0].Rows[2][2].ToString();
                }
                else
                {
                    label1.Text = "Admin  :-";
                    textBox1.Text = ds.Tables[0].Rows[2][0].ToString();
                }
                label3.Visible = true;
                textBox3.Visible = true;

                label2.Visible = true;
                textBox2.Visible = true;

                label1.Visible = true;
                textBox1.Visible = true;
            }
            else if (rows == 2)
            {
                string who = ds.Tables[0].Rows[0][0].ToString();
                if (who == "1")
                {
                    label3.Text = "ME  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    label3.Text = "Admin  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][0].ToString();
                }

                who = ds.Tables[0].Rows[1][0].ToString();
                if (who == "1")
                {
                    label2.Text = "ME  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][2].ToString();
                }
                else
                {
                    label2.Text = "Admin  :-";
                    textBox2.Text = ds.Tables[0].Rows[1][0].ToString();
                }
                label3.Visible = true;
                textBox3.Visible = true;

                label2.Visible = true;
                textBox2.Visible = true;

                label1.Visible = false;
                textBox1.Visible = false;
            }
            else if (rows == 1)
            {
                string who = ds.Tables[0].Rows[0][0].ToString();
                if (who == "1")
                {
                    label3.Text = "ME  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    label3.Text = "Admin  :-";
                    textBox3.Text = ds.Tables[0].Rows[0][0].ToString();
                }
                label3.Visible = true;
                textBox3.Visible = true;

                label1.Visible = false;
                textBox1.Visible = false;
                label2.Visible = false;
                textBox2.Visible = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
