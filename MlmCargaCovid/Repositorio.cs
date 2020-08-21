

using MlmCargaCovid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;


namespace ProyectoHipocrates
{
    public class Repositorio
    {
        DBAcces dBAccess { get; set; }
        public string establecimiento { get; set; }


        public Repositorio()
        {
            this.dBAccess = new DBAcces();
        }

        public Repositorio(string establecimiento)
        {
            this.dBAccess = new DBAcces();
            this.establecimiento = establecimiento;
        }

        public List<Paciente> ObtenerPaciente()
        {
            try
            {
                SqlConnection conn = dBAccess.GetConnection();
                SqlCommand com = new SqlCommand("SP_OBTENER_PROFESIONALES", conn);
                List<Paciente> test = new List<Paciente>();
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    Paciente paciente = new Paciente();
                    paciente.Apellido = Convert.ToString(dr["Apellido"]);
                    paciente.FechaCarga = Convert.ToDateTime(dr["FechaCarga"]);
                    paciente.Nombre = Convert.ToString(dr["Nombre"]);
                    paciente.NumeroDoc = Convert.ToInt32(dr["DNI"]);
                    test.Add(paciente);
                }
                
                return test;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        internal bool BuscarEnBasePropia(Paciente persona)
        {
            try
            {
                SqlConnection conn = dBAccess.GetConnection();
                SqlCommand com = new SqlCommand("SP_INTERNO_BUSQUEDA", conn);
                com.Parameters.Add(new SqlParameter("DNI", persona.NumeroDoc));
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();
                if(dt.Rows.Count > 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        internal void GrabarEnBasePropia(Paciente persona)
        {
            try
            {

                SqlConnection conn = dBAccess.GetConnection();
                SqlCommand com = new SqlCommand("dbo.SP_INTERNO", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.Parameters.Clear();


                com.Parameters.Add(new SqlParameter("DNI", persona.NumeroDoc));
                if(persona.Error == "No se encontro al paciente en ninguna base")
                    com.Parameters.Add(new SqlParameter("HECHO", false));
                else
                    com.Parameters.Add(new SqlParameter("HECHO", true));


                try
                {
                    conn.Open();
                    com.ExecuteScalar();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void InsertarProfesional(Paciente profesional)
        {
            try
            {

                SqlConnection conn = dBAccess.GetConnectionKlnicos(this.establecimiento);
                SqlCommand com = new SqlCommand("dbo.SP_Insertar_Profesional", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.Parameters.Clear();
                
                
                com.Parameters.Add(new SqlParameter("DNI", profesional.NumeroDoc));
                com.Parameters.Add(new SqlParameter("Apellido", profesional.Apellido));
                com.Parameters.Add(new SqlParameter("Nombre", profesional.Nombre));
                com.Parameters.Add(new SqlParameter("FechaCarga", profesional.FechaCarga));

                try
                {
                    conn.Open();
                    com.ExecuteScalar();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal void GrabarRegistros(List<Paciente> pacientes)
        {
            foreach (Paciente paciente in pacientes)
            {
                if(!String.IsNullOrEmpty(paciente.IDKlinicos))
                    EscribirHC(paciente);
            }
        }

        private void EscribirHC(Paciente paciente)
        {
            try
            {

                SqlConnection conn = dBAccess.GetConnectionKlnicos(this.establecimiento);
                SqlCommand com = new SqlCommand("dbo.ESCRIBIRHC", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.Parameters.Clear();


                com.Parameters.Add(new SqlParameter("idpaciente", paciente.IDKlinicos));
                com.Parameters.Add(new SqlParameter("idprofesional", 32));
                com.Parameters.Add(new SqlParameter("fechainicio", paciente.FechaCarga));
                

                try
                {
                    conn.Open();
                    com.ExecuteScalar();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int BuscarPacienteEnKlinicos(List<Paciente> listadoPacientes , Paciente paciente)
        {
            try
            {
                SqlConnection conn = dBAccess.GetConnectionKlnicos(this.establecimiento);
                SqlCommand com = new SqlCommand("SP_OBTENER_PACIENTE_KLINICOS", conn);
                com.Parameters.Add(new SqlParameter("numeroDocumento", paciente.NumeroDoc));
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                int cantMatch = 0;

                if (dt.Rows.Count == 1)
                {
                    cantMatch = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        paciente.IDKlinicos = Convert.ToString(dr["id"]);
                        
                    }
                }
                else
                {
                    if(dt.Rows.Count == 0)
                    {
                        
                    }

                    if (dt.Rows.Count > 1)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            bool match = false;
                            string apellidoBase = Convert.ToString(dr["primerApellido"]);
                            string nombreBase = Convert.ToString(dr["primerNombre"]);
                            string idBase = Convert.ToString(dr["id"]);

                            
                            if(apellidoBase.ToUpper().Trim().Contains(paciente.Apellido.ToUpper().Trim() ) || paciente.Apellido.ToUpper().Trim().Contains(apellidoBase.ToUpper().Trim()) )
                            {
                                match = true;
                            }

                            if (nombreBase.ToUpper().Trim().Contains(paciente.Nombre.ToUpper().Trim()) || paciente.Nombre.ToUpper().Trim().Contains(nombreBase.ToUpper().Trim()))
                            {
                                match = true;
                            }


                            if (!match)
                            {
                                continue;
                            }

                            
                            if (cantMatch == 0)
                            {
                                paciente.IDKlinicos = idBase;
                            }
                            else
                            {
                                Paciente nuevoPaciente = new Paciente();
                                nuevoPaciente.Apellido = paciente.Apellido;
                                nuevoPaciente.Edad = paciente.Edad;
                                nuevoPaciente.Error = paciente.Error;
                                nuevoPaciente.FechaCarga = paciente.FechaCarga;
                                nuevoPaciente.Nombre = paciente.Nombre;
                                nuevoPaciente.NumeroDoc= paciente.NumeroDoc;
                                nuevoPaciente.Procesado = paciente.Procesado;
                                nuevoPaciente.Telefono = paciente.Telefono;

                                nuevoPaciente.IDKlinicos = idBase;

                                listadoPacientes.Add(nuevoPaciente);
                            }
                            cantMatch++;
                        }
                    }
                }

                return cantMatch;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }

}