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
        private bool _is_complete;
        private int _stars;
        #endregion

        #region Constructors
        public Level(int lv, int stars, bool completed, bool isLocked)
        {
            _level = lv;
            _stars = stars;
            _isLocked = isLocked;
            _is_complete = completed;
        }
        #endregion

        #region Properties
        public int LevelNum
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
        public bool Is_Complete
        {
            get { return _is_complete; }
        }
        #endregion
    }
}