using AllocationMethods.Messaging;
using AllocationMethods.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace AllocationMethods.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public sealed class DirectoryViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<DirectoryEntry> _directory = new ObservableCollection<DirectoryEntry>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the DirectoryViewModel class.
        /// </summary>
        public DirectoryViewModel()
        {
            Reset();

            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);
            Messenger.Default.Register<ActionMessage>(this, ConsumeActionMessage);
        }

        private void ConsumeMessage(SimpleMessage message)
        {
            switch(message.Type)
            {
                case Messaging.SimpleMessage.MessageType.SettingsChanged:
                    break;
                case Messaging.SimpleMessage.MessageType.SimulationReset:
                    Reset();
                    break;
                default:
                    break;
            }
        }

        private void ConsumeActionMessage(ActionMessage message)
        {
            switch (message.Type)
            {
                case Messaging.ActionMessage.MessageType.StoreSuccess:
                    Directory.Add(message.PassedDirectoryEntry);
                    RaisePropertyChanged(DirectoryPropertyName);
                    Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.DirectoryCreated, Directory.Count.ToString()));
                    break;
                case Messaging.ActionMessage.MessageType.AttemptToDelete:
                    if(Directory.Count > 0)
                        Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.DirectoryEntryToDelete, FindDirectory(message.Message)));
                    break;
                case Messaging.ActionMessage.MessageType.DeleteSuccess:
                    Directory.Remove(message.PassedDirectoryEntry);
                    RaisePropertyChanged(DirectoryPropertyName);
                    Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.DirectoryDeleted, Directory.Count.ToString()));
                    break;
                default:
                    //Do Nothing
                    break;
            }
        }

       private DirectoryEntry FindDirectory(string fileName)
       {
           switch(Properties.Settings.Default.AllocationType)
           {
               case AllocationType.Contiguous:
                   ContiguousFileDirectoryEntry passedContiguousEntry = new ContiguousFileDirectoryEntry();
                   foreach (ContiguousFileDirectoryEntry entry in Directory)
                   {
                       if (entry.FileName == fileName)
                       {
                           passedContiguousEntry = entry;
                       }

                   }
                   return passedContiguousEntry;
               case AllocationType.Indexed:
                   IndexedFileDirectoryEntry passedIndexedEntry = new IndexedFileDirectoryEntry();
                   foreach (IndexedFileDirectoryEntry entry in Directory)
                   {
                       if (entry.FileName == fileName)
                       {
                           passedIndexedEntry = entry;
                       }
                   }
                   return passedIndexedEntry;
               case AllocationType.Linked:
                   LinkedFileDirectoryEntry passedLinkedEntry = new LinkedFileDirectoryEntry();
                   foreach(LinkedFileDirectoryEntry entry in Directory)
                   {
                       if (entry.FileName == fileName) 
                       {
                           passedLinkedEntry = entry;
                       }

                   }
                   return passedLinkedEntry;
               default:
                   //ERROR
                   return new DirectoryEntry();
           }
       }

        private void Reset()
        {
            
            Directory.Clear();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="Directory" /> property's name.
        /// </summary>
        public const string DirectoryPropertyName = "Directory";

        /// <summary>
        /// Sets and gets the Directory property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<DirectoryEntry> Directory
        {
            get
            {
                return _directory;
            }

            set
            {
                if (_directory == value)
                {
                    return;
                }

                _directory = value;
                RaisePropertyChanged(DirectoryPropertyName);
            }
        }

        #endregion
    }
}