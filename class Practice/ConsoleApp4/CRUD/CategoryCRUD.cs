using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp4.Model;

namespace ConsoleApp4.CRUD
{
    internal class CategoryCRUD
    {
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ToString());
        public void AddCategory(Category c)
        {
            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["MyConnection"].ToString()))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO category (Name) VALUES (@name)", con);

                cmd.Parameters.AddWithValue("@name", c.Name);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
