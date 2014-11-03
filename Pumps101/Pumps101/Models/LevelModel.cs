using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pumps101.Models
{
    public class LevelModel
    {
        #region Fields 

        private int _level_id;
        private int _maxNumberOfChances;
        private int _level;
        private double _hpGuess;
        //lvl 1 - 10
        private double _diam;		// in inches
        private double _density;	// in (lbm/ft^3)
        private double _time; 	    // in hours
        private double _volume;  	// in cubic feet
        //lvl 2 - 10
        private int _tankOneElevation; 	    // in feet ( everything is 200 - 800ft higher than 1st tank)
        private int _tankTwoElevation;
        // lvl 3 - 10
        private double _tankOnePressure;		// psig
        private double _tankTwoPressure;
        //lvl 4 - 10
        private double _viscosity;          //  lbm/feet second
        private int _vertLengthOne;          // in feet ( ranging 500ft - 1500ft, this is the horizontal pipe segments) 
        private int _vertLengthTwo;  
        //lvl 5
        //give them table to calc ev
        //lvl 6 - 10
        private double _efficencyFactor;    // %
        //lvl 7 - 10
        private double _vaporPressure;      // psi
        // lvl 8 - 10
        // give them choice in pump to use
        // "Diaphragm" "Rotary" "Centrifugal"  ... it will never be Diaphram from our setup
        // lvl 9 
        // materials, skip for now
        // lvl 10
        // material can be "Cast Iron" "Cast Steel" "Stainless Steel" "Nickel Alloy"
        private string _material;

        //
        // ANSWERS
        //
        // level 1-6 
        private double _hp_correct;  // once they check the value once it will be set so they don't have to recalculate
        // level 7 - 10
        private double _NPSH_correct;
        // level 8 - 10
        private string _pumpType_correct;
        // level 10
        private double _cost_correct;

        #endregion

        #region Constructors

        public LevelModel()
        {
        }

        public LevelModel(int lv_id, int level, int chances, double diam, double density, double time, double volume, int[] tankElevation, double[] tankPressure, double viscosity, int[] vertLength, double efficFactor, double vaporPressure, string material, double hp_correct, double NPSH_correct, string pumpType_correct, double cost_correct)
        {
            _level_id = lv_id;
            _level = level;
            _maxNumberOfChances = chances;
            _diam = diam;
            _density = density;
            _time = time;
            _volume = volume;
            _tankOneElevation = tankElevation[0];
            _tankTwoElevation = tankElevation[1];
            _tankOnePressure = tankPressure[0];
            _tankTwoPressure = tankPressure[1];
            _viscosity = viscosity;
            _vertLengthOne = vertLength[0];
            _vertLengthTwo = vertLength[1];
            _efficencyFactor = efficFactor;
            _vaporPressure = vaporPressure;
            _material = material;
            _hp_correct = hp_correct;
            _NPSH_correct = NPSH_correct;
            _pumpType_correct = pumpType_correct;
            _cost_correct = cost_correct;
            List<KeyValuePair<string, string>> pumpTypes = new List<KeyValuePair<string, string>>();
            pumpTypes.Add(new KeyValuePair<string, string>("Diaphragm", "Diaphragm"));
            pumpTypes.Add(new KeyValuePair<string, string>("Rotary", "Rotary"));
            pumpTypes.Add(new KeyValuePair<string, string>("Centrifugal", "Centrifugal"));
            PumpTypes = pumpTypes;
        }

        public LevelModel(int lv_id, int level, int chances, double diam, double density, double time, double volume, int[] tankElevation, double[] tankPressure, double viscosity, int[] vertLength, double efficFactor, double vaporPressure, string material)
        {
            _level_id = lv_id;
            _level = level;
            _maxNumberOfChances = chances;
            _diam = diam;
            _density = density;
            _time = time;
            _volume = volume;
            _tankOneElevation = tankElevation[0];
            _tankTwoElevation = tankElevation[1];
            _tankOnePressure = tankPressure[0];
            _tankTwoPressure = tankPressure[1];
            _viscosity = viscosity;
            _vertLengthOne = vertLength[0];
            _vertLengthTwo = vertLength[1];
            _efficencyFactor = efficFactor;
            _vaporPressure = vaporPressure;
            _material = material;
            List<KeyValuePair<string, string>> pumpTypes = new List<KeyValuePair<string, string>>();
            pumpTypes.Add(new KeyValuePair<string, string>("", ""));
            pumpTypes.Add(new KeyValuePair<string, string>("Diaphragm", "Diaphragm"));
            pumpTypes.Add(new KeyValuePair<string, string>("Rotary", "Rotary"));
            pumpTypes.Add(new KeyValuePair<string, string>("Centrifugal", "Centrifugal"));
            PumpTypes = pumpTypes;
        }

        public LevelModel(int level, Guid User, Boolean Authenticated)
        {
            _level = level;
            LevelsCompleted = new LevelsComplete(User, Authenticated);
        }

        #endregion

        #region Properties

        public int MaxNumberOfChances
        {
            get
            {
                return _maxNumberOfChances;
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }
        }

        [Display(Name = "Horse Power:")]
        public double HPGuess
        {
            get
            {
                return _hpGuess;
            }
            set
            {
                if(value != _hpGuess)
                {
                    _hpGuess = value;
                }
            }
        }

        public double Diameter
        {
            get
            {
                return _diam;
            }
        }

        public double Density
        {
            get
            {
                return _density;
            }
        }
        public double Time
        {
            get
            {
                return _time;
            }
        }

        public double Volume
        {
            get
            {
                return _volume;
            }
        }
        public int TankOneElevation
        {
            get
            {
                return _tankOneElevation;
            }
        }

        public int TankTwoElevation
        {
            get
            {
                return _tankTwoElevation;
            }
        }

        public double TankOnePressure
        {
            get
            {
                return _tankOnePressure;
            }
        }

        public double TankTwoPressure
        {
            get
            {
                return _tankTwoPressure;
            }
        }

        public double Viscosity
        {
            get
            {
                return _viscosity;
            }
        }

        public int VertLengthOne
        {
            get
            {
                return _vertLengthOne;
            }
        }

        public int VertLengthTwo
        {
            get
            {
                return _vertLengthTwo;
            }
        }


        public double EfficencyFactor
        {
            get
            {
                return _efficencyFactor;
            }
        }

        public double VaporPressure
        {
            get
            {
                return _vaporPressure;
            }
        }

        public string Material
        {
            get
            {
                return _material;
            }
        }

        public double HP_correct
        {
            get
            {
                return _hp_correct;
            }
        }

        public double NPSH_correct
        {
            get
            {
                return _NPSH_correct;
            }
        }

        public string PumpType_correct
        {
            get
            {
                return _pumpType_correct;
            }
        }

        public double Cost_correct
        {
            get
            {
                return _cost_correct;
            }
        }

        public LevelsComplete LevelsCompleted { get; set; }


        public int LevelId
        {
            get
            {
                return _level_id;
            }
        }

        public IEnumerable<KeyValuePair<string, string>> PumpTypes { get; set; }

        public String PumpType { get; set; }
        #endregion

    }
}