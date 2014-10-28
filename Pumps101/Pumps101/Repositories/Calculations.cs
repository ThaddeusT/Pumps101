using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pumps101.Repositories
{

    // lvl 1-6 is getting hp
    // level 7 is getting hp and Net Positive Suction
    public class Calculations
    {
        private bool _lvlIsSet;
        private int _level;
        private static double[] _diams;
        private double _A;
        private double _B;
        private double _C;
        // model
        private double _diam;		// in inches
        private double _density;	// in (lbm/ft^3)
        private double _time; 	// in hours
        private double _volume;  	// in cubic feet
        private int[] _tankElevation; 	// in feet ( everything is 200 - 800ft higher than 1st tank)
        private int[] _vertLength;      // in feet ( ranging 500ft - 1500ft, this is the horizontal pipe segments)        
        private double[] _tankPressure;		// psig
        private double _viscosity;      //  lbm/feet second
        private double _vaporPressure;   // psi
        private int _maxNumberOfChances;

        // answers
        // level 1-6 
        private double _hp_correct;  // once they check the value once it will be set so they don't have to recalculate
        // level 7 - 10
        private double _NPSH_correct;

        public double getPipeDiam() { return _diam; }
        public double getLiquidDensity() { return _density; }
        public double getTime() { return _time; }
        public double getVolume() { return _volume; }
        public int[] getTankElevations() { return _tankElevation; }
        public double getCorrectHP() { return _hpCorrect; }
        public double[] getTankPressure() { return _tankPressure; }
        public bool isLvlSet() { return _lvlIsSet; }


        Calculations(int level, int chances)
	    {
            _level = level;
		    if(level > 0 && level < 4)
		    {
			    setLevel(chances);
		    }
	    }


        // The Calculations // 


        /// <summary>
        ///  Gets correct horse power based of the values dynamically set for the level
        ///  Used by level 1-10
        /// </summary>
        /// <param name="withHeight"> lvl 2 - 6 = Make the calculation accounting for tank height</param>
        /// <param name="withPressure"> lvl 3 - 6 = Make the calculation accounting for tank pressure</param>
        /// <param name="Re"> level 4 - 5 = Reynalds number (not used by later levels </param>
        /// <returns>Correct hp to set the pump at</returns>
        private double getHorsePower(bool withHeight, bool withPressure, bool Re)
        {
            double v2;
            double workPerMass = 0;
            double volumetricFlowRate = (_volume / (_time * 3600));
            double crossSectionalArea = (Math.PI * (Math.Pow(_diam / 12, 2) / 4));

            v2 = volumetricFlowRate / crossSectionalArea;
            if(!withHeight && !withPressure)        workPerMass = getWorkFromVelocities(0, v2);
            else if (withHeight && !withPressure)   workPerMass = getWorkFromVelocities(0, v2) + getWorkFromHeight();
            else                                    workPerMass = getWorkFromVelocities(0, v2) + getWorkFromHeight() + getPressureFromWork();
            
            // level 4
            if (_level > 3)
            {
                double f;
                int L = _vertLength[0] + _vertLength[1] + _tankElevation[0] + _tankElevation[1];
                // only lvl 4 and 5
                if (Re) { 
                    double rey = (_density * v2 * (_diam / 12)) / _viscosity;
                    f = 0.0791 / Math.Pow(rey, 0.25);
                }
                else
                {
                    f = (((_A * Math.Pow(v2, 2)) + (_B * v2) + _C) * (2 * (_diam / 12))) / (_density* Math.Pow(v2, 2)) ;
                }
                
                workPerMass += 0.5 * Math.Pow(v2, 2) * (L / (_diam / 48)) * f;

                if (_level > 4)
                {
                    // value 3.4 assumes we always have 2 90 degree elbows (always 1.5) and 2gate valve(open) always (0.2)
                    workPerMass += 0.5 * Math.Pow(v2, 2) * 3.4;
                }
            }

            // returns correct hp
            return convertWorkToHorsePower(workPerMass, volumetricFlowRate, _density);
        }

        // used by level 2 - 10
        private double getWorkFromHeight()
        {
            return (_tankElevation[1] - _tankElevation[0]) * 32.174;
        }

        // used by level 3 - 10
        private double getPressureFromWork()
        {
            double deltaP = _tankPressure[1] - _tankPressure[0];

            return (deltaP / _density) * 144 * 32.174;

        }

        // This will always work for getting power!! 
        // used by level 1-10
        private double convertWorkToHorsePower(double workPerMass, double volumetricFlowRate, double density)
        {
            double hp = (((workPerMass / 32.174) * volumetricFlowRate) * density) / 550;
            return hp;
        }

        // used by level 1-10
        private double getWorkFromVelocities(double v1, double v2)
        {
            double work = .5 * (Math.Pow(v2, 2) - Math.Pow(v1, 2));
            return work;
        }


        // Net Positive Suction Head
        // Level 7 -10
        private double getNetPositiveSuctionHead()
        {
            return (144 / _density) * (_tankPressure[0] - _vaporPressure) + (_tankElevation[0] - _tankElevation[1]) - (_vertLength[0] + _tankElevation[0]);
        }

        // 


        // Setting Level //

         /// <summary>
         /// Currently, dynamically sets values for levels 1-3 and gets correct HP
         /// </summary>
         /// <param name="chances">the number of chance they have to get it correct</param>
        private void setLevel(int chances)
        {
            Random rn = new Random();
            double timeUnrounded = (rn.NextDouble() * (2 - 0.75) + 0.75);

            if (_level < 4) { 
                _diams = new double[] { 0.75, 1, 1.25, 1.5 };
                _diam = _diams[rn.Next(4)];
            }
            else
            {
                _diams = new double[] { 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5};
                _diam = _diams[rn.Next(7)];
                switch ((int)(_diam*10))
                {
                    case 15:
                        _A = 0.104;
                        _B = 0.1111;
                        _C = -0.0606;
                        break;
                    case 20:
                        _A = 0.075;
                        _B = 0.0812;
                        _C = -0.0494;
                        break;
                    case 25:
                        _A = 0.0601;
                        _B = 0.0667;
                        _C = -0.0445;
                        break;
                    case 30:
                        _A = 0.046;
                        _B = 0.0518;
                        _C = -0.0372;
                        break;
                    case 35:
                        _A = 0.0385;
                        _B = 0.0432;
                        _C = -0.0334;
                        break;
                    case 40:
                        _A = 0.033;
                        _B = 0.0373;
                        _C = -0.0311;
                        break;
                    case 45:
                        _A = 0.0251;
                        _B = 0.027;
                        _C = -0.0209;
                        break;
                    case 50:
                        _A = 0.104;
                        _B = 0.1111;
                        _C = -0.0606;
                        break;
                }

            }
            _density = 62.4;

            _tankElevation = new int[2];
            _tankPressure = new double[2];
            for (int i = 0; i < 2; i++)
            {
                if (i != 0)
                {
                    _tankElevation[i] = _tankElevation[0] + (200 + rn.Next(800));
                    _tankPressure[i] = (double)Math.Round((_tankPressure[0] + (double)rn.Next(5,24) + rn.NextDouble()) * 10) / 10;
                }
                else
                {
                    _tankElevation[i] = rn.Next(350) + 150;
                    _tankPressure[i] = (double)Math.Round((double)(rn.Next(5,49) + rn.NextDouble()) * 10) / 10;
                }

            }
            _vaporPressure = _tankPressure[0] - rn.Next(8, 15);
            if (_vaporPressure < 3) { _vaporPressure = 3; }

            _vertLength[0] = rn.Next(500, 1500);
            _vertLength[1] = rn.Next(500, 1500);
            _viscosity = rn.Next(526, 1201) * 0.0001; 
            _time = ((int)Math.Round(timeUnrounded * 100)) / 100.0;
            _volume = rn.Next(3001) + 2000;
            _lvlIsSet = true;
            _maxNumberOfChances = 3;

            _hp_correct = getHorsePower((_level > 1), (_level > 2), (_level > 3 && _level < 6));
            
            if(_level > 6){
                _NPSH_correct = getNetPositiveSuctionHead();
            }
            
        }
    }
}