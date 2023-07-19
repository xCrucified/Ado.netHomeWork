using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Util
    {
        private SqlConnection connection = null;
        public Util(string? connectionStr = null)
        {
            connectionStr ??= ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;

            connection = new SqlConnection(connectionStr);
            connection.Open();
        }
        private void ShowTable(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i),-10}\t");
            }
            Console.WriteLine("\n-------------------------------------------------------------------------------------");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i],-10}\t");
                }
                Console.WriteLine();
            }
        }
        public void GetInfo()
        {
            string cmdText = @"select * from Sellers";

            SqlCommand command = new SqlCommand(cmdText, connection);

            using var reader = command.ExecuteReader();
            ShowTable(reader);
        }
        //1
        public void AddNewSale()
        {
            //INSERT INTO Sales (BuyerId, SellerId, Amount, DateOfSale) VALUES (19, 19, 300.75, '2023-06-30');
            int BuyerId = int.Parse(Console.ReadLine());
            int SellerId = int.Parse(Console.ReadLine());
            decimal Amount = decimal.Parse(Console.ReadLine());
            DateTime DateOfSale = DateTime.Parse(Console.ReadLine());

            string cmd = $"insert into Sales (BuyerId, SellerId, Amount, DateOfSale) values (@BuyerId, @SellerId, @Amount, @DateOfSale)";

            SqlCommand command = new(cmd, connection);
            command.Parameters.AddWithValue("@BuyerId", BuyerId);
            command.Parameters.AddWithValue("@SellerId", SellerId);
            command.Parameters.AddWithValue("@Amount", Amount);
            command.Parameters.AddWithValue("@DateOfSale", DateOfSale);

            command.ExecuteNonQuery();
        }
        //2 Відобразитцію про всі продажі за певний періоди інформа
        public void ShowSales()
        {
            DateTime DateOfSale = DateTime.Parse(Console.ReadLine());
            string cmdText = @"select * from Sales as s where s.DateOfSale = @DateOfSale";
            SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@DateOfSale", DateOfSale);

            using var reader = command.ExecuteReader();
            ShowTable(reader);
        }
        //3 Показати останню покупку певного покупця по імені та прізвищу
        public void FindByName()
        {
            string Name = Console.ReadLine();
            string cmdText = @"select * from Sales as s join Buyers as b on b.Id = s.BuyerId where b.Id = s.BuyerId and b.Name = @Name";
            SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@Name", Name);
            using var reader = command.ExecuteReader();
            ShowTable(reader);
        }
        //4 Видалити продавця або покупця по id
        public void DropById()
        {
            int Id = int.Parse(Console.ReadLine());
            string cmd = $"Delete From Sales Where Sales.BuyerId = @Id";
            SqlCommand command = new(cmd, connection);
            command.Parameters.AddWithValue("Id", Id);

            command.ExecuteNonQuery();
        }
        //5  Показати продавця, загальна сума продаж якого є найбільшою
        public void ShowLargeAmount()
        {
            string cmdText = @"Select top 1 * From Sales as s Order by s.Amount desc";
            SqlCommand command = new SqlCommand(cmdText, connection);

            using var reader = command.ExecuteReader();
            ShowTable(reader);
        }
        public IEnumerable<Info> ShowAllInfo()
        {
            string cmdText = @"select * from Sales";
            SqlCommand command = new SqlCommand(cmdText, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new Info()
                {
                    Id = (int)reader["Id"],
                    BuyerId = (int)reader["BuyerId"],
                    SellerId = (int)reader["SellerId"],
                    Amount = (decimal)reader["Amount"],
                    DateOfSale = (DateTime)reader["Date Of Sale"]
                };
            }

        }
    }
}


