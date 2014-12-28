using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Models
{
    class DirectoryModel
    {
        #region Fields
        private List<DirectoryEntry> _directory;
        #endregion

        #region Constructors
        #endregion

        #region Properties
        public List<DirectoryEntry> Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }
        #endregion

        #region Methods
        #endregion
    }
}
