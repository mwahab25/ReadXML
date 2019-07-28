using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;
using System.IO;

namespace ReadWriteXML
{
    public partial class Form1 : Form
    {
        private SqlDataAdapter da;
        private SqlConnection conn;
        BindingSource bsource = new BindingSource();
        SqlCommandBuilder scb;
        DataSet ds = null;
        string sql;
        bool flag;
        //string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HandheldDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string connectionString = @"Data Source=.;Initial Catalog=HandheldDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Form1()
        {
            InitializeComponent();
           
        }
        
        void FillData()
        {
            conn = new SqlConnection(connectionString);
            sql = "select * from Tahsel";

            da = new SqlDataAdapter(sql, conn);
            conn.Open();
            ds = new DataSet();
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            da.Fill(ds, "Tahsel");
            da.Update(ds, "Tahsel");
            bsource.DataSource = ds.Tables["Tahsel"];
            dataGridView_Tahseel.DataSource = bsource;
            dataGridView_Tahseel.Refresh();
        }
        
        void FillBillData()
        {
            conn = new SqlConnection(connectionString);
            sql = "select * from Bills";

            da = new SqlDataAdapter(sql, conn);
            conn.Open();
            ds = new DataSet();
           
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
            da.Fill(ds, "Bills");
            da.Update(ds, "Bills");
            bsource.DataSource = ds.Tables["Bills"];
            dataGridView_Bills.DataSource = bsource;
        }
        
        private void button_Read_Click(object sender, EventArgs e)
        {
            SqlConnection connection;
            SqlCommand command;
            SqlDataAdapter adpter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            XmlReader xmlFile;
            string sql = null;

            int File_No;
            string Customer_Name;
            string Address;
            int Path_Seq;
            string Activity_Type;
            int Area_No;
            string Meter_Status;
            int Cur_Read;

            connection = new SqlConnection(connectionString);

            xmlFile = XmlReader.Create(textbox_Filename.Text, new XmlReaderSettings());
            ds.ReadXml(xmlFile);
            int i = 0;
            connection.Open();


            sql = "select * from Tahsel";
            adpter = new SqlDataAdapter(sql, connection);
            adpter.Fill(ds2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                flag = true;
            }

            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                File_No = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]);
                Customer_Name = ReverseString(ds.Tables[0].Rows[i].ItemArray[1].ToString());
                Address = ReverseString(ds.Tables[0].Rows[i].ItemArray[2].ToString());
                Path_Seq = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]);
                Activity_Type = ReverseString(ds.Tables[0].Rows[i].ItemArray[4].ToString());
                Area_No = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]);
                Meter_Status = ReverseString(ds.Tables[0].Rows[i].ItemArray[6].ToString());
                Cur_Read = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[7]);

                if (flag == true)
                {
                    sql = "update Tahsel set Cur_Read ='" + Cur_Read + "' where File_No = " + File_No;
                }
                else
                {
                   sql = "insert into Tahsel values(" + File_No + ",'" + Customer_Name + "','" + Address + "'," + Path_Seq + ",'" + Activity_Type + "'," + Area_No + ",'" + Meter_Status + "'," + Cur_Read + ")";
                }
                command = new SqlCommand(sql, connection);
                adpter.InsertCommand = command;
                adpter.InsertCommand.ExecuteNonQuery();
            }
            connection.Close();
            MessageBox.Show("Êã ÇáÊÍãíá ÈäÌÇÍ");
            xmlFile.Close();
            FillData();
        }

        private void button_write_Click(object sender, EventArgs e)
        {
            SqlConnection connection;
            SqlDataAdapter adapter;
            DataSet ds = new DataSet();
            string sql = null;

            connection = new SqlConnection(connectionString);
            sql = "select * from Tahsel";
            try
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                adapter.Fill(ds);
                
                connection.Close();
                dataGridView_Tahseel.DataSource = ds.Tables["Tahsel"];
                ds.WriteXml(textbox_Filename.Text);
                


                MessageBox.Show("Êã ÇáÊÍãíá ÈäÌÇÍ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            FillData();

            ChangeNode(textbox_Filename.Text);
           
        }

        private void button_upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog op1 = new OpenFileDialog();
            op1.Multiselect = true;
            op1.ShowDialog();
            op1.Filter = "allfiles|*.xml";
            textbox_Filename.Text = op1.FileName;
            //int count = 0;
            //string[] FName;
            //foreach (string s in op1.FileNames)
            //{
            //    FName = s.Split('\\');
            //    File.Copy(s, "C:\\" + FName[FName.Length - 1]);
            //    count++;
            //}
            //MessageBox.Show(Convert.ToString(count) + " File(s) copied");
        }

        private void button_read2_Click(object sender, EventArgs e)
        {
            SqlConnection connection;
            SqlCommand command;
            SqlDataAdapter adpter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            XmlReader xmlFile;
            string sql = null;

            Int64 File_No;
            string Customer_Name;
            string Address;
            int Area_No;
            string Bill_Date;
            int Bill_Qunt;
            string Cur_State;

            connection = new SqlConnection(connectionString);

            xmlFile = XmlReader.Create(textBox_Filename2.Text, new XmlReaderSettings());
            ds.ReadXml(xmlFile);
            int i = 0;
            connection.Open();
            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                File_No = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                Customer_Name = ReverseString(ds.Tables[0].Rows[i].ItemArray[1].ToString());
                Address = ReverseString(ds.Tables[0].Rows[i].ItemArray[2].ToString());
                Area_No = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]);
                Bill_Date = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                Bill_Qunt = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]);
                if (ds.Tables[0].Rows[i].ItemArray[6].ToString() == "")
                { Cur_State = null; }
                else
                {
                    Cur_State = ds.Tables[0].Rows[i].ItemArray[6].ToString();
                }
              
                sql = "insert into Bills values(" + File_No + ",'" + Customer_Name + "','" + Address + "'," + Area_No + ",'" + Bill_Date + "'," + Bill_Qunt + ",'" + Cur_State + "')";
                //sql = "update bills set Cur_State ='" + Cur_State + "' where File_No = " + File_No ;  
                command = new SqlCommand(sql, connection);
                adpter.InsertCommand = command;
                adpter.InsertCommand.ExecuteNonQuery();
            }
            connection.Close();
            MessageBox.Show("Êã ÇáÊÍãíá ÈäÌÇÍ");
            xmlFile.Close();
            FillBillData();
        }

        private void button_write2_Click(object sender, EventArgs e)
        {
            SqlConnection connection;
            SqlDataAdapter adapter;
            DataSet ds = new DataSet();
            string sql = null;

            connection = new SqlConnection(connectionString);
            sql = "select * from Bills";
            try
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                adapter.Fill(ds);
                connection.Close();
                ds.WriteXml(textBox_Filename2.Text);

                MessageBox.Show("Êã ÇáÊÍãíá ÈäÌÇÍ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            FillBillData();
            ChangeNode(textBox_Filename2.Text);
           
        }

        private void button_upload2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op1 = new OpenFileDialog();
            op1.Multiselect = true;
            op1.ShowDialog();
            op1.Filter = "allfiles|*.xml";
            textBox_Filename2.Text = op1.FileName;
            //int count = 0;
            //string[] FName;
            //foreach (string s in op1.FileNames)
            //{
            //    FName = s.Split('\\');
            //    File.Copy(s, "C:\\" + FName[FName.Length - 1]);
            //    count++;
            //}
            //MessageBox.Show(Convert.ToString(count) + " File(s) copied");
        }

        private static string ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            int j = str.Length - 1;
            for (int i = 0; i < str.Length / 2; i++)
            {
                char c = chars[i];
                chars[i] = chars[j];
                chars[j] = c;
                j--;
            }
            return new string(chars);
        }

        private void ChangeNode(string path)
        {
            

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode oldRoot = doc.SelectSingleNode("NewDataSet");
            XmlNode newRoot = doc.CreateElement("NewDataSet");
            doc.ReplaceChild(newRoot, oldRoot);

            foreach (XmlNode childNode in oldRoot.ChildNodes)
            {
                newRoot.AppendChild(childNode.CloneNode(true));
            }

            XmlNodeList PackageNodeList = newRoot.SelectNodes("Table");

            foreach (XmlNode node in PackageNodeList)
            {
                XmlElement newNode = doc.CreateElement("Table1");
                newRoot.ReplaceChild(newNode, node);

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    XmlNode clonedChildNode = childNode.CloneNode(true);
                    newNode.AppendChild(clonedChildNode);

                    //XmlNode newChildNode = doc.CreateElement("cell");
                    //newNode.ReplaceChild(newChildNode, clonedChildNode);

                    //foreach (XmlNode childChildNode in clonedChildNode.ChildNodes)
                    //{
                    //    newChildNode.AppendChild(childChildNode.CloneNode(true));
                    //}
                }
            }

            //textBox1.Text = doc.OuterXml;

            XmlTextWriter writer = new XmlTextWriter(path, null);
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
            writer.Close();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    scb = new SqlCommandBuilder(da);
            //    da.Update(ds, "Tahsel");
            //    MessageBox.Show("Êã ÇáÊÚÏíá ÈäÌÇÍ");
            //}
            //catch
            //{
            //    MessageBox.Show("ÎØÃ Ýí ÇáÊÚÏíá ¡ ÍÇæá ãÑÉ ÃÎÑí");
            //}

        }

        private void button_update2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    scb = new SqlCommandBuilder(da2);
            //    da2.Update(ds2, "Bills");
            //    MessageBox.Show("Êã ÇáÊÚÏíá ÈäÌÇÍ");
            //}
            //catch
            //{
            //    MessageBox.Show("ÎØÃ Ýí ÇáÊÚÏíá ¡ ÍÇæá ãÑÉ ÃÎÑí");
            //}

        }
    }
}