using AllocationMethods.Messaging;
using AllocationMethods.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AllocationMethods.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public sealed class StatisticsViewModel : ViewModelBase
    {
        #region Fields
        private StatisticsModel _statisticsModel = new StatisticsModel();
        private bool _isEnabled = false;
        private int _attemptsToStore = 0;
        private int _attemptsToDelete = 0;
        private int _attemptsToAccess = 0;
        private int _attemptsToRelease = 0;
        private int _totalRandomActions = 0;
        private int _storeFailures = 0;
        private int _storeSuccesses = 0;
        private string _StoreProbability = "0.00 %";
        private int _totalEntriesDeleted = 0;
        private int _totalEntriesCreated = 0;
        private string _DirectoryCount = "0";
        private int _deleteFailures = 0;
        private int _deleteSuccesses = 0;
        private List<int> _fileList = new List<int>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the StatisticsViewModel class.
        /// </summary>
        public StatisticsViewModel()
        {

            //Bind Commands
            Reset = new RelayCommand(() => ResetStatistics());

            //Reset Property Settings
            ResetStatistics();

            //  Register against the Messenger singleton to receive any simple
            //  messages.  Specifically the one that says settings have changed.
            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);
            Messenger.Default.Register<ActionMessage>(this, ConsumeActionMessage);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The <see cref="IsEnabled" /> property's name.
        /// </summary>
        public const string IsEnabledPropertyName = "IsEnabled";
        /// <summary>
        /// Sets and gets the IsEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                if (_isEnabled == value)
                {
                    return;
                }

                _isEnabled = value;
                RaisePropertyChanged(IsEnabledPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DiskAddHits" /> property's name.
        /// </summary>
        public const string DiskAddHitsPropertyName = "DiskAddHits";
        /// <summary>
        /// Sets and gets the DiskAddHits property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DiskAddHits
        {
            get
            {
                return _statisticsModel.DiskAddHits;
            }

            set
            {
                if (_statisticsModel.DiskAddHits == value)
                {
                    return;
                }

                _statisticsModel.DiskAddHits = value;
                RaisePropertyChanged(DiskAddHitsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DiskAddHits" /> property's name.
        /// </summary>
        public const string DiskAddMissesPropertyName = "DiskAddMisses";
        /// <summary>
        /// Sets and gets the DiskAddHits property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DiskAddMisses
        {
            get
            {
                return _statisticsModel.DiskAddMisses;
            }

            set
            {
                if (_statisticsModel.DiskAddMisses == value)
                {
                    return;
                }

                _statisticsModel.DiskAddMisses = value;
                RaisePropertyChanged(DiskAddMissesPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Ticks" /> property's name.
        /// </summary>
        public const string TotalTicksPropertyName = "TotalTicks";
        /// <summary>
        /// Sets and gets the Ticks property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TotalTicks
        {
            get
            {
                return _statisticsModel.TotalTicks;
            }

            set
            {
                if (_statisticsModel.TotalTicks == value)
                {
                    return;
                }

                _statisticsModel.TotalTicks = value;
                RaisePropertyChanged(TotalTicksPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AttemptsToStore" /> property's name.
        /// </summary>
        public const string AttemptsToStorePropertyName = "AttemptsToStore";
        /// <summary>
        /// Sets and gets the AttemptsToStore property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int AttemptsToStore
        {
            get
            {
                return _attemptsToStore;
            }

            set
            {
                if (_attemptsToStore == value)
                {
                    return;
                }

                _attemptsToStore = value;
                RaisePropertyChanged(AttemptsToStorePropertyName);
                RaisePropertyChanged(StoreSuccessPercentagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AttemptsToDelete" /> property's name.
        /// </summary>
        public const string AttemptsToDeletePropertyName = "AttemptsToDelete";
        /// <summary>
        /// Sets and gets the AttemptsToDelete property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int AttemptsToDelete
        {
            get
            {
                return DeleteFailures+DeleteSuccesses;
            }
        }

        /// <summary>
        /// The <see cref="AttemptsToAccess" /> property's name.
        /// </summary>
        public const string AttemptsToAccessPropertyName = "AttemptsToAccess";
        /// <summary>
        /// Sets and gets the AttemptsToAccess property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int AttemptsToAccess
        {
            get
            {
                return _attemptsToAccess;
            }

            set
            {
                if (_attemptsToAccess == value)
                {
                    return;
                }

                _attemptsToAccess = value;
                RaisePropertyChanged(AttemptsToAccessPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AttemptsToRelease" /> property's name.
        /// </summary>
        public const string AttemptsToReleasePropertyName = "AttemptsToRelease";
        /// <summary>
        /// Sets and gets the AttemptsToRelease property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int AttemptsToRelease
        {
            get
            {
                return _attemptsToRelease;
            }

            set
            {
                if (_attemptsToRelease == value)
                {
                    return;
                }

                _attemptsToRelease = value;
                RaisePropertyChanged(AttemptsToReleasePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TotalRandomActions" /> property's name.
        /// </summary>
        public const string TotalRandomActionsPropertyName = "TotalRandomActions";
        /// <summary>
        /// Sets and gets the TotalRandomActions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TotalRandomActions
        {
            get
            {
                return _totalRandomActions;
            }

            set
            {
                if (_totalRandomActions == value)
                {
                    return;
                }

                _totalRandomActions = value;
                RaisePropertyChanged(TotalRandomActionsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StoreSuccesses" /> property's name.
        /// </summary>
        public const string StoreSuccessesPropertyName = "StoreSuccesses";
        /// <summary>
        /// Sets and gets the StoreSuccesses property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int StoreSuccesses
        {
            get
            {
                return _storeSuccesses;
            }

            set
            {
                if (_storeSuccesses == value)
                {
                    return;
                }

                _storeSuccesses = value;
                RaisePropertyChanged(StoreSuccessesPropertyName);
                RaisePropertyChanged(StoreSuccessPercentagePropertyName);
                
            }
        }

        /// <summary>
        /// The <see cref="StoreFailures" /> property's name.
        /// </summary>
        public const string StoreFailuresPropertyName = "StoreFailures";
        /// <summary>
        /// Sets and gets the StoreFailures property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int StoreFailures
        {
            get
            {
                return _storeFailures;
            }

            set
            {
                if (_storeFailures == value)
                {
                    return;
                }

                _storeFailures = value;
                RaisePropertyChanged(StoreFailuresPropertyName);
            }
        }
        #endregion

        /// <summary>
        /// The <see cref="TotalEntriesCreated" /> property's name.
        /// </summary>
        public const string TotalEntriesCreatedPropertyName = "TotalEntriesCreated";
        
        /// <summary>
        /// Sets and gets the TotalEntriesCreated property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TotalEntriesCreated
        {
            get
            {
                return _totalEntriesCreated;
            }

            set
            {
                if (_totalEntriesCreated == value)
                {
                    return;
                }

                _totalEntriesCreated = value;
                RaisePropertyChanged(TotalEntriesCreatedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TotalEntriesDeleted" /> property's name.
        /// </summary>
        public const string TotalEntriesDeletedPropertyName = "TotalEntriesDeleted";

        /// <summary>
        /// Sets and gets the TotalEntriesDeleted property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TotalEntriesDeleted
        {
            get
            {
                return _totalEntriesDeleted;
            }

            set
            {
                if (_totalEntriesDeleted == value)
                {
                    return;
                }

                _totalEntriesDeleted = value;
                RaisePropertyChanged(TotalEntriesDeletedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StoreProbability" /> property's name.
        /// </summary>
        public const string StoreProbabilityPropertyName = "StoreProbability";

        /// <summary>
        /// Sets and gets the StoreProbability property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StoreProbability
        {
            get
            {
                return _StoreProbability;
            }

            set
            {
                if (_StoreProbability == value)
                {
                    return;
                }

                _StoreProbability = value;
                RaisePropertyChanged(StoreProbabilityPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StoreSuccessPercentage" /> property's name.
        /// </summary>
        public const string StoreSuccessPercentagePropertyName = "StoreSuccessPercentage";

        /// <summary>
        /// Sets and gets the StoreSuccessPercentage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StoreSuccessPercentage
        {
            get
            {
                if (StoreSuccesses == 0)
                    return "0.00 %";
                return ((double)StoreSuccesses/AttemptsToStore).ToString("P2");
            }
        }

        /// <summary>
        /// The <see cref="DeleteSuccessPercentage" /> property's name.
        /// </summary>
        public const string DeleteSuccessPercentagePropertyName = "DeleteSuccessPercentage";

        /// <summary>
        /// Sets and gets the DeleteSuccessPercentage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DeleteSuccessPercentage
        {
            get
            {
                if (DeleteSuccesses == 0)
                    return "0.00 %";
                return ((double)DeleteSuccesses/AttemptsToDelete).ToString("P2");
            }
        }

        /// <summary>
        /// The <see cref="DirectoryCount" /> property's name.
        /// </summary>
        public const string DirectoryCountPropertyName = "DirectoryCount";

        /// <summary>
        /// Sets and gets the DirectoryCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DirectoryCount
        {
            get
            {
                return _DirectoryCount;
            }

            set
            {
                if (_DirectoryCount == value)
                {
                    return;
                }

                _DirectoryCount = value;
                RaisePropertyChanged(DirectoryCountPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DeleteFailures" /> property's name.
        /// </summary>
        public const string DeleteFailuresPropertyName = "DeleteFailures";

        
        /// <summary>
        /// Sets and gets the DeleteFailures property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DeleteFailures
        {
            get
            {
                return _deleteFailures;
            }

            set
            {
                if (_deleteFailures == value)
                {
                    return;
                }

                _deleteFailures = value;
                RaisePropertyChanged(DeleteFailuresPropertyName);
                RaisePropertyChanged(AttemptsToDeletePropertyName);
                RaisePropertyChanged(DeleteSuccessPercentagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DeleteSuccesses" /> property's name.
        /// </summary>
        public const string DeleteSuccessesPropertyName = "DeleteSuccesses";

        

        /// <summary>
        /// Sets and gets the DeleteSuccesses property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DeleteSuccesses
        {
            get
            {
                return _deleteSuccesses;
            }

            set
            {
                if (_deleteSuccesses == value)
                {
                    return;
                }

                _deleteSuccesses = value;
                RaisePropertyChanged(DeleteSuccessesPropertyName);
                RaisePropertyChanged(AttemptsToDeletePropertyName);
                RaisePropertyChanged(DeleteSuccessPercentagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AverageBlockLength" /> property's name.
        /// </summary>
        public const string AverageBlockLengthPropertyName = "AverageBlockLength";

        private string _averageBlockLength = "0";

        /// <summary>
        /// Sets and gets the AverageBlockLength property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AverageBlockLength
        {
            get
            {
                return _averageBlockLength;
            }

            set
            {
                if (_averageBlockLength == value)
                {
                    return;
                }

                _averageBlockLength = value;
                RaisePropertyChanged(AverageBlockLengthPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="EmptyBlockCount" /> property's name.
        /// </summary>
        public const string EmptyBlockCountPropertyName = "EmptyBlockCount";

        private int _emptyBlockCount = 0;

        /// <summary>
        /// Sets and gets the EmptyBlockCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int EmptyBlockCount
        {
            get
            {
                return _emptyBlockCount;
            }

            set
            {
                if (_emptyBlockCount == value)
                {
                    return;
                }

                _emptyBlockCount = value;
                RaisePropertyChanged(EmptyBlockCountPropertyName);
                RaisePropertyChanged(CurrentPercentageOccupiedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="OccupiedBlockCount" /> property's name.
        /// </summary>
        public const string OccupiedBlockCountPropertyName = "OccupiedBlockCount";

        private int _occupiedBlockCount = 0;

        /// <summary>
        /// Sets and gets the OccupiedBlockCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int OccupiedBlockCount
        {
            get
            {
                return _occupiedBlockCount;
            }

            set
            {
                if (_occupiedBlockCount == value)
                {
                    return;
                }

                _occupiedBlockCount = value;
                RaisePropertyChanged(OccupiedBlockCountPropertyName);
                RaisePropertyChanged(CurrentPercentageOccupiedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CurrentPercentageOccupied" /> property's name.
        /// </summary>
        public const string CurrentPercentageOccupiedPropertyName = "CurrentPercentageOccupied";


        /// <summary>
        /// Sets and gets the CurrentPercentageOccupied property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CurrentPercentageOccupied
        {
            get
            {
                if (OccupiedBlockCount == 0)
                    return "0.00 %";
                if (EmptyBlockCount == 0)
                    return "100.00 %";
                return ((double)OccupiedBlockCount/Properties.Settings.Default.DiskBlockCount).ToString("P2");
            }
        }

        #region Commands
        public ICommand Reset { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Set all statistics back to their original values
        /// </summary>
        private void ResetStatistics()
        {
            TotalTicks = 0;
            AttemptsToRelease = 0;
            AttemptsToStore = 0;
            AttemptsToAccess = 0;
            TotalRandomActions = 0;
            StoreSuccesses = 0;
            StoreFailures = 0;
            TotalEntriesCreated = 0;
            TotalEntriesDeleted = 0;
            DirectoryCount = "0";
            DeleteFailures = 0;
            DeleteSuccesses = 0;
            AverageBlockLength = "0";
            _fileList.Clear();
            EmptyBlockCount = 0;
            OccupiedBlockCount = 0;
        }

        //SetStatistics where necessary
        private void SetStatistics() 
        {
            StoreProbability = CalculateStoreProbability();
        }

        private string CalculateStoreProbability()
        {
            double pass = new double();
            int level = Properties.Settings.Default.DiskStoreFrequencyLevel;
            if (level == 0)
            {
                pass = 0;
            }
            else
            {
                pass=(double)level/(level+1);
            }
            return pass.ToString("P2");
        }

        /// <summary>
        /// Consume Messages
        /// </summary>
        /// <param name="obj"></param>
        private void ConsumeMessage(SimpleMessage message)
        {
            switch (message.Type) 
            {
                case SimpleMessage.MessageType.SimulationStart:
                    SetStatistics();
                    break;
                case SimpleMessage.MessageType.SimulationStop:
                    break;
                case SimpleMessage.MessageType.SimulationReset:
                    ResetStatistics();
                    break;
                case SimpleMessage.MessageType.SimulationTick:
                    OnTick();
                    break;
                case SimpleMessage.MessageType.SettingsChanged:
                    SetStatistics();
                    break;
                case SimpleMessage.MessageType.DirectoryCreated:
                    TotalEntriesCreated++;
                    DirectoryCount=message.Message;
                    break;
                case SimpleMessage.MessageType.DirectoryDeleted:
                    TotalEntriesDeleted++;
                    DirectoryCount=message.Message;
                    break;
                case SimpleMessage.MessageType.SendEmptyBlockCount:
                    EmptyBlockCount = message.PassedInt;
                    break;
                case SimpleMessage.MessageType.SendOccupiedBlockCount:
                    OccupiedBlockCount = message.PassedInt;
                    break;
                default:
                    //Do Nothing
                    break;
            }
        }

        private void ConsumeActionMessage(ActionMessage message)
        {
            switch (message.Type)
            {
                case Messaging.ActionMessage.MessageType.AttemptToStore:
                    OnAttemptToStore(message.PassedFile);
                    break;
                case Messaging.ActionMessage.MessageType.AttemptToAccess:
                    OnAttemptToAccess();
                    break;
                case Messaging.ActionMessage.MessageType.AttemptToRelease:
                    OnAttemptToRelease();
                    break;
                case Messaging.ActionMessage.MessageType.StoreSuccess:
                    OnStoreSuccess();
                    break;
                case Messaging.ActionMessage.MessageType.StoreFail:
                    OnStoreFailure();
                    break;
                case Messaging.ActionMessage.MessageType.DeleteFail:
                    DeleteFailures++;
                    TotalRandomActions++;
                    break;
                case Messaging.ActionMessage.MessageType.DeleteSuccess:
                    DeleteSuccesses++;
                    TotalRandomActions++;
                    break;
                default:
                    //Do Nothing
                    break;
            }
        }

        private void OnAttemptToRelease()
        {
            AttemptsToRelease++;
            TotalRandomActions++;
        }

        private void OnAttemptToAccess()
        {
            AttemptsToAccess++;
            TotalRandomActions++;
        }


        private void OnAttemptToStore(File file)
        {
            _fileList.Add(file.BlockLength);
            int total = 0;
            foreach (int i in _fileList)
            {
                total += i;
            }
            AverageBlockLength = ((double)total / _fileList.Count).ToString("F2");
            AttemptsToStore++;
            TotalRandomActions++;
        }

        private void OnStoreSuccess() 
        {
            StoreSuccesses++;
        }

        private void OnStoreFailure()
        {
            StoreFailures++;
        }

        private void OnTick()
        {
            TotalTicks++;
        }

        #endregion
    }
}