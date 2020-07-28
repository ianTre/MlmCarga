using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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


        public Paciente(DataRow row)
        {
            var columns = row.ItemArray;

            Nombre = columns[0].ToString();
            if (String.IsNullOrEmpty(Nombre))
                throw new Exception("El registro no contiene nombre");

            Apellido = columns[1].ToString();
            if (String.IsNullOrEmpty(Apellido))
                throw new Exception("El registro no contiene Apellido");

            int aux;
            string doc =columns[2].ToString();
            if(String.IsNullOrEmpty(doc))
                throw new Exception("El registro no contiene DNI");

            if(!int.TryParse(doc,out aux))
                throw new Exception("El registro no contiene DNI");
            NumeroDoc = aux;

            Edad = 0;
            int.TryParse(columns[3].ToString(), out aux);
            Edad = aux;


            string fecha = columns[9].ToString();
            DateTime auxFecha;
            DateTime.TryParse(fecha, out auxFecha);
            FechaCarga = auxFecha;


            Telefono = columns[22].ToString();



        }   
    }

}
