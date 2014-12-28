using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    class StatisticsModel
    {
     
        #region Fields
        private int _totalTicks;
        private int _diskAddMisses;
        private int _diskAddHits;
        #endregion

        #region Constructors
   
        #endregion

        #region Properties

        public int DiskAddHits
        {
            get { return _diskAddHits; }
            set { _diskAddHits = value; }
        }

        public int DiskAddMisses
        {
            get { return _diskAddMisses; }
            set { _diskAddMisses = value; }
        }

        public int TotalTicks
        {
            get { return _totalTicks; }
            set { _totalTicks = value; }
        }

        #endregion

        #region Methods
        #endregion
        

    }
}
