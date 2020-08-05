using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ClosedXML.Excel;


namespace MlmCargaCovid
{
    class Paciente
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int NumeroDoc { get; set; }
        public int Edad { get; set; }
        public DateTime FechaCarga { get; set; }
        public string Telefono { get; set; }


        public Paciente(IXLRow row)
        {

            try
            {

            
            Nombre = row.Cell(1).Value.ToString();
            if (String.IsNullOrEmpty(Nombre))
                throw new Exception("El registro no contiene nombre");

            Apellido = row.Cell(2).Value.ToString();
            if (String.IsNullOrEmpty(Apellido))
                throw new Exception("El registro no contiene Apellido");

            int aux;
            string doc = row.Cell(3).Value.ToString();
            if(String.IsNullOrEmpty(doc))
                throw new Exception("El registro no contiene DNI");

            if(!int.TryParse(doc,out aux))
                throw new Exception("El registro no contiene DNI");
            NumeroDoc = aux;
            
            Edad = 0;
            int.TryParse(row.Cell(4).Value.ToString(), out aux);
            Edad = aux;


            string fecha = row.Cell(10).Value.ToString();
            DateTime auxFecha;
            DateTime.TryParse(fecha, out auxFecha);
            FechaCarga = auxFecha;


            Telefono = row.Cell(23).Value.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        internal void BuscarEnKlinicos()
        {
            
        }
    }

}
