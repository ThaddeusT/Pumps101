using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pumps101.Repositories
{
    public class Calculations
    {
        private bool _lvlIsSet;
        private int _level;
        private static double[] _diams;
        private double _diam;		// in inches
        private double _density;	// in (lbm/ft^3)
        private double _time; 	// in hours
        private double _volume;  	// in cubic feet
        private int[] _tankElevation; 	// in feet ( everything is 200 - 800ft higher than 1st tank)
        private double _hpCorrect = 0;  // once they check the value once it will be set so they don't have to recalculate
        private double[] _tankPressure;		// psig
        private int _maxNumberOfChances;

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
			    setLevelOneToThree(chances);
		    }
	    }


        // The Calculations // 


        /// <summary>
        ///  Gets correct horse power based of the values dynamically set for the level
        /// </summary>
        /// <param name="withHeight">Make the calculation accounting for tank height</param>
        /// <param name="withPressure">Make the calculation accounting for tank pressure</param>
        /// <returns>Correct hp to set the pump at</returns>
        private double getHorsePower(bool withHeight, bool withPressure)
        {
            double v2, workPerMass;
            double volumetricFlowRate = (_volume / (_time * 3600));
            double crossSectionalArea = (Math.PI * (Math.Pow(_diam / 12, 2) / 4));


            v2 = volumetricFlowRate / crossSectionalArea;
            if(!withHeight && !withPressure)        workPerMass = getWorkFromVelocities(0, v2);
            else if (withHeight && !withPressure)   workPerMass = getWorkFromVelocities(0, v2) + getWorkFromHeight();            
            else if (!withHeight && withPressure)   workPerMass = getWorkFromVelocities(0, v2) + getPressureFromWork();
            else                                    workPerMass = getWorkFromVelocities(0, v2) + getWorkFromHeight() + getPressureFromWork();
            
            // returns correct hp
            return convertWorkToHorsePower(workPerMass, volumetricFlowRate, _density);
        }

        private double getWorkFromHeight()
        {
            return (_tankElevation[1] - _tankElevation[0]) * 32.174;
        }

        private double getPressureFromWork()
        {
            double deltaP = _tankPressure[1] - _tankPressure[0];

            return (deltaP / _density) * 144 * 32.174;

        }

        // This will always work for getting power!!
        private double convertWorkToHorsePower(double workPerMass, double volumetricFlowRate, double density)
        {
            double hp = (((workPerMass / 32.174) * volumetricFlowRate) * density) / 550;
            return hp;
        }


        private double getWorkFromVelocities(double v1, double v2)
        {
            double work = .5 * (Math.Pow(v2, 2) - Math.Pow(v1, 2));
            return work;
        }

        // Setting Level //

         /// <summary>
         /// Currently, dynamically sets values for levels 1-3 and gets correct HP
         /// </summary>
         /// <param name="chances">the number of chance they have to get it correct</param>
        private void setLevelOneToThree(int chances)
        {
            Random rn = new Random();
            double timeUnrounded = (rn.NextDouble() * (2 - 0.75) + 0.75);

            _diams = new double[] { 0.75, 1, 1.25, 1.5 };
            _density = 62.4;
            _diam = _diams[rn.Next(4)];

            _tankElevation = new int[2];
            _tankPressure = new double[2];
            for (int i = 0; i < 2; i++)
            {
                if (i != 0)
                {
                    _tankElevation[i] = _tankElevation[0] + (200 + rn.Next(800));
                    _tankPressure[i] = (double)Math.Round((_tankPressure[0] + (double)(rn.Next(19) + 5) + rn.NextDouble()) * 10) / 10;
                }
                else
                {
                    _tankElevation[i] = rn.Next(350) + 150;
                    _tankPressure[i] = (double)Math.Round((double)((rn.Next(44) + 5) + rn.NextDouble()) * 10) / 10;
                }

            }

            _time = ((int)Math.Round(timeUnrounded * 100)) / 100.0;
            _volume = rn.Next(3001) + 2000;
            _lvlIsSet = true;
            _maxNumberOfChances = 3;

            if (_hpCorrect == 0)
            {
                _hpCorrect = getHorsePower((_level > 1), (_level > 2));
            }
        }
    }
}