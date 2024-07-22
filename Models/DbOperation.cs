using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace REGISTRATION.Models
{
    public class DbOperation
    {

        public SqlConnection Con = new SqlConnection(
            ConfigurationManager.
            ConnectionStrings["RegistrationForm"].
            ConnectionString

            );


        public List<User> GetUser()
        {
            List<User> list = new List<User>();

            using (SqlCommand cmd = new SqlCommand("select * from registered_user", Con))
            {
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new User()
                    {
                        registered_user_id = Convert.ToInt32(dr[0]),
                        registered_user_name = Convert.ToString(dr[1]),
                        registered_user_mail = Convert.ToString(dr[2]),
                        registered_user_password = Convert.ToString(dr[3])

                    });

                }

            }

            return list;
        }

        public bool RegisteringUser(string name,string email,string password,string about)
        {
            string query = "INSERT INTO registered_user (registered_user_name, registered_user_mail, registered_user_password, registered_user_about) VALUES (@Name, @Email, @Password, @About)";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@About", about);


                try
                {
                    Con.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        public User GetUserById(string userId)
        {
            User user = null;
            string query = "SELECT * FROM registered_user WHERE registered_user_id = @UserId";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    user = new User()
                    {
                        registered_user_id = Convert.ToInt32(dr[0]),
                        registered_user_name = Convert.ToString(dr[1]),
                        registered_user_mail = Convert.ToString(dr[2]),
                        registered_user_password = Convert.ToString(dr[3])
                    };
                }
            }

            return user;
        }


    }


}
