using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// Interaction logic for mali.xaml
    /// </summary>
    public partial class mali : UserControl
    {
        
        private readonly string ConnectionString = File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_config.txt"));
        public mali()
        {
            InitializeComponent();
            dgberuz();
            //int[] lcids = new int[] { 1065, 1033, 1025 };
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(1065);
            
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            
            


        }
        public void filter()
        {
            if (cbaztarikh.IsChecked == true || cbtatatikh.IsChecked == true || cbkargar.IsChecked == true || cbnumber.IsChecked == true)
            {
                string filteru = "";
                if (cbnumber.IsChecked == true)
                    filteru += "shomaretell ='" + txthamrah.Text + "' AND ";
                if (cbaztarikh.IsChecked == true  && cbtatatikh.IsChecked == true)
                    filteru += "tarikh BETWEEN'" + txtaztarikh.Text + "' AND '" + txttatarikh.Text + "' AND ";
                if (cbaztarikh.IsChecked == true && cbtatatikh.IsChecked == false)
                    filteru += "tarikh ='" + txtaztarikh.Text + "' AND ";
                if (cbkargar.IsChecked == true)
                    filteru += "kargar ='" + txtkargar.Text + "' AND ";
                filteru = filteru.Remove(filteru.Length - 4);



                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                //string tarikh = refdate();
                OleDbCommand query = new OleDbCommand("SELECT * FROM history WHERE " + filteru, connection);

                OleDbDataAdapter adp = new OleDbDataAdapter(query);
                DataTable table = new DataTable("history");
                adp.Fill(table);
                datagridmali.ItemsSource = table.DefaultView;
                adp.Update(table);
                
                int cash = 0;
                int tedad = 0;
                foreach (DataRow row in table.Rows)
                {
                    tedad = tedad + 1;
                    cash = cash + Convert.ToInt32(row["ghimat"]);
                }
                tedadfilter.Text = tedad.ToString();
                hazinefilter.Text = cash.ToString();
                

                connection.Close();

            }
        }
        public void dgberuz()
        {
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            //string tarikh = refdate();
            OleDbCommand query = new OleDbCommand("SELECT * FROM history", connection);

            OleDbDataAdapter adp = new OleDbDataAdapter(query);
            DataTable table = new DataTable("history");
            adp.Fill(table);
            datagridmali.ItemsSource = table.DefaultView;
            adp.Update(table);
            connection.Close();
        }



  

        private void cbtatatikh_Click(object sender, RoutedEventArgs e)
        {
            if (cbtatatikh.IsChecked == true)
            {
                txttatarikh.IsEnabled = true;
            }
            if (cbtatatikh.IsChecked == false)
            {
                txttatarikh.IsEnabled = false;
            }


        }

        private void cbaztarikh_Click(object sender, RoutedEventArgs e)
        {
            if (cbaztarikh.IsChecked == true)
            {
                //calendar1.IsEnabled = true;
                txtaztarikh.IsEnabled = true;
                cbtatatikh.IsEnabled = true;
                
            }
            if (cbaztarikh.IsChecked == false)
            {
                //calendar1.IsEnabled = false;
                txtaztarikh.IsEnabled = false;
                cbtatatikh.IsEnabled = false;
                txttatarikh.IsEnabled = false;
            }
        }

        private void cbkargar_Click(object sender, RoutedEventArgs e)
        {
            if ( cbkargar.IsChecked == true  )
            {
                txtkargar.IsEnabled = true;
            }
            if (cbkargar.IsChecked == false)
            {
                txtkargar.IsEnabled = false;
            }
        }

        private void cbnumber_Click(object sender, RoutedEventArgs e)
        {
            if (cbnumber.IsChecked == true)
            {
                cbnumber.IsEnabled = true;
            }
            if (cbnumber.IsChecked == false)
            {
                cbnumber.IsEnabled = false;
            }
        }

        private void dofilter_Click(object sender, RoutedEventArgs e)
        {
            filter();
        }

        public string tarikh()
        {
            string numbers = persianCalendar.SelectedDate.ToString();

            string[] output = numbers.Split('/', '/');
            if (output[1].Length == 1)
            {
                output[1] = "0" + output[1];
            }
            if (output[2].Length == 1)
            {
                output[2] = "0" + output[2];
            }
            string result = output[0] + "/" + output[1] + "/" + output[2];

            return result;
        }
        private void txtaztarikh_GotFocus(object sender, RoutedEventArgs e)
        {

            txtaztarikh.Text = tarikh();


        }

        private void txttatarikh_GotFocus(object sender, RoutedEventArgs e)
        {
            txttatarikh.Text = tarikh();
        }

        private void cleanfilter_Click(object sender, RoutedEventArgs e)
        {
            dgberuz();
        }
    }
}
