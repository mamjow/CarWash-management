using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace CarWash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    

        public MainWindow()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(1065);
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            tarikh.Text = refdate();
            zaman.Text = DateTime.Now.ToString("HH:mm");


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Upanel.Children.Clear();
            carwash Car = new carwash();
            Upanel.Children.Add(Car);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Upanel.Children.Clear();
            Setting settingu = new Setting();
            Upanel.Children.Add(settingu);
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

            switch (maha)
            {
                case "01":
                    maha = "فروردین";
                    break;
                case "02":
                    maha = "اردیبهشت";
                    break;
                case "03":
                    maha = "خرداد";
                    break;
                case "04":
                    maha = "تیر";
                    break;
                case "05":
                    maha = "مرداد";
                    break;
                case "06":
                    maha = "شهریور";
                    break;
                case "07":
                    maha = "مهر";
                    break;
                case "08":
                    maha = "آبان";
                    break;
                case "09":
                    maha = "آذر";
                    break;
                case "10":
                    maha = "دی";
                    break;
                case "11":
                    maha = "بهمن";
                    break;
                case "12":
                    maha = "اسفند";
                    break;
            }
            string emruz = ruza  + " - " + maha + " - " + pc.GetYear(DateTime.Now);
            return emruz;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Upanel.Children.Clear();
            mali bmali = new mali();
            
            Upanel.Children.Add(bmali);
        }
    }

}
