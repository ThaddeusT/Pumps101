using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pumps101.Models
{
    public class Level
    {
        #region Fields
        private int _level;
        private bool _isLocked;
        private int _stars;
        #endregion

        #region Constructors
        public Level(int lv, int stars, bool isLocked)
        {
            _level = lv;
            _stars = stars;
            _isLocked = isLocked;
        }
        #endregion

        #region Properties
        public int Level
        {
            get { return _level; }
        }
        public bool Locked
        {
            get { return _isLocked; }
        }
        public int Stars
        {
            get { return _stars; }
        }
        #endregion
    }
}