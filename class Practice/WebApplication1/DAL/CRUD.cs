using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class CRUD
    {
        private readonly string _config;
        private readonly string _connStr;

        public CRUD(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
        }

        public void AddPatient(Patient p)
        {
            SqlConnection con = new SqlConnection(_connStr);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into patients values('" + p.Name + "','" + p.Phone + "')", con);
            cmd.ExecuteNonQuery();
        }

        public List<Patient> GetPatients()
        {
            SqlConnection con = new SqlConnection(_connStr);
            SqlDataAdapter da = new SqlDataAdapter("select * from patients", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Patient> patients = new List<Patient>();
            foreach (DataRow dr in dt.Rows)
            {

                Patient p = new Patient()
                {
                    Id = int.Parse(dr["id"].ToString()),
                    Name = dr["name"].ToString(),
                    Phone = dr["phone"].ToString()
                };
                patients.Add(p);
            }
            return patients;
        }
        public Patient GetPatientById(int id)
        {
            SqlConnection con = new SqlConnection(_connStr);
            SqlDataAdapter da = new SqlDataAdapter("select * from patients where id=" + id, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Patient p = new Patient()
            {
                Id = int.Parse(dt.Rows[0]["id"].ToString()),
                Name = dt.Rows[0]["name"].ToString(),
                Phone = dt.Rows[0]["phone"].ToString()
            };
            return p;
        }
        public void UpdatePatient(Patient p)
        {
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update patients set name=@name, phone=@phone where id=@id", con);
                cmd.Parameters.AddWithValue("@name", p.Name);
                cmd.Parameters.AddWithValue("@phone", p.Phone);
                cmd.Parameters.AddWithValue("@id", p.Id);
                cmd.ExecuteNonQuery();
            }
            ;
        }

        public string DeletePatient(int id)
        {
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from patients where id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return "Patient deleted successfully.";
                }
                else
                {
                    return "Patient not found.";
                }
            }
        }
        
    }

}