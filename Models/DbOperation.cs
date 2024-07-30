using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            using (SqlCommand cmd = new SqlCommand("exec GETALLUSER", Con))
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
                        registered_user_password = Convert.ToString(dr[3]),
                        registered_user_about = Convert.ToString(dr[4]),
                        registered_user_imagepath = Convert.ToString(dr[5])

                    });

                }

            }

            return list;
        }

        public bool RegisteringUser(string name, string email, string password, string about, string imagepath)
        {
            string query = "INSERT INTO registered_user (registered_user_name, registered_user_mail, registered_user_password, registered_user_about, registered_user_imagepath) " +
                           "VALUES (@Name, @Email, @Password, @About, @Image)";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@About", about);
                cmd.Parameters.AddWithValue("@Image", imagepath);

                try
                {
                    Con.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    // Log the exception message for debugging purposes
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }


        public User GetUserById(int userId)
        {
            User user = null;
            string query = "exec USERBYID @Userid=@UserId";

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
                        registered_user_password = Convert.ToString(dr[3]),
                        registered_user_about = Convert.ToString(dr[4]),
                        registered_user_imagepath = Convert.ToString(dr[5])

                    };

                   
                }
            }

            return user;

        }

        /*--Edit--*/
        public bool EditOperation(User editDetails) 
        {
            string query = "EXEC EDITUSER @EditUserId=@EditId," +
                "@EditUserName=@EditName," +
                "@EditUserAbout=@EditAbout," +
                "@EditUserPassword=@EditPassword," +
                "@EditUserMail=@EditMail,"+
                "@EditUserImage=@EditImage";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@EditName", editDetails.registered_user_name);
                cmd.Parameters.AddWithValue("@EditMail", editDetails.registered_user_mail);
                cmd.Parameters.AddWithValue("@EditPassword", editDetails.registered_user_password);
                cmd.Parameters.AddWithValue("@EditAbout",editDetails.registered_user_about);
                cmd.Parameters.AddWithValue("@EditId", editDetails.registered_user_id);

                cmd.Parameters.AddWithValue("@EditImage",editDetails.registered_user_imagepath);

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

        /*Delete*/
        public bool DeleteOperation(int deleteId)
        {
            string query = "EXEC DELETEUSER @DeleteUserId=@DelId";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {

                cmd.Parameters.AddWithValue("@DelId", deleteId);

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


    }
}
