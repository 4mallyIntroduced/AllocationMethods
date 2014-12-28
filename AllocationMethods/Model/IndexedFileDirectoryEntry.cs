

namespace AllocationMethods.Model
{
    class IndexedFileDirectoryEntry : DirectoryEntry
    {
        #region Fields
        private int _indexBlockReference;
        #endregion

        #region Constructors

        public IndexedFileDirectoryEntry()
        {
            _indexBlockReference = 0;
            Type = AllocationType.Indexed;

        }

        public IndexedFileDirectoryEntry(string fileName, int indexBlockReference)
        {
            FileName = fileName;
            IndexBlockReference = indexBlockReference;
            Type = AllocationType.Indexed;

        }

        #endregion

        #region Properties
        public int IndexBlockReference
        {
            get { return _indexBlockReference; }
            set { _indexBlockReference = value; }
        }
        #endregion



    }
}
