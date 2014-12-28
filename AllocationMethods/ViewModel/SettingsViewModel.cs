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
    public sealed class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private bool _isEnabled = true;
        private int _diskBlockCount = Properties.Settings.Default.DiskBlockCount;
        private AllocationType _allocationMethod = Properties.Settings.Default.AllocationType;
        private int _randomActionAfterBlankTicks = Properties.Settings.Default.RandomActionAfterBlankTicks;
        private int _maximumFileSize = Properties.Settings.Default.MaximumFileSize;
        private int _minimumFileSize = Properties.Settings.Default.MinimumFileSize;
        private string _saveText = "";
        private int _storeFrequencyLevel = Properties.Settings.Default.DiskStoreFrequencyLevel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SimulationSettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            //Bind Commands
            Save = new RelayCommand(() => SaveSettings());
            Reset = new RelayCommand(() => ResetSettings());

            //Reset Property Settings
            ResetSettings();

            //  Register against the Messenger singleton to receive any simple
            //  messages.  Specifically the one that says settings have changed.
            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);  
        }


        #endregion

        #region Properties
        /// <summary>
        /// The <see cref="IsEnabledperty's name.
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
        /// The <see cref="AllocationMethod" /> property's name.
        /// </summary>
        public const string AllocationMethodPropertyName = "AllocationMethod";
        /// <summary>
        /// Sets and gets the AllocationMethod property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AllocationType AllocationMethod
        {
            get
            {
                return _allocationMethod;
            }

            set
            {
                if (_allocationMethod == value)
                {
                    return;
                }

                _allocationMethod = value;
                RaisePropertyChanged(AllocationMethodPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DiskBlockCount" /> property's name.
        /// </summary>
        public const string DiskBlockCountPropertyName = "DiskBlockCount";
        /// <summary>
        /// Sets and gets the DiskBlockCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DiskBlockCount
        {
            get
            {
                return _diskBlockCount;
            }

            set
            {
                if (_diskBlockCount == value)
                {
                    return;
                }

                _diskBlockCount = value;
                RaisePropertyChanged(DiskBlockCountPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="RandomActionAfterBlankTicks" /> property's name.
        /// </summary>
        public const string RandomActionAfterBlankTicksPropertyName = "RandomActionAfterBlankTicks";
        /// <summary>
        /// Sets and gets the RandomActionFrequency property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int RandomActionAfterBlankTicks
        {
            get
            {
                return _randomActionAfterBlankTicks;
            }

            set
            {
                if (_randomActionAfterBlankTicks == value)
                {
                    return;
                }

                _randomActionAfterBlankTicks = value;
                RaisePropertyChanged(RandomActionAfterBlankTicksPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaximumFileSize" /> property's name.
        /// </summary>
        public const string MaximumFileSizePropertyName = "MaximumFileSize";
        /// <summary>
        /// Sets and gets the MaximumFileSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaximumFileSize
        {
            get
            {
                return _maximumFileSize;
            }

            set
            {
                if (_maximumFileSize == value)
                {
                    return;
                }

                _maximumFileSize = value;
                RaisePropertyChanged(MaximumFileSizePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MinimumFileSize" /> property's name.
        /// </summary>
        public const string MinimumFileSizePropertyName = "MinimumFileSize";
        /// <summary>
        /// Sets and gets the MinimumFileSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MinimumFileSize
        {
            get
            {
                return _minimumFileSize;
            }

            set
            {
                if (_minimumFileSize == value)
                {
                    return;
                }

                _minimumFileSize = value;
                RaisePropertyChanged(MinimumFileSizePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SaveText" /> property's name.
        /// </summary>
        public const string SaveTextPropertyName = "SaveText";

        /// <summary>
        /// Sets and gets the SaveText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SaveText
        {
            get
            {
                return _saveText;
            }

            set
            {
                if (_saveText == value)
                {
                    return;
                }

                _saveText = value;
                RaisePropertyChanged(SaveTextPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="StoreFrequencyLevel" /> property's name.
        /// </summary>
        public const string StoreFrequencyLevelPropertyName = "StoreFrequencyLevel";

        /// <summary>
        /// Sets and gets the StoreFrequencyLevel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int StoreFrequencyLevel
        {
            get
            {
                return _storeFrequencyLevel;
            }

            set
            {
                if (_storeFrequencyLevel == value)
                {
                    return;
                }

                _storeFrequencyLevel = value;
                RaisePropertyChanged(StoreFrequencyLevelPropertyName);
            }
        }


        #endregion

        #region Commands

        public ICommand Save { get; private set; }
        public ICommand Reset { get; private set; }

        #endregion

        #region Methods
        private void ConsumeMessage(SimpleMessage message)
        {
            switch(message.Type)
            {
                case Messaging.SimpleMessage.MessageType.SimulationStart:
                    ResetSettings(); //Reset settings to saved settings on start
                    IsEnabled = false; //Disable settings
                    break;
                case Messaging.SimpleMessage.MessageType.SimulationReset:
                    ResetSettings();
                    IsEnabled = true;
                    break;
                default:
                    //Do Nothing
                    break;
            }
        }

        /// <summary>
        /// Saves any unsaved settings
        /// </summary>
        private void SaveSettings()
        {
            SaveText = "Settings Saved";

            //Add any properties to properly save
            Properties.Settings.Default.AllocationType = AllocationMethod;
            Properties.Settings.Default.DiskBlockCount = DiskBlockCount;
            Properties.Settings.Default.RandomActionAfterBlankTicks = RandomActionAfterBlankTicks;
            Properties.Settings.Default.MinimumFileSize = MinimumFileSize;
            Properties.Settings.Default.MaximumFileSize = MaximumFileSize;
            Properties.Settings.Default.DiskStoreFrequencyLevel = StoreFrequencyLevel;
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SettingsChanged));
            
        }

        /// <summary>
        /// Resets any displayed settings to the saved properties
        /// </summary>
        private void ResetSettings()
        {
            SaveText = ""; //Reset Save Text
            //Add any properties to properly reset
            AllocationMethod = Properties.Settings.Default.AllocationType;
            DiskBlockCount = Properties.Settings.Default.DiskBlockCount;
            MinimumFileSize = Properties.Settings.Default.MinimumFileSize;
            MaximumFileSize = Properties.Settings.Default.MaximumFileSize;
            RandomActionAfterBlankTicks = Properties.Settings.Default.RandomActionAfterBlankTicks;
            StoreFrequencyLevel = Properties.Settings.Default.DiskStoreFrequencyLevel;
        }
        #endregion
    }
}