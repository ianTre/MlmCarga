using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace ProyectoHipocrates
{
    public class DBAcces
    {
        private SqlConnection con;

        public SqlConnection GetConnection()
        {
            string sqlcon = "Data Source=localhost;" +
                            "Initial Catalog=KLINICOS;" +
                            "persist security info = True;" +
                            "Integrated Security=SSPI;";
            this.con = new SqlConnection(sqlcon);
            return this.con;
        }

        public SqlConnection GetConnectionKlnicos(string esta)
        {
            string sqlcon;
            switch (esta)
            {
                
                case "rebasa":
                    sqlcon = "data source = 172.46.1.10;initial catalog=KLINICOS; persist security info =True; user id =sa; password =sql2018*; MultipleActiveResultSets =True;";
                    break;

                case "germani":
                    sqlcon = "data source=172.28.1.220;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "sakamoto":
                    sqlcon = "data source=172.45.1.10;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "cemefir":
                    sqlcon = "data source=192.168.71.10;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "mental":
                    sqlcon = "data source=192.168.70.10;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "niños":
                    sqlcon = "data source=172.29.1.100;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "eizaguirre":
                    sqlcon = "data source=192.168.10.40;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "giovinazzo":
                    sqlcon = "data source=172.26.1.10;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;

                case "policlinico":
                    sqlcon = "data source=192.168.34.10;initial catalog=KLINICOS;persist security info=True;user id=sa;password=sql2018*;MultipleActiveResultSets=True;";
                    break;



                default:
                    sqlcon = "data source = 172.46.1.10;initial catalog=KLINICOS; persist security info =True; user id =sa; password =sql2018*; MultipleActiveResultSets =True;";
                    break;
            }

            
            this.con = new SqlConnection(sqlcon);
            return this.con;
        }


    }
}