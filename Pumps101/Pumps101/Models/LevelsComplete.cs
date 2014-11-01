using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Pumps101.Models
{
    public class LevelsComplete
    {
        #region Fields 

        List<Level> _levels = new List<Level>();

        #endregion


        #region Constructors

        public LevelsComplete(Guid User, Boolean Authenticated)
        {
            if (Authenticated)
            {
                bool[] set = new bool[9];
                var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM Levels_Complete";
                    //string ld = command.ExecuteScalar().ToString();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        int count = 0;
                        while (reader.Read() && count < 9)
                        {
                            //string u = ;
                            if (reader["user"].ToString().CompareTo(User.ToString().ToUpper()) == 0)
                            {
                                _levels.Add(new Level((int)reader["level"], (int)reader["stars"], (bool)reader["completed"], (bool)reader["locked"]));
                                set[(int)reader["level"] - 1] = true;
                                count++;
                            }
                        }
                    }
                    command.Connection.Close();
                }

                for (int i = 0; i < set.Length; i++)
                {
                    if (!set[i])
                    {
                        if(i == 0){_levels.Add(new Level(i+1, 0, false, false));}
                        else {_levels.Add(new Level(i+1, 0, false, true));}
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand("", connection))
                            {
                                command.CommandText = "INSERT INTO Levels_Complete VALUES (@user, @level, @completed, @stars, @locked)";
                                command.Parameters.AddWithValue("@user", User);
                                command.Parameters.AddWithValue("@level", i+1);
                                command.Parameters.AddWithValue("@completed", false);
                                command.Parameters.AddWithValue("@stars", 0);
                                if(i == 0){
                                    command.Parameters.AddWithValue("@locked", 0);
                                }
                                else{
                                    command.Parameters.AddWithValue("@locked", 1);
                                }
                                command.ExecuteNonQuery();
                            }
                            connection.Close();
                        }
                    }
                }
            }
        }

        #endregion

        #region Properties

        public List<Level> Levels
        {
            get { return _levels; }
        }

        #endregion

    }
}
