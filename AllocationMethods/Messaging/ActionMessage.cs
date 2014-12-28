using AllocationMethods.Model;
namespace AllocationMethods.Messaging
{
    /// <summary>
    /// A simple message class that we use to pass messages around the application
    /// via the Messenger singleton, but keep the parts sufficiently decoupled
    /// for the MVVM style of work.
    /// </summary>
    public class ActionMessage
    {
        public ActionMessage(MessageType type): this(type, string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type of action being performed</param>
        /// <param name="fileName">Name of file</param>
        public ActionMessage(MessageType type, string message)
        {
            Type = type;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type of action being performed</param>
        /// <param name="fileName">Name of file</param>
        public ActionMessage(MessageType type, int fileName)
        {
            Type = type;
            FileName = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">This action should be only store attempts</param>
        /// <param name="passedFile">Actual File passed, only for store</param>
        public ActionMessage(MessageType type, File passedFile)
        {
            Type = type;
            PassedFile = passedFile;
        }

        public ActionMessage(MessageType type, DirectoryEntry passedDirectoryEntry)
        {
            Type = type;
            PassedDirectoryEntry = passedDirectoryEntry;
        }

        public ActionMessage(MessageType type, File passedFile, DirectoryEntry passedDirectoryEntry)
        {
            Type = type;
            PassedDirectoryEntry = passedDirectoryEntry;
            PassedFile = passedFile;
        }

        public enum MessageType
        {
            AttemptToStore,
            AttemptToDelete,
            AttemptToAccess,
            AttemptToRelease,
            DirectoryEntryToDelete,
            StoreSuccess,
            StoreFail,
            DeleteSuccess,
            DeleteFail,
            AccessSuccess,
            AccessFail,
            ReleaseSuccess,
            ReleaseFail
        }
        public string Message { get; set; }

        public MessageType Type { get; set; }

        public int FileName { get; set; }

        public File PassedFile { get; set; }

        public DirectoryEntry PassedDirectoryEntry { get; set; }

    }
}
