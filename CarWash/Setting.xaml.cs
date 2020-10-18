using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarWash
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl

    {
        private const string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|/mydb.mdb;Persist Security Info=True;Jet OLEDB:Database Password=mjm2k4";
        //private const string ConnectionString = "Data Source=(localdb)/MSSQLLocalDB; AttachDbFilename=C:/Users/mjmos/source/repos/WpfApp1/WpfApp1/dbsetting.mdf;Integrated Security=True";
        // C:/Users/mjmos/source/repos/WpfApp1/WpfApp1 

        public Setting()
        {
            InitializeComponent();
            listhaberuz();
            listberuz();
            moshtari();

        }

        private void Rikhtan(string namtable, string fields, string infos)
        {

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            //SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                if (connection.State == ConnectionState.Closed)
                {


                    connection.Open();

                    int i = 0;
                    string query = "SELECT COUNT(*) FROM " + namtable + " WHERE " + fields + " =@kelid ";
                    OleDbCommand Cmd0 = new OleDbCommand();
                    Cmd0.Connection = connection;
                    Cmd0.CommandText = query;

                    Cmd0.Parameters.AddWithValue("@kelid", infos);

                    i = (int)Cmd0.ExecuteScalar();

                    if (i == 0)
                    {

                        query = "INSERT INTO " + namtable + " (" + fields + ") VALUES (@info)";
                        //string query = "INSERT INTO khodro (khodro) VALUES (@namkhodro)";
                        //string query = "INSERT INTO khodro (ID,khodro) VALUES (1,'"+ namkhodro.Text +"' )";
                        //SqlCommand sqlCmd = new SqlCommand(query, connection);
                        OleDbCommand Cmd = new OleDbCommand();
                        Cmd.Connection = connection;
                        Cmd.CommandText = query;

                        //sqlCmd.Parameters.AddWithValue("@namkhordro", namkhodro.Text);
                        Cmd.Parameters.AddWithValue("@namkhordro", infos);


                        //sqlCmd.ExecuteNonQuery();
                        Cmd.ExecuteNonQuery();


                        connection.Close();
                        //MessageBox.Show("done." + query);
                    }
                    else
                    {
                        MessageBox.Show("تکراری است");
                    }
                }
                else
                {
                    MessageBox.Show("faild.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //sqlcon.Close();
                connection.Close();
            }

        }

        private void listhaberuz()
        {
            // beruz khardan listha

            string tabl = "khodro";
            string setun = "khodro";
            string mavad = namkhodro.Text;
            listberuz(tabl, setun, listkhodro);
            cbberuz(tabl, setun, cbkhodro);

            tabl = "service";
            setun = "service";
            mavad = namservice.Text;
            listberuz(tabl, setun, listservice);
            cbberuz(tabl, setun, cbservice);

            tabl = "kargar";
            setun = "kargar";
            mavad = namkargar.Text;
            listberuz(tabl, setun, listkargar);

        }
        public void cbberuz(string namtable, string fields, ComboBox namcb)
        {
            //beruz kardan combobox ha
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            namcb.Items.Clear();
            connection.Open();
            OleDbCommand query = new OleDbCommand("SELECT * FROM " + namtable + " ", connection);
            OleDbDataAdapter adp = new OleDbDataAdapter(query);
            DataTable table = new DataTable();
            adp.Fill(table);
            foreach (DataRow row in table.Rows)
            {

                namcb.Items.Add(row[fields].ToString());


            }
            connection.Close();

        }
        //beruz kardan list moshakhas

            public void moshtari()
        {
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            //string tarikh = refdate();
            OleDbCommand query = new OleDbCommand("SELECT * FROM customer ", connection);

            OleDbDataAdapter adp = new OleDbDataAdapter(query);
            DataTable table = new DataTable("customer");
            adp.Fill(table);
            datagridmoshtari.ItemsSource = table.DefaultView;
            adp.Update(table);
        }
        private void listberuz(string namtable, string fields, ListBox namlist)

        {

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            namlist.Items.Clear();
            connection.Open();
            OleDbCommand query = new OleDbCommand("SELECT * FROM " + namtable + " ", connection);
            OleDbDataAdapter adp = new OleDbDataAdapter(query);
            DataTable table = new DataTable();
            adp.Fill(table);
            foreach (DataRow row in table.Rows)
            {

                namlist.Items.Add(row[fields].ToString());

            }
            connection.Close();



        }
        private void adkhodro_Click(object sender, RoutedEventArgs e)
        {
            string tabl = "khodro";
            string setun = "khodro";
            string mavad = namkhodro.Text;
            Rikhtan(tabl, setun, mavad);
            listberuz(tabl, setun, listkhodro);
            cbberuz(tabl, setun, cbkhodro);
            namkhodro.Text = "";
        }

        private void addservice_Click(object sender, RoutedEventArgs e)
        {
            string tabl = "service";
            string setun = "service";
            string mavad = namservice.Text;
            Rikhtan(tabl, setun, mavad);
            listberuz(tabl, setun, listservice);
            cbberuz(tabl, setun, cbservice);
            namservice.Text = "";
        }

        private void addkargar_Click(object sender, RoutedEventArgs e)
        {
            string tabl = "kargar";
            string setun = "kargar";
            string mavad = namkargar.Text;
            Rikhtan(tabl, setun, mavad);
            listberuz(tabl, setun, listkargar);
            namkargar.Text = "";
        }

        private void del(string namtable, string fields, string infos)
        {


            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            //SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                if (connection.State == ConnectionState.Closed)
                {

                    connection.Open();
                    string query = "DELEte FROM " + namtable + " WHERE " + fields + "=@info";
                    //string query = "INSERT INTO khodro (khodro) VALUES (@namkhodro)";
                    //string query = "INSERT INTO khodro (ID,khodro) VALUES (1,'"+ namkhodro.Text +"' )";
                    //SqlCommand sqlCmd = new SqlCommand(query, connection);
                    OleDbCommand Cmd = new OleDbCommand();
                    Cmd.Connection = connection;
                    Cmd.CommandText = query;

                    //sqlCmd.Parameters.AddWithValue("@namkhordro", namkhodro.Text);
                    Cmd.Parameters.AddWithValue("@namkhordro", infos);


                    //sqlCmd.ExecuteNonQuery();
                    Cmd.ExecuteNonQuery();


                    connection.Close();
                    //MessageBox.Show("done." + query);
                }
                else
                {
                    MessageBox.Show("faild.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //sqlcon.Close();
                connection.Close();
            }
        }
        private void add(string name, string khedmat, string ghimat)
        {

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            //SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                if (connection.State == ConnectionState.Closed)
                {

                    connection.Open();
                    
                    //check for multi value

                    int i = 0;
                    string query = "SELECT COUNT(*) FROM price WHERE khodroID =@khodro AND  serviceID =@service";
                    OleDbCommand Cmd0 = new OleDbCommand();
                    Cmd0.Connection = connection;
                    Cmd0.CommandText = query;

                    Cmd0.Parameters.AddWithValue("@khodro", name);
                    Cmd0.Parameters.AddWithValue("@service", khedmat);
                    i = (int)Cmd0.ExecuteScalar();

                    if (i==0)
                    {

                    query = "INSERT INTO price (khodroID,serviceID,ghimat) VALUES (@khodroID,@serviceID,@ghimat)";
                    OleDbCommand Cmd = new OleDbCommand();
                    //string query = "INSERT INTO khodro (khodro) VALUES (@namkhodro)";
                    //string query = "INSERT INTO khodro (ID,khodro) VALUES (1,'"+ namkhodro.Text +"' )";
                    //SqlCommand sqlCmd = new SqlCommand(query, connection);
                    // OleDbCommand Cmd = new OleDbCommand();
                    Cmd.Connection = connection;
                    Cmd.CommandText = query;

                    //sqlCmd.Parameters.AddWithValue("@namkhordro", namkhodro.Text);
                    Cmd.Parameters.AddWithValue("@KhodroID", name);
                    Cmd.Parameters.AddWithValue("@serviceID", khedmat);
                    Cmd.Parameters.AddWithValue("@ghimat", ghimat);
                    //sqlCmd.ExecuteNonQuery();
                    Cmd.ExecuteNonQuery();
                    
                    ghimatlist.Items.Add(new MyItem { Idkhodro = name, Idkhedmat = khedmat, Idprice = ghimat });
                    MessageBox.Show("هزینه خدمت ثبت شد");
                    }
                    else
                    {
                        OleDbCommand Cmd2 = new OleDbCommand();
                        query = "UPDATE price SET ghimat=@ghimatnew  WHERE khodroID=@khodro AND  serviceID=@service";

                        Cmd2.Connection = connection;
                        Cmd2.CommandText = query;
                        Cmd2.Parameters.AddWithValue("@ghimatnew", ghimat);
                        Cmd2.Parameters.AddWithValue("@khodro", name);
                        Cmd2.Parameters.AddWithValue("@service", khedmat);
                        Cmd2.ExecuteNonQuery();
                        MessageBox.Show("هزینه خدمت بروز شد.");
                    }

                    connection.Close();
                   
                }
                else
                {
                    MessageBox.Show("عملیات به خطا مواجه شد.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //sqlcon.Close();
                listberuz();
                connection.Close();
            }

           
        }


        private void listberuz()
        {

            ghimatlist.Items.Clear();

            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            //SqlConnection connection = new SqlConnection(ConnectionString);
            string query;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    if (cbkhodro.SelectedItem == null)
                    {
                        query = "Select * FROM price";
                    }
                    else
                    {
                        query = "Select * FROM price WHERE khodroID ='" + cbkhodro.Text + "'";
                    }
                    OleDbCommand Cmd = new OleDbCommand();
                    Cmd.Connection = connection;
                    Cmd.CommandText = query;

                    OleDbDataReader reader = Cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader.GetValue(1).ToString();
                        string khedmat = reader["serviceID"].ToString();
                        string ghimat = reader.GetValue(3).ToString();
                        ghimatlist.Items.Add(new MyItem { Idkhodro = name, Idkhedmat = khedmat, Idprice = ghimat });

                    }
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("faild.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //sqlcon.Close();
                connection.Close();
            }

        }


  
        public class MyItem
        {
            public string Idkhodro { get; set; }

            public string Idkhedmat { get; set; }
            public string Idprice { get; set; }
        }
        private void addprice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string name = cbkhodro.SelectedItem.ToString();
                string khedmat = cbservice.SelectedItem.ToString();
                string ghimat = txtghimat.Text;
                if (ghimat != "0")
                {
                    add(name, khedmat, ghimat);
                }
                else
                {
                    MessageBox.Show("لطفا هزینه را وارد نمایید.");
                }
            }
            catch (Exception)
            {
                
                MessageBox.Show("لطفا تمام بخش ها را پر نمایید.");

            }


        }

        private void cbkhodro_DropDownClosed(object sender, EventArgs e)
        {
            listberuz();
        }

        private void txtghimat_GotFocus(object sender, RoutedEventArgs e)
        {
            txtghimat.Text = "";
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void listkhodro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            { 
                namkhodro.Text = listkhodro.SelectedItem.ToString();
            }
            catch (Exception)
            {
                namkhodro.Text = "";

            }
        }

        private void delkhodro_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("در صورت حذف خودرو از لیست ، تمام هزینه های تنظیم شده برای خودرو نیز حذف میگردد. آیا میخواهید این کار صورت بگیرد ؟ ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                string tabl = "khodro";
                string setun = "khodro";
                string mavad = namkhodro.Text;
                del(tabl, setun, mavad);
                tabl = "price";
                setun = "khodroID";
                del(tabl, setun, mavad);
                listberuz(tabl, setun, listkhodro);
                cbberuz(tabl, setun, cbkhodro);
                listhaberuz();
                namkhodro.Text = "";
                MessageBox.Show("خودرو "+ mavad + " از تمام لیست ها حذف شد.");
            }

        }

        private void delservice_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("در صورت حذف یک خدمت از لیست ، تمام هزینه های تنظیم شده برای این خدمت نیز حذف میگردد. آیا میخواهید این کار صورت بگیرد ؟ ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
            string tabl = "service";
            string setun = "service";
            string mavad = namservice.Text;
            del(tabl, setun, mavad);

            tabl = "price";
            setun = "serviceID";
            del(tabl, setun, mavad);
                listberuz(tabl, setun, listservice);
            cbberuz(tabl, setun, cbservice);
            listhaberuz();
             MessageBox.Show("خدمت " + mavad + " از تمام لیست ها حذف شد.");
                namservice.Text = "";
            }
         }

        private void delkargar_Click(object sender, RoutedEventArgs e)
        {
            string tabl = "kargar";
            string setun = "kargar";
            string mavad = namkargar.Text;
            del(tabl, setun, mavad);
            listberuz(tabl, setun, listkargar);
            namkargar.Text = "";
        }

        private void listservice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                namservice.Text = listservice.SelectedItem.ToString();
            }
            catch (Exception)
            {
                namservice.Text = "";

            }
        }

        private void listkargar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                namkargar.Text = listkargar.SelectedItem.ToString();
            }
            catch (Exception)
            {
                namkargar.Text = "";

            }
        }

        private void namkhodro_GotFocus(object sender, RoutedEventArgs e)
        {
            namkhodro.Text = "";
        }

        private void namservice_GotFocus(object sender, RoutedEventArgs e)
        {
            namservice.Text = "";
        }

        private void namkargar_GotFocus(object sender, RoutedEventArgs e)
        {
            namkargar.Text = "";
        }

        private void findmoshtari_Click(object sender, RoutedEventArgs e)
        {
            if (txtnum.Text != "")
            {
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                string shomare = Convert.ToInt64(txtnum.Text).ToString();
                OleDbCommand query = new OleDbCommand("SELECT * FROM customer WHERE tell ='" + shomare + "'", connection);

                OleDbDataAdapter adp = new OleDbDataAdapter(query);
                DataTable table = new DataTable("customer");
                adp.Fill(table);
                datagridmoshtari.ItemsSource = table.DefaultView;
                adp.Update(table);
                connection.Close();
            }

        }

        private void delmoshtari_Click(object sender, RoutedEventArgs e)
        {
            string ID;

            DataGrid gd = (DataGrid)datagridmoshtari;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                ID = row_selected["ID"].ToString();
                string tel = row_selected["tell"].ToString();
                try
                {
                    if (MessageBox.Show("ایا میخواهید شماره " + tel + " از لیست حذف گردد ؟ ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        moshtari();
                    }
                    else
                    {

                        OleDbConnection connection = new OleDbConnection();
                        connection.ConnectionString = ConnectionString;
                        connection.Open();
                        OleDbCommand Cmd2 = new OleDbCommand();
                        string query = "DELETE FROM customer WHERE ID=@id";

                        Cmd2.Connection = connection;
                        Cmd2.CommandText = query;

                        Cmd2.Parameters.AddWithValue("@ID", ID);

                        Cmd2.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show(" موردانتخاب شده حذف شد.");
                        moshtari();

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }


}
