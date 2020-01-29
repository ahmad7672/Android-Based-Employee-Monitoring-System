using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text.RegularExpressions;
//using System.Net.Mail;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using DirectShowLib;

namespace PCBC
{
    public partial class Form1 : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Emgu.CV.Capture grabber;
 
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        int counter;
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            int x = Screen.PrimaryScreen.WorkingArea.Width + this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height + this.Height;
            this.Bounds = new Rectangle(x, y, this.Width, this.Height);
            counter = 6;
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
        Timer timer = new Timer();
        Timer timer1 = new Timer();
        Timer localTimer = new Timer();

        public string getID()
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
            
            return sum + ne;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlDataAdapter da = new SqlDataAdapter("Select Id from table1 where id = '"+getID()+"'", con);
            con.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string flag = "Old";
            string text = getID();

            int row = ds.Tables[0].Rows.Count;
            if (row == 0)
            {
                flag = "New";
                goto New;
            }

            for (int i = 0; i < row; i++)
            {
                if (text == Convert.ToString(ds.Tables[0].Rows[i][0]))
                {
                    con.Close();
                    flag = "Done";

                    goto New;
                }
                else
                {
                    flag = "New";
                }
            }

        New:

            if (flag == "Done")
            {
                con.Close();
                timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
                timer.Interval = (1000) * (5);              // Timer will tick ever 5 seconds
                timer.Enabled = true;                       // Enable the timer
                timer.Start();


            }
            else if (flag == "New")
            {
                con.Close();
                Id e1 = new Id();
                e1.Show();
            }
            localTimer.Tick += new EventHandler(localTimer_Tick); // Everytime timer ticks, timer_Tick will be called
            localTimer.Interval = (1000) *(60) * (60) * (1);              // Timer will tick after every 1 hour
            localTimer.Enabled = true;                       // Enable the timer
            localTimer.Start();
        }

        void Web()
        {
            SqlConnection con;
            SqlCommand cmd;
            string id = getID();
            //Get the current frame from capture device
            
      //      currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if(_SystemCamereas.Length > 0)
            {
                grabber.QueryFrame(); grabber.QueryFrame();
                currentFrame = grabber.QueryFrame(); //draw the image obtained from camera

                imageBoxFrameGrabber.Image = currentFrame;
                imageBoxFrameGrabber.Image.Save("WebCam.jpg");

                Image img = Image.FromFile("WebCam.jpg");
                MemoryStream tmpStream = new MemoryStream();
                img.Save(tmpStream, ImageFormat.Jpeg);
                byte[] imgBytes = new byte[1304140];
                tmpStream.Read(imgBytes, 0, 1304140);

                byte[] image = null;
                string filepath = "WebCam.jpg";
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                image = br.ReadBytes((int)fs.Length);


                string sql = " Update table1 Set Web = @Imgg where Id='" + id + "'";
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@Imgg", image));
                int x = cmd.ExecuteNonQuery();
                con.Close();
            }
            
        }

        void localTimer_Tick(object sender, EventArgs e)
        {
            localScreenCapture(true);
            //localFetchImages();
        }

        public void localFetchImages()
        {

            if (counter % 5 == 0)
            {

                SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                SqlCommand cmd = new SqlCommand("DELETE TOP (5) FROM pData", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\P\PC control Using Android Over Internet (Ahmad  Waris)\User\User\Database2.mdf;Integrated Security=True");
                cmd = new SqlCommand("DELETE TOP (5) FROM table1", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            string id = getID();
            string name = "";

            SqlConnection conn = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlDataAdapter d = new SqlDataAdapter("Select Id,Name from table1 where id = '" + getID() + "'", conn);
            conn.Open();
            DataTable ds = new DataTable();
            d.Fill(ds);
            
            string text = getID();

            foreach (DataRow row in ds.Rows)
            {
                if(row["ID"].ToString() == id)
                    name= row["Name"].ToString();
            }

            conn.Close();
            using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\P\PC control Using Android Over Internet (Ahmad  Waris)\User\User\Database2.mdf;Integrated Security=True"))
            {
                

                SqlCommand com = new SqlCommand("select top 4 * from table1 order by id desc");
                com.CommandType = CommandType.Text;
                com.Connection = con;
                byte[] scr = null, web = null;
                

                using (SqlDataAdapter da = new SqlDataAdapter(com))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["screenShot"] != DBNull.Value)
                            scr = (byte[])row["screenShot"];
                        else
                            scr = null;
                        if (row["webShot"] != DBNull.Value)
                            web = (byte[])row["webShot"];
                        else
                            web = null;



                        string sql = " insert into pData (screenShot,webShot,clientID)  values(Convert(varbinary(max),@Imgg),Convert(varbinary(max),@Imgg2),'" + name + "')";
                        SqlConnection con2 = new SqlConnection(@"workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                        if (con2.State != ConnectionState.Open)
                            con2.Open();
                        SqlCommand cmd = new SqlCommand(sql, con2);
                        cmd.Parameters.Add(new SqlParameter("@Imgg", scr == null ? (object)DBNull.Value : scr));
                        cmd.Parameters.Add(new SqlParameter("@Imgg2", web == null ? (object)DBNull.Value : web));
                        int x = cmd.ExecuteNonQuery();
                        con2.Close();
                    }
                    
                }
            }
        }
      
        void timer_Tick(object sender, EventArgs e)
        {
            string id=getID(), text = GetMACAddress();
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlCommand cmd;
            SqlDataReader dr;
            cmd = new SqlCommand("Select Control from table1 where Id='" + id + "'", con);
            con.Open();
            dr = cmd.ExecuteReader();
            dr.Read();
            if (Convert.ToString(dr[0]) == "2")
            {
                con.Close();
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                fullScreen();
            }
            else if (Convert.ToString(dr[0]) == "5")
            {
                con.Close();
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                logoff();
            }
            else if (Convert.ToString(dr[0]) == "4")
            {
                con.Close();
                this.WindowState = FormWindowState.Minimized;
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else if (Convert.ToString(dr[0]) == "3")
            {
                con.Close();
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                screenCapture(true);
                this.Close();
                Process.Start("PCBC.exe");
            }
            else if (Convert.ToString(dr[0]) == "6")
            {
                con.Close();
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                Process.Start("shutdown", "/s /t 0");
                
            }
            else if (Convert.ToString(dr[0]) == "7")
            {
                con.Close();
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                
                grabber = new Emgu.CV.Capture();
                grabber.QueryFrame();
                Web();
                this.Close();
                Process.Start("PCBC.exe");
            }
            else if (Convert.ToString(dr[0]) == "8")
            {
                con.Close();
                con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
                cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                localFetchImages();
                Process.Start("PCBC.exe");
            }
        }

        public void screenCapture(bool showCursor)
        {
            
            string id = getID();
            string name = "default", text = GetMACAddress();
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlCommand cmd = new SqlCommand("Update table1 Set Control='1' where Id='" + id + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            
            Point curPos = new Point(Cursor.Position.X, Cursor.Position.Y);
            Size curSize = new Size();
            curSize.Height = Cursor.Current.Size.Height;
            curSize.Width = Cursor.Current.Size.Width;

            //Conceal this form while the screen capture takes place
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.TopMost = false;

            //Allow 250 milliseconds for the screen to repaint itself (we don't want to include this form in the capture)
            System.Threading.Thread.Sleep(250);

            Rectangle bounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));
            string fi = ".jpg";

            ScreenShot.CaptureImage(showCursor, curSize, curPos, Point.Empty, Point.Empty, bounds, name + ".jpeg", fi);

            //The screen has been captured and saved to a file so bring this form back into the foreground
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.TopMost = true;

            Image img = Image.FromFile(name+".jpeg");
            MemoryStream tmpStream = new MemoryStream();
            img.Save(tmpStream, ImageFormat.Jpeg);
            byte[] imgBytes = new byte[1304140];
            tmpStream.Read(imgBytes, 0, 1304140);

            byte[] image = null;
            string filepath = name + ".jpeg";
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            image = br.ReadBytes((int)fs.Length);
            
            
            string sql = " Update table1 Set Snap = @Imgg where Id='" + id + "'";
            con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add(new SqlParameter("@Imgg", image));
            int x = cmd.ExecuteNonQuery();
            con.Close();
            
        }

        public void logoff()
        {
            string id = getID();
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlCommand cmd = new SqlCommand("Update table1 Set Control='1' where ID='" + id + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            ExitWindowsEx(0, 0);
        }

        public void fullScreen()
        {
            string id = getID();
            SqlConnection con = new SqlConnection("workstation id=MyPDBPCC.mssql.somee.com;packet size=4096;user id=cma93_SQLLogin_1;pwd=jztlqk3kqs;data source=MyPDBPCC.mssql.somee.com;persist security info=False;initial catalog=MyPDBPCC");
            SqlCommand cmd = new SqlCommand("Update table1 Set Control='1' where Id='" + id + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            this.WindowState = FormWindowState.Maximized;
        }


        public void localScreenCapture(bool showCursor)
        {
            counter++;
            string id = getID();
            string name = "default", text = GetMACAddress();
            SqlConnection con;
            SqlCommand cmd;
            

            Point curPos = new Point(Cursor.Position.X, Cursor.Position.Y);
            Size curSize = new Size();
            curSize.Height = Cursor.Current.Size.Height;
            curSize.Width = Cursor.Current.Size.Width;

            //Conceal this form while the screen capture takes place
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.TopMost = false;

            //Allow 250 milliseconds for the screen to repaint itself (we don't want to include this form in the capture)
            System.Threading.Thread.Sleep(250);

            Rectangle bounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));
            string fi = ".jpg";

            ScreenShot.CaptureImage(showCursor, curSize, curPos, Point.Empty, Point.Empty, bounds, name + ".jpeg", fi);

            //The screen has been captured and saved to a file so bring this form back into the foreground
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.TopMost = true;

            Image img = Image.FromFile(name + ".jpeg");
            MemoryStream tmpStream = new MemoryStream();
            img.Save(tmpStream, ImageFormat.Jpeg);
            byte[] imgBytes = new byte[1304140];
            tmpStream.Read(imgBytes, 0, 1304140);

            byte[] image = null;
            string filepath = name + ".jpeg";
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            image = br.ReadBytes((int)fs.Length);

            byte[] image2 = localWeb();

            string sql = " insert into table1 (screenSHot,webShot)  values(Convert(varbinary(max),@Imgg),Convert(varbinary(max),@Imgg2))";
            con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\P\PC control Using Android Over Internet (Ahmad  Waris)\User\User\Database2.mdf;Integrated Security=True");
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add(new SqlParameter("@Imgg", image == null ? (object)DBNull.Value : image));
            cmd.Parameters.Add(new SqlParameter("@Imgg2", image2 == null ? (object)DBNull.Value : image2));
            int x = cmd.ExecuteNonQuery();
            con.Close();

        }
        byte[] localWeb()
        {
            string id = getID();
            //Get the current frame from capture device
            byte[] image = null;
            //      currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (_SystemCamereas.Length > 0)
            {
                grabber.QueryFrame(); grabber.QueryFrame();
                currentFrame = grabber.QueryFrame(); //draw the image obtained from camera

                imageBoxFrameGrabber.Image = currentFrame;
                imageBoxFrameGrabber.Image.Save("WebCam.jpg");

                Image img = Image.FromFile("WebCam.jpg");
                MemoryStream tmpStream = new MemoryStream();
                img.Save(tmpStream, ImageFormat.Jpeg);
                byte[] imgBytes = new byte[1304140];
                tmpStream.Read(imgBytes, 0, 1304140);

                
                string filepath = "WebCam.jpg";
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                image = br.ReadBytes((int)fs.Length);          
            }
            return image;

        }

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
    }
}
