using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp5.Models;

namespace ConsoleApp5.CRUD
{
    internal class CategoryCRUD
    {
        SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["MyConnection"].ToString());
        public void AddCategory(Category c)
        {
            
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO category (Name) VALUES (@name)", con);

                    cmd.Parameters.AddWithValue("@name", c.Name);

                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }
                
            }
        }
        public string UpgradeCategory(Category c)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update category set [Name] ='" + c.Name + "' where Id = " + c.Id ,con);
                cmd.ExecuteNonQuery();
                return "Succefully Updated";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
            

        }

        public String DeleteCategory(Category c)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from category where Id = '" + c.Id +"'", con);
                cmd.ExecuteNonQuery();
                return "Succefully Deleted";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
        public List<Category> CategoriesList()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from category", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Category> list = new List<Category>();
            foreach (DataRow drow in dt.Rows)
            {
                Category c = new Category();
                c.Id = int.Parse(drow["Id"].ToString());
                c.Name = drow["Name"].ToString();
                list.Add(c);
            }
            return list;
        }
    }
}
