using System;

namespace AllocationMethods.Model
{
    /// <summary>
    /// Simple EventArgs class for the Disk model.
    /// </summary>
    public class DiskModelEventArgs : EventArgs
    {
        private DirectoryEntry _directoryEntry;

        public DirectoryEntry DirectoryEntry
        {
            get { return _directoryEntry; }
            set { _directoryEntry = value; }
        }

        private File _eventFile;

        public File EventFile
        {
            get { return _eventFile; }
            set { _eventFile = value; }
        }
        

        public DiskModelEventArgs()
        {
        }

        /// <summary>
        /// For use with disk storage sucess events
        /// </summary>
        /// <param name="directoryEntry"></param>
        public DiskModelEventArgs(DirectoryEntry directoryEntry)
        {
            DirectoryEntry = directoryEntry;
        }

        /// <summary>
        /// For use with disk delete success events
        /// </summary>
        /// <param name="directoryEntry"></param>
        public DiskModelEventArgs(File eventFile, DirectoryEntry directoryEntry)
        {
            DirectoryEntry = directoryEntry;
            EventFile = eventFile;
        }

        /// <summary>
        /// For use with disk storage failure events
        /// </summary>
        /// <param name="directoryEntry"></param>
        public DiskModelEventArgs(File eventFile)
        {
            EventFile = eventFile;
        }
    }
}
