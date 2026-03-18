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
    internal class ProductCRUD
    {
        SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["MyConnection"].ToString());
        public string AddProduct(Product product)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into products values (@Name,@CategoryId)", con);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        public Product UpdateProduct(Product product)
        {
            SqlCommand cmd = new SqlCommand("update products set Name=@Name,CategoryId=@CategoryId where Productid=@ProductId", con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.ExecuteNonQuery();
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public string DeleteProduct(Product product)
        {
            SqlCommand cmd = new SqlCommand("delete from products where Productid=@ProductId", con);
            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.ExecuteNonQuery();
                return "Deleted Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
        public List<Product> GetProducts()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from products", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<Product> products = new List<Product>();
                foreach (DataRow dr in dt.Rows)
                {
                    Product product = new Product()
                    {
                        ProductId = int.Parse(dr["ProductId"].ToString()),
                        Name = dr["Name"].ToString(),
                        CategoryId = int.Parse(dr["CategoryId"].ToString())
                    };
                    products.Add(product);

                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();

            }
        }
    }
}
