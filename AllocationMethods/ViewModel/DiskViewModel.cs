using AllocationMethods.Messaging;
using AllocationMethods.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllocationMethods.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public sealed class DiskViewModel : ViewModelBase
    {   
        #region Fields
        private DiskModel _diskModel = new DiskModel();
        private ObservableCollection<FileBlock> _blockCollection = new ObservableCollection<FileBlock>();
        private string _title = "Disk";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the DiscViewModel class.
        /// </summary>
        public DiskViewModel()
        {
            _diskModel.ResetList();
            BlockCollection = new ObservableCollection<FileBlock>(_diskModel.Disk);

            AddEventHandlers();

            //  Register against the Messenger singleton to receive any simple
            //  messages.  Specifically the one that says settings have changed.
            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);
            Messenger.Default.Register<ActionMessage>(this, ConsumeActionMessage);
        }

        private void ConsumeActionMessage(ActionMessage message)
        {
            switch (message.Type)
            {
                case Messaging.ActionMessage.MessageType.AttemptToStore:
                    _diskModel.AttemptToStore(message.PassedFile);
                    break;
                case Messaging.ActionMessage.MessageType.DirectoryEntryToDelete:
                    _diskModel.AttemptToDelete(message.PassedDirectoryEntry);
                    break;
                default:
                    //Do Nothing
                    break;
            }
        }

        private void ConsumeMessage(SimpleMessage message)
        {
            switch (message.Type)
            {
                case Messaging.SimpleMessage.MessageType.SettingsChanged:
                    _diskModel.ResetList();
                    BlockCollection = new ObservableCollection<FileBlock>(_diskModel.Disk);
                    break;
                case Messaging.SimpleMessage.MessageType.SimulationTick:
                    break;
                case Messaging.SimpleMessage.MessageType.SimulationReset:
                    _diskModel.ResetList();
                    break;
                default:
                    //Do Nothing
                    break;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";



        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
        }

        /// <summary>
        /// The <see cref="BlockCollection" /> property's name.
        /// </summary>
        public const string BlockCollectionPropertyName = "BlockCollection";



        /// <summary>
        /// Sets and gets the BlockCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<FileBlock> BlockCollection
        {
            get
            {
                return _blockCollection; //return _blockCollection ?? new ObservableCollection<FileBlock>(_diskModel.Disk);
            }

            set
            {
                if (_blockCollection == value)
                {
                    return;
                }

                _blockCollection = value;
                RaisePropertyChanged(BlockCollectionPropertyName);
            }
        }  
     
        #endregion

        #region Methods
        public void OnUpdateDisk(object sender, DiskModelEventArgs e)
        {
            BlockCollection = new ObservableCollection<FileBlock>(_diskModel.Disk);
            SendBlockCounts();
        }

        public void OnStorageFailure(object sender, DiskModelEventArgs e) 
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.StoreFail, e.EventFile));
        }

        public void OnStorageSuccess(object sender, DiskModelEventArgs e)
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.StoreSuccess, e.DirectoryEntry));
        }

        public void OnDeleteSuccess(object sender, DiskModelEventArgs e)
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.DeleteSuccess, e.EventFile, e.DirectoryEntry));
        }

        private void AddEventHandlers()
        {
            _diskModel.DiskChanged += (sender, e) => OnUpdateDisk(sender, e);
            _diskModel.StorageFailure += (sender, e) => OnStorageFailure(sender,e);
            _diskModel.StorageSuccess += (sender, e) => OnStorageSuccess(sender, e);
            _diskModel.DeleteSuccess += (sender, e) => OnDeleteSuccess(sender, e);
        }

        private void SendBlockCounts() 
        {
            int occupied = 0;
            int empty = 0;
            foreach (FileBlock i in BlockCollection)
            {
                if (i.IsOccupied)
                    occupied++;
                else
                    empty++;
            }
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SendEmptyBlockCount, empty));
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SendOccupiedBlockCount, occupied));
        }
        #endregion
    }
}