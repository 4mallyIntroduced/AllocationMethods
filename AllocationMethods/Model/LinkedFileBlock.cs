using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    class LinkedFileBlock : FileBlock
    {

        #region Fields
        //FIXME
        private int _nextBlockOfFile;
        private bool _hasPointer; //FIXME - Move to FileBlock?
        #endregion

        #region Constructors

        public LinkedFileBlock(int blockAddress)
        {
            BlockAddress = blockAddress;
            IsInUse = false;
            IsOccupied = false;
            Type = AllocationType.Linked;
        }  

        public LinkedFileBlock(int blockAddress, File storedFile)
        {
            
            BlockAddress = blockAddress;
            StoredFile = storedFile;
            IsInUse = false;
            IsOccupied = true;
            Type = AllocationType.Linked;

            //Must Be Manually Set
            _hasPointer = false;
            _nextBlockOfFile = 0;
            
        }  
        #endregion

        #region Properties

        public int NextBlockOfFile
        {
            get { return _nextBlockOfFile; }
            set { _nextBlockOfFile = value; }
        }

        public bool HasPointer
        {
            get { return _hasPointer; }
            set { _hasPointer = value; }
        }

        #endregion

    }
}
