using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    class ContiguousFileDirectoryEntry : DirectoryEntry
    {
        #region Fields
        private int _startingFileBlockAddress;
        private int _fileBlockLength;

        #endregion

        #region Constructors

        public ContiguousFileDirectoryEntry()
        {
            _startingFileBlockAddress = 0;
            _fileBlockLength = 0;
            Type = AllocationType.Contiguous;
        }

        public ContiguousFileDirectoryEntry(string fileName, int startingBlockAddress, int fileBlockLength)
        {
            FileName = fileName;
            StartingFileBlockAddress = startingBlockAddress;
            FileBlockLength = fileBlockLength;
            Type = AllocationType.Contiguous;
        }

        #endregion

        #region Properties

        public int StartingFileBlockAddress
        {
            get { return _startingFileBlockAddress; }
            set { _startingFileBlockAddress = value; }
        }

        public int FileBlockLength
        {
            get { return _fileBlockLength; }
            set { _fileBlockLength = value; }
        }

        #endregion

    }
}
