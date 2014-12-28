using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    public class DirectoryEntry
    {
        #region Fields

        private string _fileName;
        private AllocationType _type;

        #endregion

        #region Constructors

        public DirectoryEntry()
        {
            _fileName = string.Empty;

        }

        #endregion

        #region Properties
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public AllocationType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

    }
}
