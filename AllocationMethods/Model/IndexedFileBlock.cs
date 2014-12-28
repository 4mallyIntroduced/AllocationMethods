using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    class IndexedFileBlock : FileBlock
    {       
        #region Fields
        //FIXME
        private bool _isIndexBlock;
        private List<int> _indexBlockList;
        #endregion

        #region Constructors

        
        public IndexedFileBlock()
        {
            _isIndexBlock = false;
            IsInUse = false;
            IsOccupied = false;
            _indexBlockList = new List<int>();
            BlockAddress = 0;
            Type = AllocationType.Indexed;
        }

        public IndexedFileBlock(int blockAddress, bool isOccupied, bool isIndexBlock, File storedFile)
        {
            BlockAddress = blockAddress;
            IsOccupied = isOccupied;
            IsIndexBlock = isIndexBlock;
            if (isIndexBlock)
                _indexBlockList = new List<int>();
            StoredFile = storedFile;
            IsInUse = false;
            Type = AllocationType.Indexed;

        } 


        /// <summary>
        /// Default Constructor for Delete and Refreshes
        /// </summary>
        /// <param name="blockAddress"></param>
        public IndexedFileBlock(int blockAddress)
        {
            _isIndexBlock = false;
            IsInUse = false;
            IsOccupied = false;
            BlockAddress = blockAddress;
            Type = AllocationType.Indexed;
            
        }
 
        #endregion

        #region Properties

        public bool IsIndexBlock
        {
            get { return _isIndexBlock; }
            set { _isIndexBlock = value; }
        }

        public List<int> IndexBlockList
        {
            get { return _indexBlockList; }
            set { _indexBlockList = value; }
        }

        
        #endregion

    }
}
