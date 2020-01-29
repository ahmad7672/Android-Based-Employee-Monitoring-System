using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Data.SqlClient;

namespace PCBC
{
    public partial class Id : Form
    {
        public Id()
        {
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

        private void Id_Load(object sender, EventArgs e)
        {
            string text = GetMACAddress();
            string numbers = Regex.Replace(text, @"[^\d]+", "");
            string ne = numbers.Substring(numbers.Length - 3, 3);

            string all = numbers.Remove(numbers.Length-3);
            int num = Convert.ToInt32(all);
            int sum = 0;
            while (num != 0)
            {
                sum += num % 10;
                num /= 10;
            }
            //textBox5.Text = sum.ToString();
            textBox6.Text=sum+ne;
            //textBox1.Text = text;
            //textBox2.Text = numbers;
            //textBox3.Text = ne;
            //textBox4.Text = all;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                SqlCommand cmd = new SqlCommand("Insert into table1 (Name,Id) values ('" + textBox1.Text + "','" + textBox6.Text + "')", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                this.Close();
                Application.Exit();
                System.Diagnostics.Process.Start("PCBC.exe");
            }
            else
            {
                MessageBox.Show("Please Enter Name ","Error !!!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
