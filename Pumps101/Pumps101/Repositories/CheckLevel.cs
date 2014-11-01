using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace Pumps101.Repositories
{

    // TODO : Pull correct vals from DB
    public static class CheckLevel
    {
        // Level 1-6
        // out is starts
        // returns string with a message about what they did wrong, empty string if no message should display
        public static string checkLevel(int level_id, double horsePowerUser, out int stars, out bool maxReached)
        {
            double _hpCorrect = 0;
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT hp FROM Level_Answers WHERE level_id = @lvl_id";
                command.Parameters.AddWithValue("@lvl_id", level_id);
                //SqlDataReader reader =  //.ExecuteReader();
                
                    _hpCorrect = (double)command.ExecuteScalar();
                
                command.Connection.Close();
            }
            maxReached = isMaxReached(level_id);
            stars = compare(horsePowerUser, _hpCorrect);
            if (stars > 0)
            {
                levelPassed(level_id, stars);
            }
            return "";
        }

        // Level 7
        public static string checkLevel(int level_id, double horsePowerUser, double npshUser, out int stars, out bool maxReached)
        {
            double _hpCorrect = 0;
            double _npshCorrect = 0;
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT hp,npsh FROM Level_Answers WHERE level_id = @lvl_id";
                command.Parameters.AddWithValue("@lvl_id", level_id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    _hpCorrect = (double)reader["hp"];
                    _npshCorrect = (double)reader["npsh"];
                }
                command.Connection.Close();
            }
            
            maxReached = isMaxReached(level_id);
            stars = 0;
            int hpS = compare(horsePowerUser, _hpCorrect);
            int npshS = compare(npshUser, _npshCorrect);

            if(hpS == 0 && npshS == 0)
            {
                return "";
            }
            else if(hpS == 0)
            {
                return "Horsepower is Incorrect";
            }
            else if (npshS == 0)
            {
                return "NPSH is Incorrect";
            }
            else
            {
                stars = (hpS + npshS) / 2;
                if (stars > 0)
                {
                    levelPassed(level_id, stars);
                }
            }

            return "";
        }


        // Level 8
        public static string checkLevel(int level_id, double horsePowerUser, double npshUser, string pumpUser, out int stars, out bool maxReached)
        {
            double _hpCorrect = 0;
            double _npshCorrect = 0;
            string _pumpCorrect = "";

            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT hp,npsh FROM Level_Answers WHERE level_id = @lvl_id";
                command.Parameters.AddWithValue("@lvl_id", level_id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    _hpCorrect = (double)reader["hp"];
                    _npshCorrect = (double)reader["npsh"];
                    _pumpCorrect = reader["pump_type"].ToString();
                }
                command.Connection.Close();
            }

            maxReached = isMaxReached(level_id);
            stars = 0;
            int hpS = compare(horsePowerUser, _hpCorrect);
            int npshS = compare(npshUser, _npshCorrect);
            
            if(pumpUser.Equals(_pumpCorrect))
            {
                return "Incorrect Pump Type";
            }
            else if (hpS == 0 && npshS == 0)
            {
                return "";
            }
            else if (hpS == 0)
            {
                return "Horsepower is Incorrect";
            }
            else if (npshS == 0)
            {
                return "NPSH is Incorrect";
            }
            else
            {
                stars = (hpS + npshS) / 2;
                if(stars > 0)
                {
                    levelPassed(level_id, stars);
                }
            }


            return "";
        }

        // Level 10
        public static string checkLevel(int level_id, double horsePowerUser, double npshUser, string pumpUser, double costUser, out int stars, out bool maxReached)
        {
            double _hpCorrect = 0;
            double _npshCorrect = 0;
            string _pumpCorrect = "";
            double _costCorrect = 0;
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT hp,npsh FROM Level_Answers WHERE level_id = @lvl_id";
                command.Parameters.AddWithValue("@lvl_id", level_id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    _hpCorrect = (double)reader["hp"];
                    _npshCorrect = (double)reader["npsh"];
                    _pumpCorrect = reader["pump_type"].ToString();
                    _costCorrect = (double)reader["cost"];
                }
                command.Connection.Close();
            }


            maxReached = isMaxReached(level_id);
            stars = 0;
            int hpS = compare(horsePowerUser, _hpCorrect);
            int npshS = compare(npshUser, _npshCorrect);
            int costC = compare(costUser, _costCorrect);

            

            if (pumpUser.Equals(_pumpCorrect))
            {
                return "Incorrect Pump Type";
            }
            else if (hpS == 0 && npshS == 0 && costC == 0)
            {
                return "";
            }
            if(hpS == 0 || npshS == 0 || costC == 0)
            {
                StringBuilder s = new StringBuilder();
                if(hpS == 0)
                {
                    s.Append("Horsepower");
                }
                if (npshS == 0)
                {
                    if(s.Length > 2)
                    {
                        s.Append(", NPSH");
                    }
                    else { s.Append("NPSH"); }
                }
                if(costC == 0)
                {
                    if (s.Length > 2)
                    {
                        s.Append(", and Cost");
                    }
                    else { s.Append("Cost"); }
                }
                if(s.Length > 10)
                {
                    s.Append(" are Incorrect");
                }
                else{s.Append(" is Inccorect");}
                
                return s.ToString();
            }
            else
            {
                stars = (hpS + npshS + costC) / 3;
                if (stars > 0)
                {
                    levelPassed(level_id, stars);
                }
            }
            return "";
        }


        private static int compare(double val, double correct)
        {
            if (val <= correct + (0.01 * correct) && val >= correct - (0.01 * correct))
            {
                return 3;
            }
            else if (val <= correct + (0.05 * correct) && val >= correct - (0.05 * correct))
            {
                return 2;
            }
            else if (val <= correct + (0.1 * correct) && val >= correct - (0.1 * correct))
            {
                return 1;
            }
            return 0;
        }

        private static bool isMaxReached(int level_id)
        {
            int attempts = 0;
            int chances = 0;
            // increments attemps if attemps reached then filled the active status of level and return true 
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT chances,attempts FROM Levels WHERE level_id = @lvl_id";
                command.Parameters.AddWithValue("@lvl_id", level_id);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                chances = (int)reader["chances"];
                attempts = (int)reader["attempts"];   
                
                command.Connection.Close();
            }

            attempts++;
            if (attempts == chances)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels SET attempts = @new, is_active = 0 WHERE level_id = @lvl_id";
                    command.Parameters.AddWithValue("@new", attempts);
                    command.Parameters.AddWithValue("@lvl_id", level_id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels SET attempts = @new Where level_id = @lvl_id";
                    command.Parameters.AddWithValue("@new", attempts);
                    command.Parameters.AddWithValue("@lvl_id", level_id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return false;
        }

        /// <summary>
        ///  Called there is 1 or more stars
        ///  Unlocks next level
        ///  Deactivates current board
        ///  Adds number of stars to Levels_Complete if it is higher than the current
        /// </summary>
        private static void levelPassed(int level_id, int stars)
        {
            int level = 0;
            Guid user;
            int ID = 0;
            int stars_prev = 0;

            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT * FROM Levels WHERE level_id = @lvl_id";
                command.Parameters.AddWithValue("@lvl_id", level_id);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                user = (Guid)reader["user"];    
                level = (int)reader["level"];
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

            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                connection.Open();
                command.CommandText = "SELECT * FROM Levels_Completed WHERE level = @level";
                command.Parameters.AddWithValue("@level", level);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["user"].ToString().CompareTo(user.ToString().ToUpper()) == 0)
                        {
                            ID = (int)reader["ID"];
                            stars_prev = (int)reader["stars"];
                            break;
                        }
                    }
                }
                command.Connection.Close();
            }
            if (stars > stars_prev)
            {
                //updates current level
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels_Completed SET completed = 1, stars = @stars Where ID = @id";
                    command.Parameters.AddWithValue("@id", ID);
                    command.Parameters.AddWithValue("@stars", stars);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            if (level != 9)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM Levels_Completed WHERE level = @level";
                    command.Parameters.AddWithValue("@level", level + 1);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["user"].ToString().CompareTo(user.ToString().ToUpper()) == 0)
                            {
                                ID = (int)reader["ID"];
                                break;
                            }
                        }
                    }
                    command.Connection.Close();
                }
                // unlocks next level
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels SET locked = 0 Where ID = @id";
                    command.Parameters.AddWithValue("@id", ID);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }



        }
    }
}