//using System;
//using System.Data;
//using System.Data.SqlClient;
//using Microsoft.VisualBasic.FileIO;

//namespace ReadDataFromCSVFile
//{
//    static class Program
//    {
//        static void Main()
//        {
//            string csv_file_path = @"C:\Users\19024868\OneDrive - Republic Polytechnic\Documents\C300FYP_Year3\Student.csv";
//            DataTable csvData = GetDataTabletFromCSVFile(csv_file_path);
//            Console.WriteLine("Rows count:" + csvData.Rows.Count);
//            Console.ReadLine();
//        }
      
//        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
//        {
//            DataTable csvData = new DataTable();
//            try
//            {
//                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
//                {
//                    csvReader.SetDelimiters(new string[] { "," });
//                    csvReader.HasFieldsEnclosedInQuotes = true;
//                    string[] colFields = csvReader.ReadFields();
//                    foreach (string column in colFields)
//                    {
//                        DataColumn datecolumn = new DataColumn(column);
//                        datecolumn.AllowDBNull = true;
//                        csvData.Columns.Add(datecolumn);
//                    }
//                    while (!csvReader.EndOfData)
//                    {
//                        string[] fieldData = csvReader.ReadFields();
//                        //Making empty value as null
//                        for (int i = 0; i < fieldData.Length; i++)
//                        {
//                            if (fieldData[i] == "")
//                            {
//                                fieldData[i] = null;
//                            }
//                        }
//                        csvData.Rows.Add(fieldData);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//            }
//            return csvData;
           

//        }
//          private static void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData)
//        {
//            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=FYP_Database;Integrated Security=True"))
//            {
//                dbConnection.Open();
//                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
//                {
//                    s.DestinationTableName = "Student";

//                    foreach (var column in csvFileData.Columns)
//                        s.ColumnMappings.Add(column.ToString(), column.ToString());

//                    s.WriteToServer(csvFileData);
//                }
//            }
//        }

//    }

//}
