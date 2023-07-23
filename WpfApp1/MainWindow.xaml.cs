using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Xml.Linq;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connStr = null;
        private DataSet dataSet = null;
        private SqlDataAdapter adapter = null;

        public MainWindow()
        {
            InitializeComponent();
            connStr = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;

        }
        private void LoadInfo(string Name)
        {
            string cmd = $"select * from {Name};";

            adapter = new(cmd, connStr);

            new SqlCommandBuilder(adapter);

            dataSet = new();
            adapter.Fill(dataSet);

            dgrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }



        private void LoadAdmin()
        {
            string cmd = $"select * from Users as u Where u.StaffId = 1;";

            adapter = new(cmd, connStr);

            new SqlCommandBuilder(adapter);

            dataSet = new();
            adapter.Fill(dataSet);

            dgrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        public void DropUsersWithRole(int role)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                string cmd = "exec sp_delete_UserByRole @role";

                SqlCommand command = new SqlCommand(cmd, connection);

                command.Parameters.AddWithValue("@role", role);

                command.ExecuteNonQuery();
            }
        }

        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadInfo("Users");
        }

        private void ShowStaff_Click(object sender, RoutedEventArgs e)
        {
            LoadInfo("Staffs");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            adapter.Update(dataSet);
        }

        private void AdminShow_Click(object sender, RoutedEventArgs e)
        {
            LoadAdmin();
        }

        private void DropByStaff_Click(object sender, RoutedEventArgs e)
        {
            DropUsersWithRole(2);
        }
    }
}