using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    public class FileBlock : ViewModelBase
    {

        #region Fields
        private File _storedFile;
        private bool _isOccupied;
        private bool _isInUse;
        private int _blockAddress;

        //NEXT Improve and add space to blocks
        private int _totalMemory;
        private int _pointerMemoryUsed;
        private int _fileMemoryUsed;
        #endregion

        #region Constructors

        public FileBlock()
        {
            _blockAddress = 0;
            _isOccupied = false;
            _isInUse = false;
        }

        public FileBlock(int blockAddress)
        {
            BlockAddress = blockAddress;
            _isOccupied = false;
            _isInUse = false;
        }

        #endregion

        #region Properties

        public bool IsOccupied
        {
            get { return _isOccupied; }
            set { _isOccupied = value; }
        }

        public bool IsInUse
        {
            get { return _isInUse; }
            set 
            { 
                _isInUse = value;
                // Call OnPropertyChanged whenever the property is updated
                RaisePropertyChanged("IsInUse");
            }
        }

        public File StoredFile
        {
            get { return _storedFile; }
            set {   _storedFile = value;
                    // Call OnPropertyChanged whenever the property is updated
                    RaisePropertyChanged("StoredFile");
            }
        }

        public int BlockAddress
        {
            get { return _blockAddress; }
            set { _blockAddress = value; }
        }


        private AllocationType type;

        public AllocationType Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion
    }
}
