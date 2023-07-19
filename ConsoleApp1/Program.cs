using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Util util = new Util();
            //util.AddNewSale();
            //util.ShowSales();
            //util.FindByName();
            //util.DropById();
            util.ShowLargeAmount();
        }
    }
}