namespace AllocationMethods.Model
{
    class LinkedFileDirectoryEntry : DirectoryEntry
    {
        #region Fields
        private int _startingFileBlockAddress;
        private int _endingFileBlockAddress;
        #endregion

        #region Constructors
        public LinkedFileDirectoryEntry()
        {
            _startingFileBlockAddress = 0;
            _endingFileBlockAddress = 0;
            Type = AllocationType.Linked;

        }
    
        public LinkedFileDirectoryEntry(string fileName, int startingFileBlockAddress, int endingFileBlockAddress)
        {
            FileName = fileName;
            StartingFileBlockAddress = startingFileBlockAddress;
            EndingFileBlockAddress = endingFileBlockAddress;
            Type = AllocationType.Linked;
        }       

        #endregion

        #region Properties

        public int StartingFileBlockAddress
        {
            get { return _startingFileBlockAddress; }
            set { _startingFileBlockAddress = value; }
        }

        public int EndingFileBlockAddress
        {
            get { return _endingFileBlockAddress; }
            set { _endingFileBlockAddress = value; }
        }

        #endregion
    }
}
