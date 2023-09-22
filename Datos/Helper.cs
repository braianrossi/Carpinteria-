using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria_2._0.Datos
{
    internal class Helper
    {
       public void Conectar()
       {
            SqlConnection conexion = new SqlConnection();
            conexion.ConnectionString = @"Data Source=localhost;Initial Catalog=Carpinteria_2023;Integrated Security=True";
            conexion.Open();

       }       

    }
}
