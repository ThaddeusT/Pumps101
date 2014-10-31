using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public LevelsComplete(Guid user)
        {
            // TODO: get all levels for the user
            // if level hasnt been created in database, add it
            // set levels
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
