
using ClosedXML.Excel;
using ProyectoHipocrates;
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
            var data = ReadExcelFileClose("BASE", @"C:\Codigo\GitHub\Covid-Carga\MlmCarga\MlmCargaCovid\ExcelSISA.xlsx");
            List<Paciente> pacientes = new List<Paciente>();
            foreach (var row in data)
            {
                Console.WriteLine("Paciente " + row.RowNumber().ToString());
                if (row.RowNumber() == 1)
                {
                    continue;
                }
                try
                {
                    

                    Paciente persona = new Paciente(row);
                    if (BuscarEnBasePropia(persona))
                        continue;
                    BuscarEnKlinicos(persona);

                    pacientes.Add(persona);

                    GrabarEnBasePropia(persona);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    cantErrores++;
                    continue;
                }

                Thread.Sleep(10);
                if (row.RowNumber() % 100 == 0)
                    Console.Clear();
            }

            Console.WriteLine(cantErrores);
            Console.ReadLine();







        }

        private static bool BuscarEnBasePropia(Paciente persona)
        {
            try
            {
                bool encontrado = (new Repositorio()).BuscarEnBasePropia(persona);
                return encontrado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void GrabarEnBasePropia(Paciente persona)
        {
            try
            {
                (new Repositorio()).GrabarEnBasePropia(persona);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private static Paciente BuscarEnKlinicos(Paciente persona)
        {

            List<string> establecimientos = new List<string>();
            establecimientos.Add("germani");
            establecimientos.Add("rebasa");
            establecimientos.Add("niños");
            establecimientos.Add("policlinico");
            establecimientos.Add("cemefir");
            establecimientos.Add("giovinazzo");
            bool pacienteEncontrado = false;
            foreach (string item in establecimientos)
            {
                try
                {


                    Repositorio repo = new Repositorio(item);
                    List<Paciente> pacientes = new List<Paciente>();
                    pacientes.Add(persona);
                    var cantidadMatch = repo.BuscarPacienteEnKlinicos(pacientes, persona);
                    if (cantidadMatch > 0)
                        repo.GrabarRegistros(pacientes);

                    if (cantidadMatch > 0)
                        pacienteEncontrado = true;
                }
                catch (Exception ex)
                {

                    persona.Error = ex.Message;
                    continue;
                }
            }

            if (!pacienteEncontrado)
                persona.Error = "No se encontro al paciente en ninguna base";
            return persona;
        }

        private static void GuardarEnBase(Paciente persona)
        {

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
