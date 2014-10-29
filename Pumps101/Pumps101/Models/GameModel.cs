using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pumps101.Models
{
    public class GameModel
    {
        #region Fields

        //General Parameters of Game Components
        private int m_level;
<<<<<<< .mine
        private double m_pipeDiameter;
        private double m_liquidDensity;
        private double m_time;
        private double m_volume;
        private int m_tank1Elevation;
        private int m_tank2Elevation;
        private double m_tank1Pressure;
        private double m_tank2Pressure;
=======








>>>>>>> .theirs

        //View Parameters
        private int m_container1X;
        private int m_container1Y;
        private int m_container2X;
        private int m_container2Y;

        #endregion

        #region Constructors

        public GameModel(int Level, double PipeDiameter, double LiquidDensity, double Time, double Volume, int Tank1Elevation,
            int Tank2Elevation, double Tank1Pressure, double Tank2Pressure)
        {
            m_level = Level;
            m_pipeDiameter = PipeDiameter;
            m_liquidDensity = LiquidDensity;
            m_time = Time;
            m_volume = Volume;
            m_tank1Elevation = Tank1Elevation;
            m_tank2Elevation = Tank2Elevation;
            m_tank1Pressure = Tank1Pressure;
            m_tank2Pressure = Tank2Pressure;
        }

        #endregion

        #region Properties

        public int Level
        {
            get
            {
                return m_level;
            }
            set
            {
                if (value != m_level)
                {
                    m_level = value;
                }
            }
        }

        public double PipeDiameter
        {
            get
            {
                return m_pipeDiameter;
            }
            set
            {
                if (value != m_pipeDiameter)
                {
                    m_pipeDiameter = value;
                }
            }
        }

        public double LiquidDensity
        {
            get
            {
                return m_liquidDensity;
            }
            set
            {
                if (value != m_liquidDensity)
                {
                    m_liquidDensity = value;
                }
            }
        }

        public double Time
        {
            get
            {
                return m_time;
            }
            set
            {
                if (value != m_time)
                {
                    m_time = value;
                }
            }
        }

        public double Volume
        {
            get
            {
                return m_volume;
            }
            set
            {
                if (value != m_volume)
                {
                    m_volume = value;
                }
            }
        }

        public int Tank1Elevation
        {
            get
            {
                return m_tank1Elevation;
            }
            set
            {
                if (value != m_tank1Elevation)
                {
                    m_tank1Elevation = value;
                }
            }
        }

        public int Tank2Elevation
        {
            get
            {
                return m_tank2Elevation;
            }
            set
            {
                if (value != m_tank2Elevation)
                {
                    m_tank2Elevation = value;
                }
            }
        }

        public double Tank1Pressure
        {
            get
            {
                return m_tank1Pressure;
            }
            set
            {
                if (value != m_tank1Pressure)
                {
                    m_tank1Pressure = value;
                }
            }
        }

        public double Tank2Pressure
        {
            get
            {
                return m_tank2Pressure;
            }
            set
            {
                if (value != m_tank2Pressure)
                {
                    m_tank2Pressure = value;
                }
            }
        }

        #endregion
    }
}