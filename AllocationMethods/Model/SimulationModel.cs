using AllocationMethods.Messaging;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace AllocationMethods.Model
{
    internal class SimulationModel : ISimulationModel
    {
        #region Fields
        /// <summary>
        /// The underlying timer
        /// </summary>
        readonly DispatcherTimer timer = new DispatcherTimer();
        private List<File> _files = new List<File>();
        private TimeSpan _currentTime;
        private int frequencyCounter = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a timer of the specified duration.
        /// </summary>
        /// <param name="duration"></param>
        public SimulationModel()
        {
            //  use a coarse grained interval, as this timer isn't meant
            //  to be completely accurate.
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += (sender, e) => OnDispatcherTimerTick();
            Reset();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Return the timer interval that we're using.
        /// </summary>
        public TimeSpan Interval { get { return timer.Interval; } }

        /// <summary>
        /// Time complete 
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set {
                if (_currentTime != value)
                    _currentTime = value; 
            }
        }

        public SimulationState Status { get; set; }
        #endregion

        #region Methods
        
        /// <summary>
        /// Start the countdown.
        /// </summary>
        public void Start()
        {
            timer.Start();
            OnStarted();
        }

        /// <summary>
        /// Stop the countdown.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            OnStopped();
        }

        /// <summary>
        /// Stop the current countdown and reset.
        /// </summary>
        public void Reset()
        {
            Stop();
            CurrentTime = TimeSpan.Zero;
            OnReset();
        }

        private void DoRandomActivity()
        {
            Random r = new Random();

            switch (r.Next(Properties.Settings.Default.DiskStoreFrequencyLevel + 1))
            {
                
                case 0:
                    if (!IsListEmpty())
                        AttemptToDeleteFile();
                    else
                        Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.DeleteFail));
                    break;
                //Used to get 50% to 90% Random on Create File
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    CreateFile();
                    break;
                case 10:
                    if (!IsListEmpty())
                        AttemptToAccessFile();
                    break;
                case 11:
                    if (!IsListEmpty())
                        AttemptToReleaseFile();
                    break;
                case 12:
                    //Do Nothing
                    break;
            }

        }

        /// <summary>
        /// Creates Random File
        /// </summary>
        public void CreateFile()
        {
            Random r = new Random();
            //File file = new File(r.Next(int.MaxValue).ToString(), r.Next(Properties.Settings.Default.MinimumFileSize, Properties.Settings.Default.MaximumFileSize));
            File file = new File(GetRandomHexNumber(), r.Next(Properties.Settings.Default.MinimumFileSize, Properties.Settings.Default.MaximumFileSize));
            _files.Add(file);
            AttemptToStoreFile(file);
        }

        public string GetRandomHexNumber()
        {
            var random = new Random();
            string color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }

        /// <summary>
        /// Tries to store File on Disk, sends message to disk to store
        /// </summary>
        public void AttemptToStoreFile(File file) 
        {
            if (AttemptToStore != null)
            {
                AttemptToStore(this, new SimulationModelEventArgs(CurrentTime, SimulationState.Running, file));
            }
        }

        public void OnStoreFail(File file)
        {
            if (_files.Contains(file))
                _files.Remove(file);
        }

        public void OnDeleteSuccess(File file) 
        {
            if (_files.Contains(file))
                _files.Remove(file);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        public void AttemptToDeleteFile()
        {
            Random r = new Random();
            int index = r.Next(_files.Count);
            if (_files[index].Name == "")
            {
                Console.WriteLine("test");
            }

            if (AttemptToDelete != null)
            {
                AttemptToDelete(this, new SimulationModelEventArgs(CurrentTime, SimulationState.Running, _files[index]));
            }
        }

        public void DeleteFile(File file) 
        {
            _files.Remove(file);
        }

        /// <summary>
        /// Access file, sends message to directory to find file to access
        /// </summary>
        public void AttemptToAccessFile() { }

        /// <summary>
        /// Release file, sends message to directory to find file to access
        /// </summary>
        public void AttemptToReleaseFile() { }

        public bool IsListEmpty() 
        {
            if (_files.Count > 0)
                return false;
            return true;
        }

        #region Event Handlers
        /// <summary>
        /// Handle the ticking of the system timer.
        /// </summary>
        private void OnDispatcherTimerTick()
        {
            CurrentTime = CurrentTime + Interval;
            OnTick();
        }
        #endregion
        #endregion

        #region Events

        public event EventHandler<SimulationModelEventArgs> Tick;
        public event EventHandler<SimulationModelEventArgs> Started;
        public event EventHandler<SimulationModelEventArgs> Stopped;
        public event EventHandler<SimulationModelEventArgs> TimerReset;

        //TODO 
        // Possibly make new EventArgs
        public event EventHandler<SimulationModelEventArgs> AttemptToStore;
        public event EventHandler<SimulationModelEventArgs> AttemptToDelete;
        public event EventHandler<SimulationModelEventArgs> AttemptToAccess;
        public event EventHandler<SimulationModelEventArgs> AttemptToRelease;

        #region OnReset
        /// <summary>
        /// Triggers the TimerReset event
        /// </summary>
        private void OnReset()
        {
            Status = SimulationState.Paused;
            _files.Clear(); //Clear Files
            //Reset frequency counter to start fresh
            frequencyCounter = 0;

            if (TimerReset != null)
            {
                TimerReset(this, new SimulationModelEventArgs(CurrentTime, SimulationState.Ready));
            }
        }
        #endregion

        #region OnStopped
        /// <summary>
        /// Triggers the Stopped event.
        /// </summary>
        private void OnStopped()
        {
            Status = SimulationState.Paused;

            if (Stopped != null)
            {
                Stopped(this, new SimulationModelEventArgs(CurrentTime, SimulationState.Paused));
            }
        }
        #endregion

        #region OnStarted
        /// <summary>
        /// Triggers the Started event.
        /// </summary>
        private void OnStarted()
        {
            Status = SimulationState.Running;

            if (Started != null)
            {
                Started(this, new SimulationModelEventArgs(CurrentTime, SimulationState.Running));
            }
        }
        #endregion

        #region OnTick
        /// <summary>
        /// Triggers the Tick event.
        /// </summary>
        private void OnTick()
        {
            Status = SimulationState.Running;

            if (Tick != null)
            {
                Tick(this, new SimulationModelEventArgs(CurrentTime, SimulationState.Running));
            }
            //THINKME Correct Frequency counter, Have to slow down speed for visibility reasons
            if(frequencyCounter == Properties.Settings.Default.RandomActionAfterBlankTicks)
            {
                DoRandomActivity();
                frequencyCounter = 0;
            }
            else
                frequencyCounter++;
        }
        #endregion

        #endregion
    }
}
