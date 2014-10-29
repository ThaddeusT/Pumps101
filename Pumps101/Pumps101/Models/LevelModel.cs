using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pumps101.Models
{
    public class LevelModel
    {

        public int _maxNumberOfChances;
        public int _level;

        //lvl 1 - 10
        public double _diam;		// in inches
        public double _density;	// in (lbm/ft^3)
        public double _time; 	    // in hours
        public double _volume;  	// in cubic feet

        //lvl 2 - 10
        public int _tankOneElevation; 	    // in feet ( everything is 200 - 800ft higher than 1st tank)
        public int _tankTwoElevation;

        // lvl 3 - 10
        public double _tankOnePressure;		// psig
        public double _tankTwoPressure;

        //lvl 4 - 10
        public double _viscosity;          //  lbm/feet second
        public int _vertLengthOne;          // in feet ( ranging 500ft - 1500ft, this is the horizontal pipe segments) 
        public int _vertLengthTwo;  

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
        public string _material;


        //
        // ANSWERS
        //
        // level 1-6 
        public double _hp_correct;  // once they check the value once it will be set so they don't have to recalculate
        // level 7 - 10
        public double _NPSH_correct;
        // level 8 - 10
        public string _pumpType_correct;
        // level 10
        public double _cost_correct;


        public LevelModel(int level, int chances, double diam, double density, double time, double volume, int[] tankElevation, double[] tankPressure, double viscosity, int[] vertLength, double efficFactor, double vaporPressure, string material, double hp_correct, double NPSH_correct, string pumpType_correct, double cost_correct)
        {
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
        }
    }
}