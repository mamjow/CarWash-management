using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.IO;

namespace CarWash
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 

    public partial class carwash : UserControl
    {

        private readonly string ConnectionString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_config.txt"));

        public carwash()
        {
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            listkhadamberuz();
            cbberuz("khodro", "khodro", cbinkhodro);
            cbberuz("kargar", "kargar", cbinkargar);

            formdatagridha();
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


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (cbinreserve.IsChecked == false)
            {
                txtindate.Text = refdate();
                txtintime.Text = DateTime.Now.ToString("HH:mm");
            }


        }
        private void txtintime_GotFocus(object sender, RoutedEventArgs e)
        {
            txtintime.Text = DateTime.Now.ToString("HH:mm");
        }

        private void txtindate_GotFocus(object sender, RoutedEventArgs e)
        {
            txtindate.Text = refdate();

        }
        public string refdate()
        {
            System.Globalization.PersianCalendar pc = new
            System.Globalization.PersianCalendar();
            int mah = pc.GetMonth(DateTime.Now);
            int ruz = pc.GetDayOfMonth(DateTime.Now);
            string maha = pc.GetMonth(DateTime.Now).ToString();
            string ruza = pc.GetDayOfMonth(DateTime.Now).ToString();
            if (mah < 10)
                maha = "0" + mah.ToString();
            if (ruz < 10)
                ruza = "0" + ruz.ToString();
            string emruz = pc.GetYear(DateTime.Now) + "/" + maha + "/" + ruza;
            return emruz;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            txtindate.IsEnabled = true;
            txtintime.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            txtindate.IsEnabled = false;
            txtintime.IsEnabled = false;


        }
        private void listkhadamberuz()
        {
            listkhadamat.Items.Clear();
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    //TheList = new ObservableCollection<BoolStringClass>();
                    connection.Open();
                    OleDbCommand query = new OleDbCommand("SELECT * FROM service", connection);
                    OleDbDataAdapter adp = new OleDbDataAdapter(query);
                    DataTable table = new DataTable();
                    adp.Fill(table);
                    foreach (DataRow row in table.Rows)
                    {
                        listkhadamat.Items.Add(row["service"].ToString());//new CheckedListItem { Namekh = row["service"].ToString() });
                                                                          //TheList.Add(new BoolStringClass { TheText = row["service"].ToString() });


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
                connection.Close();
            }

        }

        private void listkhadamat_LostFocus(object sender, RoutedEventArgs e)
        {
            mohasebe();
        }
        public string mohasebe()
        {
            string khadamat="";
            string query;
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            
            String strItem;

            txthazinekol.Text = "0";
            int hazinekol = Convert.ToInt32(txthazinekol.Text);
            if (cbinkhodro.Text != "")
            {
                int sh = 0;
                foreach (Object selecteditem in listkhadamat.SelectedItems)
                {
                    sh = sh + 1;
                    strItem = selecteditem as String;
                    //MessageBox.Show(strItem);
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                            query = "SELECT * FROM price WHERE khodroID =@khodro AND  serviceID =@service";
                            OleDbCommand Cmd = new OleDbCommand();

                            Cmd.Parameters.AddWithValue("@khodro", cbinkhodro.Text);
                            Cmd.Parameters.AddWithValue("@service", strItem);
                            Cmd.Connection = connection;
                            Cmd.CommandText = query;

                            OleDbDataReader reader = Cmd.ExecuteReader();
                            reader.Read();
                            //   string name = reader.GetValue(1).ToString();

                            int ghimat = Convert.ToInt32(reader[3].ToString());
                            hazinekol = hazinekol + ghimat;
                            khadamat = khadamat + "-" + strItem;
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("faild.");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToString() == "No data exists for the row/column.")
                        {
                            MessageBox.Show("اطلاعات کافی برای خدمت " + strItem + "  مربوط به خودروی " + cbinkhodro.Text + " وجود ندارد.");
                        }
                        else
                        {
                            MessageBox.Show(ex.Message);
                        }
                        txthazinekol.Text = "0";
                        hazinekol = 0;
                        connection.Close();

                    }
                    finally
                    {
                        txthazinekol.Text = hazinekol.ToString();
                    }

                }


            }
            else
            {
                MessageBox.Show("خودرو انتخاب نشده است.");
                listkhadamat.SelectedItems.Clear();
            }
            
            if (txthazinekol.Text == "0")
            {
                listkhadamat.SelectedItems.Clear();
            }
            return khadamat;
        }
        public string ozviat(string shomare)
        {
            string result ="0"; 
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            shomare = Convert.ToInt64(shomare).ToString();//baraye hazf 0  avale shomare
            try
            {
                if (connection.State == ConnectionState.Closed)
                {

                    connection.Open();

                    //check for multi value

                    int i = 0;
                    string query = "SELECT COUNT(*) FROM customer WHERE tell =@tel";
                    OleDbCommand Cmd0 = new OleDbCommand();
                    Cmd0.Connection = connection;
                    Cmd0.CommandText = query;

                    Cmd0.Parameters.AddWithValue("@tel", shomare);

                    i = (int)Cmd0.ExecuteScalar();

                    if (i == 0)
                    {
                        result= "";
                     
                    }
                    else
                    {
                        OleDbCommand Cmd2 = new OleDbCommand();
                        query = "SELECT * FROM customer WHERE tell =@tel";

                        Cmd2.Connection = connection;
                        Cmd2.CommandText = query;
                        Cmd2.Parameters.AddWithValue("@tel", shomare);

                        OleDbDataReader reader = Cmd2.ExecuteReader();
                        reader.Read();
                        string namemoshtari = reader[1].ToString();

                        result = namemoshtari;

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
                
                connection.Close();
            }
            return result;
        }

        private void txtinname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtintell.Text != "")
            {
               string shomare = txtintell.Text;
                shomare = Convert.ToInt64(shomare).ToString();
                string moshtari = ozviat(shomare);
                if (moshtari == "" && txtinname.Text != "")
                {
                    moshtari = txtinname.Text;
                    if (MessageBox.Show("عضویت شماره " + shomare + " به نام " + moshtari + " صورت بگیرد ؟ ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                    }
                    else
                    {
                        OleDbConnection connection = new OleDbConnection();
                        connection.ConnectionString = ConnectionString;
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                                string query = "INSERT INTO customer (name,tell) VALUES (@name,@tel)";
                                OleDbCommand Cmd = new OleDbCommand();
                                Cmd.Connection = connection;
                                Cmd.CommandText = query;
                                Cmd.Parameters.AddWithValue("@name", moshtari);
                                Cmd.Parameters.AddWithValue("@tel", Convert.ToInt64(shomare));
                                Cmd.ExecuteNonQuery();
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
                            //MessageBox.Show("shodam");
                            connection.Close();
                        }

                    }
                }

            }
        }

        private void txtinname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtintell.Text != "")
            {
                txtinname.Text = ozviat(txtintell.Text);
            }
            else
            {
                MessageBox.Show("لطفا ابتدا شماره همراه مشتری را وارد نمایید.");
            }
        }

        private void txtintell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtintell_LostFocus(object sender, RoutedEventArgs e)
        {
            
            string shomare = txtintell.Text;
            if (shomare != "")
            {
                shomare = Convert.ToInt64(shomare).ToString();
                if (shomare.Length != 10)
                {
                    MessageBox.Show("تعداد ارقام شماره وارد شده ناصحیح میباشد.");
                    txtintell.Text = "";
                }
            }

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^ا-ی]+");
            e.Handled = regex.IsMatch(e.Text);
        }



        private void pelakcode_GotFocus(object sender, RoutedEventArgs e)
        {
            pelakcode.Text = "";
        }

        private void pelak1_GotFocus(object sender, RoutedEventArgs e)
        {
            pelak1.Text = "";
        }

        private void pelak2_GotFocus(object sender, RoutedEventArgs e)
        {
            pelak2.Text = "";
        }

        private void pelak3_GotFocus(object sender, RoutedEventArgs e)
        {
            pelak3.Text = "";
        }

        public string barrasi()
        {
            string natije="";
            string empty = "";
            string pelak = pelakcode.Text + "(" + pelak1.Text + pelak2.Text + pelak3.Text + ")";
            if (cbinkargar.Text != empty && cbinkhodro.Text != empty && txthazinekol.Text != "0" && txtinname.Text != empty && txtintell.Text != empty && txtindate.Text != empty && txtintime.Text != empty && pelak.Length ==10)
            {
                natije = "ok";
            }
            else
                natije = "notok";
            return natije;
        }


        private void addin_Click(object sender, RoutedEventArgs e)
        {
            string khadamat = mohasebe();
            string pelak = pelakcode.Text + "(" + pelak1.Text + pelak2.Text + pelak3.Text + ")"; 
            string tarikh = txtindate.Text;
            string intime = txtintime.Text;
            string outtime = "";
            string statuskar = "darhal";
            if (cbinreserve.IsChecked == true )
                    {
                statuskar = "reserve";
                //MessageBox.Show("chekcdد.");
                //sabt(khadamat, pelak, tarikh, intime, outtime, statuskar);
            }

            if (barrasi() == "ok")
            {
                sabt(khadamat,pelak,tarikh,intime,outtime,statuskar);
                //MessageBox.Show("go sabt");
            }
            else if (pelak.Length !=10)
            {
                MessageBox.Show("لطفا شماره پلاک را صحیح وارد کنید.");
            }
            else
                MessageBox.Show("لطفا اطلاعات را صحیح وارد کنید.");

        }

        public void sabt(string khadamat,string pelak,string tarikh,string intime ,string outtime,string statusekar)
        {
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    if (statusekar == "darhal" || statusekar == "reserve")
                    {
                    string query = "INSERT INTO history (khodro,tarikh,intime,kargar,ghimat,khadamat,shomaretell,pelak,statuskar) VALUES (@khodro,@tarikh,@intime,@kargar,@ghimat,@khadmat,@shomaretell,@pelak,@statuskar)";
                    OleDbCommand Cmd = new OleDbCommand();
                    Cmd.Connection = connection;
                    Cmd.CommandText = query;
                    Cmd.Parameters.AddWithValue("@khodro", cbinkhodro.Text);
                    Cmd.Parameters.AddWithValue("@tarikh", tarikh);//----
                    Cmd.Parameters.AddWithValue("@intime", intime);//---
                    Cmd.Parameters.AddWithValue("@kargar", cbinkargar.Text);
                    Cmd.Parameters.AddWithValue("@ghimat",txthazinekol.Text);
                    Cmd.Parameters.AddWithValue("@khadamat",khadamat);//---
                    Cmd.Parameters.AddWithValue("@shomaretell", Convert.ToInt64(txtintell.Text));
                    Cmd.Parameters.AddWithValue("@pelak",pelak);//---
                    Cmd.Parameters.AddWithValue("@statuskar",statusekar);//---
                    Cmd.ExecuteNonQuery();
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
                MessageBox.Show("ثبت شد.");
                connection.Close();
            }
        }

        public void dgberuz()
        {
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            string tarikh = refdate();
            OleDbCommand query = new OleDbCommand("SELECT * FROM history WHERE statuskar = 'takmil' AND tarikh='" + tarikh + "'", connection);

            OleDbDataAdapter adp = new OleDbDataAdapter(query);
            DataTable table = new DataTable("history");
            adp.Fill(table);
            datagridkar.ItemsSource = table.DefaultView;
            adp.Update(table);
            connection.Close();
        }
        public void dgreservberuz()
        {
            try
            {

                OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            //string baghi =refdate() ;
            OleDbCommand query = new OleDbCommand("SELECT * FROM history WHERE statuskar = 'reserve' ", connection);

            OleDbDataAdapter adpc = new OleDbDataAdapter(query);
            DataTable table = new DataTable("history");
            adpc.Fill(table);
            datagridreserve.ItemsSource = table.DefaultView;
            
            adpc.Update(table);
            connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    
        public void dgnowberuz()
        {
            try
            {

            
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            string baghi = refdate();
            OleDbCommand query = new OleDbCommand("SELECT * FROM history WHERE statuskar = 'darhal' ", connection);

            OleDbDataAdapter adpd = new OleDbDataAdapter(query);
            DataTable table = new DataTable("history");
            adpd.Fill(table);
            datagriddarhal.ItemsSource = table.DefaultView;
               // datagriddarhal.Columns[1].Header = "sa";

            adpd.Update(table);
            connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void formdatagridha()
        {
            dgberuz();
            dgnowberuz();
            dgreservberuz();

        }
        public string modatkar(string intime, string outtime)
        {
            string[] outin = intime.Split(':');
            string[] outout = outtime.Split(':');
            int mins = 0;
            int h1 = Convert.ToInt32(outin[0]);
            int h2 = Convert.ToInt32(outout[0]);
            int m1 = Convert.ToInt32(outin[1]);
            int m2 = Convert.ToInt32(outout[1]);
            int hour = h2 - h1;
            if (hour == 0)
            {
                mins = m2 - m1;
            }
            else
            {
                mins = (60 * hour) - m1 + m2;
            }
            
            return mins.ToString();
        }
        private void btnpayankar_Click(object sender, RoutedEventArgs e)
        {
            string ID;
            
            DataGrid gd = (DataGrid)datagriddarhal;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                ID = row_selected["ID"].ToString();
                //string khadamat = row_selected["khadamat"].ToString();
                //string pelak = row_selected["pelak"].ToString();
                //string tarikh = row_selected["tarikh"].ToString();
                string outtime = DateTime.Now.ToString("HH:mm");
                string intime = row_selected["intime"].ToString();
                string modat = modatkar(intime,outtime);
                try
                {
                    
                    string statusekar = "takmil";
                    //sabt(khadamat, pelak, tarikh, intime, outtime, statusekar);

                    OleDbConnection connection = new OleDbConnection();
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    OleDbCommand Cmd2 = new OleDbCommand();
                    string query = "UPDATE history SET outtime=@outtime , modat=@modat , statuskar=@newstatus WHERE ID=@id";

                    Cmd2.Connection = connection;
                    Cmd2.CommandText = query;
                    Cmd2.Parameters.AddWithValue("@outtime", outtime);//---
                    Cmd2.Parameters.AddWithValue("@modat", modat);
                    Cmd2.Parameters.AddWithValue("@newstatus", statusekar);
                    Cmd2.Parameters.AddWithValue("@ID", ID);

                    Cmd2.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("هزینه خدمت دریافت شد.");
                    formdatagridha();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void btnreadyres_Click(object sender, RoutedEventArgs e)
        {
            string ID;
            

            DataGrid gd = (DataGrid)datagridreserve;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                ID = row_selected["ID"].ToString();

                try
                {
                    string intime = DateTime.Now.ToString("HH:mm");
                    string statusekar = "darhal";
                    string tarikh = refdate();
                    //sabt(khadamat, pelak, tarikh, intime, outtime, statusekar);

                    OleDbConnection connection = new OleDbConnection();
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    OleDbCommand Cmd2 = new OleDbCommand();
                    string query = "UPDATE history SET intime=@intime , statuskar=@newstatus ,tarikh=@tarikh WHERE ID=@id";

                    Cmd2.Connection = connection;
                    Cmd2.CommandText = query;
                    Cmd2.Parameters.AddWithValue("@intime", intime);//---
                    Cmd2.Parameters.AddWithValue("@newstatus", statusekar);
                    Cmd2.Parameters.AddWithValue("@tarikh", tarikh);
                    Cmd2.Parameters.AddWithValue("@ID", ID);

                    Cmd2.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("خودرو مورد نظر آماده دربافت خدمات شد.");
                    formdatagridha();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnhazfres_Click(object sender, RoutedEventArgs e)
        {
            string ID;


            DataGrid gd = (DataGrid)datagridreserve;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                ID = row_selected["ID"].ToString();

                try
                {


                    OleDbConnection connection = new OleDbConnection();
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    OleDbCommand Cmd2 = new OleDbCommand();
                    string query = "DELETE FROM history WHERE ID=@id";

                    Cmd2.Connection = connection;
                    Cmd2.CommandText = query;

                    Cmd2.Parameters.AddWithValue("@ID", ID);

                    Cmd2.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show(" موردانتخاب شده حذف شد.");
                    formdatagridha();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void print()
        {
            PrintDialog printDlg = new PrintDialog();

            FlowDocument doc = CreateFlowDocument();

            doc.Name = "FlowDoc";

            IDocumentPaginatorSource idpSource = doc;

            //printDlg.PrintableAreaWidth
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");

        }
        private FlowDocument CreateFlowDocument()

        {
            string khadamat = mohasebe();
            string pelak = pelakcode.Text + "(" + pelak1.Text + pelak2.Text + pelak3.Text + ")";
            string tarikh = txtindate.Text;
            string intime = txtintime.Text;
            string hazine = txthazinekol.Text;
            string nam = txtinname.Text;
            string kargar = cbinkargar.Text;
            //string outtime = "";
            //string statuskar = "darhal";
            // Create a FlowDocument

            FlowDocument doc = new FlowDocument();

            //// Create a Section

            //Section sec = new Section();

            //// Create first Paragraph

            //Paragraph p1 = new Paragraph();

            //// Create and add a new Bold, Italic and Underline

            //Bold bld = new Bold();

            //bld.Inlines.Add(new Run("First Paragraph"));

            //Italic italicBld = new Italic();

            //italicBld.Inlines.Add(bld);

            //Underline underlineItalicBld = new Underline();

            //underlineItalicBld.Inlines.Add(italicBld);

            //// Add Bold, Italic, Underline to Paragraph

            //p1.Inlines.Add(underlineItalicBld);
            //// Add Paragraph to Section

            //sec.Blocks.Add(p1);



            //// Add Section to FlowDocument

            //doc.Blocks.Add(sec);
           doc.MinPageWidth = 1024.0;
            doc.MinPageHeight = 480.0;
            //Specify maximum page sizes.
            doc.MaxPageWidth = 1024.0;
            doc.MaxPageHeight = 768.0;
            doc.LineHeight =  48;
            doc.PagePadding = new Thickness(40, 40, 45, 40);
            
            doc.FlowDirection = FlowDirection.RightToLeft;

            Table oTable = new Table();
            oTable.BorderBrush = System.Windows.Media.Brushes.Black;
            //oTable.FlowDirection = FlowDirection.RightToLeft;
            ThicknessConverter tc = new ThicknessConverter();
            oTable.BorderThickness = (Thickness)tc.ConvertFromString("0.01in"); ;
            // Append the table to the document
            doc.Blocks.Add(oTable);
            // Create 5 columns and add them to the table's Columns collection.
            int numberOfColumns = 3;
            for (int x = 0; x < numberOfColumns; x++)
            {
                oTable.Columns.Add(new TableColumn());
                
            }
            // Create and add an empty TableRowGroup Rows.
            oTable.RowGroups.Add(new TableRowGroup());
            oTable.Columns[0].Width = new GridLength(150);
            oTable.Columns[1].Width = new GridLength(300);
            
            oTable.Columns[2].Width = new GridLength(150);
            oTable.RowGroups[0].Rows.Add(new TableRow()); // Add the table head row.
            TableRow currentRow = oTable.RowGroups[0].Rows[0];           //Configure the table head row
            currentRow.Background = System.Windows.Media.Brushes.Gray;
            currentRow.Foreground = System.Windows.Media.Brushes.White;
            
           // Add the header row with content,
           ////////////
            Paragraph p = new Paragraph(new Run("گزارش کار"));
            p.FontSize = 25;
            //p.TextAlignment = TextAlignment.Right;
            p.FontWeight = FontWeights.ExtraBold;
            //p.TextAlignment = TextAlignment.Right;
            //p.Foreground = Brushes.Gray;
            //////////////////
            currentRow.Cells.Add(new TableCell(p));
            
            //currentRow.Cells.Add(new TableCell());
            ////////////
           
            Paragraph p2 = new Paragraph(new Run(tarikh));
            p2.TextAlignment = TextAlignment.Center;
            p2.FontSize = 20;
            p2.FontWeight = FontWeights.ExtraBold;
            currentRow.Cells.Add(new TableCell(p2));

            Paragraph p3 = new Paragraph(new Run(intime));
            p3.TextAlignment = TextAlignment.Center;
            p3.FontSize = 20;
            p3.FontWeight = FontWeights.ExtraBold;
            currentRow.Cells.Add(new TableCell(p3));
            

            //Add Libya data row
            oTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = oTable.RowGroups[0].Rows[1];
            //Configure the row layout
            
            currentRow.Background = System.Windows.Media.Brushes.White;
            currentRow.Foreground = System.Windows.Media.Brushes.Black;
            //Add the country name in the first cell
            
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("خدمات"))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });
            
            //Add the country flag in the second cell
            //BitmapImage bmp0 = new BitmapImage();
            //System.Windows.Controls.Image img0 = new System.Windows.Controls.Image();
            //bmp0.BeginInit();
            //bmp0.UriSource = new Uri("LybianFlag.png", UriKind.Relative);
            //bmp0.EndInit();
            //Paragraph oParagraph0 = new Paragraph();
            //oParagraph0.Background = new ImageBrush(bmp0);

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(khadamat))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });

            //Add the calling code

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(hazine))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });

            /////////////////////////////////
            oTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = oTable.RowGroups[0].Rows[2];
            //Configure the row layout
            currentRow.Background = System.Windows.Media.Brushes.White;
            currentRow.Foreground = System.Windows.Media.Brushes.Black;
            //Add the country name in the first cell
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("کارگر"))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });

            //Add the country flag in the second cell
            //BitmapImage bmp0 = new BitmapImage();
            //System.Windows.Controls.Image img0 = new System.Windows.Controls.Image();
            //bmp0.BeginInit();
            //bmp0.UriSource = new Uri("LybianFlag.png", UriKind.Relative);
            //bmp0.EndInit();
            //Paragraph oParagraph0 = new Paragraph();
            //oParagraph0.Background = new ImageBrush(bmp0);

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(kargar))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });

            //Add the calling code

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("دربافت شد"))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });





            oTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = oTable.RowGroups[0].Rows[3];
            //Configure the row layout
            currentRow.Background = System.Windows.Media.Brushes.White;
            currentRow.Foreground = System.Windows.Media.Brushes.Black;
            //Add the country name in the first cell
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nam))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(pelak))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });

            //currentRow.Cells.Add(new TableCell(new Paragraph(new Run("دربافت شد"))) { BorderThickness = new Thickness(1, 0, 0, 1), BorderBrush = Brushes.Black });



            //Paragraph p = new Paragraph(new Run("گزارش کار"));
            //p.FontSize = 36;
            //doc.Blocks.Add(p);

            p = new Paragraph(new Run("کارواش حاج رضا مشیری"));

            p.FontSize = 14;
            p.FontStyle = FontStyles.Italic;
            p.TextAlignment = TextAlignment.Left;
            p.Foreground = Brushes.Gray;
            doc.Blocks.Add(p);



            return doc;

        }


    }
}
