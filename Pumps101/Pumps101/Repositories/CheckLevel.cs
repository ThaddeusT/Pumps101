using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pumps101.Repositories
{
    public static class CheckLevel
    {
        // Level 1-6
        // out is starts
        // returns string with a message about what they did wrong, empty string if no message should display
        public static string checkLevel(double horsePowerUser, double _hpCorrect, out int stars)
        {
            stars = compare(horsePowerUser, _hpCorrect);
            return "";
        }

        // Level 7
        public static string checkLevel(double horsePowerUser, double _hpCorrect, double npshUser, double _npshCorrect, out int stars)
        {
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
        public static string checkLevel(double horsePowerUser, double _hpCorrect, double npshUser, double _npshCorrect, string pumpUser, string _pumpCorrect, out int stars)
        {
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
        public static string checkLevel(double horsePowerUser, double _hpCorrect, double npshUser, double _npshCorrect, string pumpUser, string _pumpCorrect, double costUser, double _costCorrect, out int stars)
        {
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



    }
}