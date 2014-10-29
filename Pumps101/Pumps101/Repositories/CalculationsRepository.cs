using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pumps101.Models;

namespace Pumps101.Repositories
{
    // lvl 1-6 is getting hp
    // level 7 is getting hp and Net Positive Suction
    public class CalculationsRepository
    {
        private static double[] _diams;
        private double _A;
        private double _B;
        private double _C;
        private double _volumetricFlowRate = 0;

        // 
        // MODEL
        //

        private int _maxNumberOfChances;
        private int _level;

        //lvl 1 - 10
        private double _diam;		// in inches
        private double _density;	// in (lbm/ft^3)
        private double _time; 	    // in hours
        private double _volume;  	// in cubic feet

        //lvl 2 - 10
        private int[] _tankElevation; 	    // in feet ( everything is 200 - 800ft higher than 1st tank)

        // lvl 3 - 10
        private double[] _tankPressure;		// psig

        //lvl 4 - 10
        private double _viscosity;          //  lbm/feet second
        private int[] _vertLength;          // in feet ( ranging 500ft - 1500ft, this is the horizontal pipe segments) 
        
        //lvl 5
        //give them table to calc ev

        //lvl 6 - 10
        private double _efficencyFactor;    // %

        //lvl 7 - 10
        private double _vaporPressure = -1;      // psi

        // lvl 8 - 10
        // give them choice in pump to use
        // "Diaphragm" "Rotary" "Centrifugal"  ... it will never be Diaphram from our setup

        // lvl 9 
        // materials, skip for now

        // lvl 10
        // material can be "Cast Iron" "Cast Steel" "Stainless Steel" "Nickel Alloy"
        private string _material = null;

        
        //
        // ANSWERS
        //
        // level 1-6 
        private double _hp_correct;  // once they check the value once it will be set so they don't have to recalculate
        // level 7 - 10
        private double _NPSH_correct = -1;
        // level 8 - 10
        private string _pumpType_correct = "";
        // level 10
        private double _cost_correct = -1;


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
            _volumetricFlowRate = (_volume / (_time * 3600));
            double crossSectionalArea = (Math.PI * (Math.Pow(_diam / 12, 2) / 4));

            v2 = _volumetricFlowRate / crossSectionalArea;
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
            if(_level > 5)
            {
                return convertWorkToHorsePower(workPerMass, _volumetricFlowRate, _density) / _efficencyFactor;
            }
            else 
            { 
                return convertWorkToHorsePower(workPerMass, _volumetricFlowRate, _density);
            }
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
            return (((workPerMass / 32.174) * volumetricFlowRate) * density) / 550;
        }

        // used by level 1-10
        private double getWorkFromVelocities(double v1, double v2)
        {
            return .5 * (Math.Pow(v2, 2) - Math.Pow(v1, 2));
        }

        // Net Positive Suction Head
        // Level 7 - 10
        private double getNetPositiveSuctionHead()
        {
            return (144 / _density) * (_tankPressure[0] - _vaporPressure) + (_tankElevation[0] - _tankElevation[1]) - (_vertLength[0] + _tankElevation[0]);
        }

        // Based on flow rate decide what pump to use
        // level 8 - 10
        private string getPumpToUse()
        {
            if(_volumetricFlowRate <= 0.006)
            {
                return "Diaphragm";
            }
            else if(_volumetricFlowRate <= 0.1)
            {
                return "Rotary";
            }
            else
            {
                return "Centrifugal";
            }
        }

        // level 10
        private double getCost()
        {
            double baseCost = 0;
            double pressure = (_hp_correct * 1641.61) / (_volumetricFlowRate * 32.174);

            if(_pumpType_correct.CompareTo("Rotary") == 0)
            {
                baseCost = 34933 * Math.Pow((_volumetricFlowRate * 0.02832), 0.4041);
                if (_material.CompareTo("Cast Iron") == 0)
                {
                    // do nothing
                }
                else if (_material.CompareTo("Cast Steel") == 0)
                {
                    baseCost = baseCost * 1.4;
                }
                else if (_material.CompareTo("Stainless Steel") == 0)
                {
                    baseCost = baseCost * 2;
                }
                else
                {
                    baseCost = baseCost * 4;
                }

                if (pressure <= 1035)
                {
                    return baseCost;
                }
                else if (pressure <= 5000)
                {
                    return baseCost * 2;
                }
                else if (pressure <= 10000)
                {
                    return baseCost * 2.5;
                }
                else if (pressure <= 20000)
                {
                    return baseCost * 3.1;
                }
                else
                {
                    return baseCost * 3.4;
                }
            }
            else if(_pumpType_correct.CompareTo("Centrifugal") == 0)
            {
               baseCost = 18116 * Math.Pow((_volumetricFlowRate * 0.02832), 0.3638);

               if (_material.CompareTo("Cast Iron") == 0)
               {
                   // do nothing
               }
               else if (_material.CompareTo("Cast Steel") == 0)
               {
                   baseCost = baseCost * 1.8;
               }
               else if (_material.CompareTo("Stainless Steel") == 0)
               {
                   baseCost = baseCost * 2.4;
               }
               else
               {
                   baseCost = baseCost * 5;
               }

               if (pressure <= 1035)
               {
                   return baseCost;
               }
               else if (pressure <= 5000)
               {
                   return baseCost * 2.1;
               }
               else if (pressure <= 10000)
               {
                   return baseCost * 2.8;
               }
               else if (pressure <= 20000)
               {
                   return baseCost * 3.5;
               }
               else
               {
                   return baseCost * 4;
               }
            }
            return baseCost;

        }

        // Setting Level //

        /// <summary>
        /// Currently, dynamically sets values for levels 1-10 and gets correct HP and all other correct values
        /// </summary>
        /// <param name="chances">the number of chance they have to get it correct</param>
        public LevelModel getLevel(int level, int chances)
        {
            _level = level;
            Random rn = new Random();
            if (_level < 4) { 
                _diams = new double[] { 0.75, 1, 1.25, 1.5 };
                _diam = _diams[rn.Next(4)];
            }
            // later levels have more options for diam
            else
            {
                _diams = new double[] { 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5};
                _diam = _diams[rn.Next(8)];
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
            _efficencyFactor = rn.Next(65, 86) * 0.001;
            _tankElevation = new int[2];
            _tankPressure = new double[2];
            for (int i = 0; i < 2; i++)
            {
                if (i != 0)
                {
                    _tankElevation[i] = _tankElevation[0] + (200 + rn.Next(801));
                    _tankPressure[i] = (double)Math.Round((_tankPressure[0] + (double)rn.Next(5,25) + rn.NextDouble()) * 10) / 10;
                }
                else
                {
                    _tankElevation[i] = rn.Next(351) + 150;
                    _tankPressure[i] = (double)Math.Round((double)(rn.Next(5,50) + rn.NextDouble()) * 10) / 10;
                }

            }
            _vaporPressure = _tankPressure[0] - rn.Next(8, 16);
            if (_vaporPressure < 3) { _vaporPressure = 3; }
            _vertLength = new int[2];
            _vertLength[0] = rn.Next(500, 1501);
            _vertLength[1] = rn.Next(500, 1501);
            _viscosity = rn.Next(526, 1202) * 0.0001;
            double timeUnrounded = 0;

            // use higher volume and less time for later levels
            if (_level > 7)
            {
                timeUnrounded = (rn.NextDouble() * (1.25) + 0.25);
                _volume = rn.Next(5000, 10001);
            }
            else
            {
                timeUnrounded = (rn.NextDouble() * (2 - 0.75) + 0.75);
                _volume = rn.Next(2000, 5001);
            }
            _time = ((int)Math.Round(timeUnrounded * 100)) / 100.0;
            _maxNumberOfChances = chances;
            _hp_correct = getHorsePower((_level > 1), (_level > 2), (_level > 3 && _level < 6));
            
            if(_level > 6){
                _NPSH_correct = getNetPositiveSuctionHead();
            }
            if(_level > 7)
            {
                _pumpType_correct = getPumpToUse();
            }
            // set material for level 10 since there is no level 9 where they select it themself
            if(_level > 9)
            {
                string[] materialArr = new string[] { "Cast Iron", "Cast Steel", "Stainless Steel", "Nickel Alloy" };
                _material = materialArr[rn.Next(4)];
                _cost_correct = getCost();
            }

            return new LevelModel(_level,_maxNumberOfChances, _diam, _density, _time, _volume, _tankElevation, _tankPressure, _viscosity, _vertLength, _efficencyFactor, _vaporPressure, _material, _hp_correct, _NPSH_correct, _pumpType_correct, _cost_correct);
        }
    }
}