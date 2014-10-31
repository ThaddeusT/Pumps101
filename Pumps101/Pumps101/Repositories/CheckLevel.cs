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
            maxReached = isMaxReached(level_id);
            stars = compare(horsePowerUser, _hpCorrect);
            return "";
        }

        // Level 7
        public static string checkLevel(int level_id, double horsePowerUser, double npshUser, out int stars, out bool maxReached)
        {
            double _hpCorrect = 0;
            double _npshCorrect = 0;
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
            }

            return "";
        }


        // Level 8
        public static string checkLevel(int level_id, double horsePowerUser, double npshUser, string pumpUser, out int stars, out bool maxReached)
        {
            double _hpCorrect = 0;
            double _npshCorrect = 0;
            string _pumpCorrect = "";
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
            }

            return "";
        }

        // Level 10
        public static string checkLevel(int level_id, double horsePowerUser, double _hpCorrect, double npshUser, double _npshCorrect, string pumpUser, string _pumpCorrect, double costUser, double _costCorrect, out int stars, out bool maxReached)
        {
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
                if (reader.HasRows)
                {
                    chances = (int)reader["chance"];
                    attempts = (int)reader["attempts"];   
                }
                command.Connection.Close();
            }

            attempts++;
            if (attempts == chances)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "UPDATE Levels SET attempts = @new, is_active = false Where level_id = @lvl_id";
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

    }
}