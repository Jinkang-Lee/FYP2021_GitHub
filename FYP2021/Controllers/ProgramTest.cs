using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace FYP2021.Controllers
{
    class ProgramTest
    {
         static void Program(string[] args)
        {
            var lineNumber = 0;
            using (SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\\ProjectsV13;Initial Catalog=FYP_Database;Integrated Security=True"))
            {
                conn.Open();
                using (StreamReader reader = new StreamReader(@"D:\FYP\Student.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(lineNumber != 0)
                        {
                            var values = line.Split(',');

                            var sql = "INSERT INTO Student VALUES ('" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "','" + values[5] + "','" + values[6] + "')";

                            var cmd = new SqlCommand();
                            cmd.CommandText = sql;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                        lineNumber++;
                        
                    }
                }
                conn.Close();
            }
            Console.WriteLine("Products Import Complete");
            Console.ReadLine();
        }
    }
}
