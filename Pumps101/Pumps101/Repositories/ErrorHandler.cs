using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Pumps101.Repositories
{
    public static class ErrorHandler
    {
        //Deactivates the current level
        public static void LevelError(Guid user, int level)
        {
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            int level_id = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM Levels WHERE level = @level";
                    command.Parameters.AddWithValue("@level", level + 1);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["user"].ToString().CompareTo(user.ToString().ToUpper()) == 0)
                            {
                                level_id = (int)reader["level_id"];
                                break;
                            }
                        }
                    }
                    command.Connection.Close();
                }
                //deactivates level
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels SET is_active = 0 Where level_id = @lvl_id";
                    command.Parameters.AddWithValue("@lvl_id", level_id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch { /* Error in error handler... I DON'T EVEN KNOW!!! I quit. */}
        }

        //Deactivates the current level
        public static void LevelError(int level_id)
        {
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                //deactivates level
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels SET is_active = 0 Where level_id = @lvl_id";
                    command.Parameters.AddWithValue("@lvl_id", level_id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch { /* Error in error handler... I DON'T EVEN KNOW!!! I quit. */}
        }

        



    }
}