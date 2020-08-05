using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading;

namespace MlmCargaCovid
{
    class Program
    {
        static void Main(string[] args)
        {

            int cantErrores = 0;
            Console.WriteLine("Hola mundo");
            //var data = ReadExcelFile("BASE", @"D:\Codigo\MlmCargaCovid\MlmCargaCovid\ExcelSISA.xlsx");
            var data = ReadExcelFileClose("BASE", @"D:\Codigo\MlmCargaCovid\MlmCarga\MlmCargaCovid\ExcelSISA.xlsx");
            List<Paciente> pacientes = new List<Paciente>();
            foreach (var row in data)
            {
                Console.WriteLine("Paciente " + row.RowNumber().ToString());
                if ( row.RowNumber() == 1 )
                {
                    continue;
                }
                try
                {
                    Paciente persona = new Paciente(row);
                    pacientes.Add(persona);
                    persona.BuscarEnKlinicos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    cantErrores++;
                    continue;
                }
                
                Thread.Sleep(10);
                if(row.RowNumber() % 100 == 0)
                    Console.Clear();
            }

            Console.WriteLine(cantErrores);
            Console.ReadLine();
            






        }

        static private IXLRows ReadExcelFileClose(string sheetName, string path)
        {


            string fileName = path;
            var excelWorkbook = new XLWorkbook(fileName);

            var nonEmptyDataRows = excelWorkbook.Worksheet(sheetName).RowsUsed();
            return nonEmptyDataRows;

        }


        static private DataTable ReadExcelFile(string sheetName, string path)
        {

            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        return dt;
                    }

                }
            }
        }
    }
}
