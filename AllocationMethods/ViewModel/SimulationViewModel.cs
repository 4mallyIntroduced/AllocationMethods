using AllocationMethods.Messaging;
using AllocationMethods.Model;
using AllocationMethods.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;
using System.Windows.Shell;

// MM: Added
using System.Threading;
using System.Collections.Generic;

namespace AllocationMethods.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public sealed class SimulationViewModel : ViewModelBase
    {
        #region Members
        readonly ISimulationModel _simulation = SimulationModelBuilder.GetNewTimer();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the SimulationControlViewModel class.
        /// </summary>
        public SimulationViewModel()
        {
            
            //  add event handlers
            AddEventHandlers();

            //  bind the commands to their respective actions
            BindCommands();

            //  Register against the Messenger singleton to receive any simple
            //  messages.  Specifically the one that says settings have changed.
            Messenger.Default.Register<SimpleMessage>(this, ConsumeMessage);
            Messenger.Default.Register<ActionMessage>(this, ConsumeActionMessage);
        }



        #endregion

        #region Commands

        public ICommand Start { get; private set; }
        public ICommand Pause { get; private set; }
        public ICommand Reset { get; private set; }

        #region Start Command

        /// <summary>
        /// Start the underlying timer
        /// </summary>
        void StartTimerExecute()
        {
            _simulation.Start();
        }

        /// <summary>
        /// can we start the underlying timer?
        /// </summary>
        /// <returns></returns>
        bool CanStartTimerExecute()
        {
            return _simulation.Status != SimulationState.Running;
        }

        #endregion

        #region Pause Command

        /// <summary>
        /// Stop the underlying timer.
        /// </summary>
        void StopTimerExecute()
        {
            _simulation.Stop();
        }

        /// <summary>
        /// Can the timer be stopped?
        /// </summary>
        /// <returns></returns>
        bool CanStopTimerExecute()
        {
            return _simulation.Status == SimulationState.Running;
        }


        #endregion

        #region Reset Command
        /// <summary>
        /// Reset the timer and update corresponding values.
        /// </summary>
        void ResetTimerExecute()
        {
            _simulation.Reset();
            UpdateTimerValues();
        }

        private void BindCommands()
        {
            Start = new RelayCommand(() => StartTimerExecute(), CanStartTimerExecute);
            Pause = new RelayCommand(() => StopTimerExecute());
            Reset = new RelayCommand(() => ResetTimerExecute());
        }
        #endregion

        #endregion

        #region Properties

        #region TimerValue
        /// <summary>
        /// The <see cref="TimerValue" /> property's name.
        /// </summary>
        public const string TimerValuePropertyName = "TimerValue";

        private string _timerValue = "00:00:00";

        /// <summary>
        /// Sets and gets the TimerValue property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TimerValue
        {
            get
            {
                return _timerValue;
            }

            set
            {
                if (_timerValue == value)
                {
                    return;
                }

                _timerValue = value;
                RaisePropertyChanged(TimerValuePropertyName);
            }
        }
        #endregion

        #region CurrentTime
        /// <summary>
        /// The <see cref="CurrentTime" /> property's name.
        /// </summary>
        public const string CurrentTimePropertyName = "CurrentTime";

        private TimeSpan _currentTime = TimeSpan.Zero;

        /// <summary>
        /// Sets and gets the CurrentTime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public TimeSpan CurrentTime
        {
            get
            {
                return _currentTime;
            }

            set
            {
                if (_currentTime == value)
                {
                    return;
                }

                _currentTime = value;
                RaisePropertyChanged(CurrentTimePropertyName);
            }
        }
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Consume any messages that are passed between models
        /// </summary>
        /// <param name="message"></param>
        void ConsumeMessage(SimpleMessage message)
        {
            switch (message.Type)
            {
                case SimpleMessage.MessageType.SettingsChanged:
                    StopTimerExecute();
                    break;
                case SimpleMessage.MessageType.SimulationStop:
                    //StopTimerExecute();
                    break;
                case SimpleMessage.MessageType.SimulationStart:
                    //StartTimerExecute();
                    break;
                default:
                    //This is an error
                    break;
            }
        }

        private void ConsumeActionMessage(ActionMessage message)
        {
            switch (message.Type)
            {
                case ActionMessage.MessageType.StoreFail:
                    _simulation.OnStoreFail(message.PassedFile);
                    break;
                case ActionMessage.MessageType.DeleteSuccess:
                    _simulation.OnDeleteSuccess(message.PassedFile);
                    break;
                default:
                    //This is an error
                    break;
            }
        }

        /// <summary>
        /// Update the timer view model properties based on the time span passed in.
        /// </summary>
        /// <param name="t"></param>
        private void UpdateTimer(SimulationModelEventArgs e)
        {
            UpdateTimerValues();
        }

        /// <summary>
        /// Update the timer view model properties based on the time span passed in.
        /// </summary>
        /// <param name="t"></param>
        private void UpdateTimerValues()
        {
            TimeSpan t = _simulation.CurrentTime;
            TimerValue = string.Format("{0}:{1}:{2}", t.Hours.ToString("D2"),
                t.Minutes.ToString("D2"), t.Seconds.ToString("D2"));
        }

        /// <summary>
        /// Add the event handlers.
        /// </summary>
        private void AddEventHandlers()
        {
            _simulation.Tick += (sender, e) => OnTick(sender, e);
            _simulation.Started += (sender, e) => OnStarted(sender, e);
            _simulation.Stopped += (sender, e) => OnStopped(sender, e);
            _simulation.TimerReset += (sender, e) => OnReset(sender, e);
            _simulation.AttemptToStore += (sender, e) => OnAttemptToStore(sender,e);
            _simulation.AttemptToDelete += (sender, e) => OnAttemptToDelete(sender, e);
            _simulation.AttemptToAccess += (sender, e) => OnAttemptToAccess(sender, e);
            _simulation.AttemptToRelease += (sender, e) => OnAttemptToRelease(sender, e);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Fires when the timer ticks.  Ticks out to be of the order of 
        /// tenths of a second or so to prevent excessive spamming of this method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTick(object sender, SimulationModelEventArgs e)
        {
            UpdateTimer(e);
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SimulationTick, TimerValue));
        }

        /// <summary>
        /// Fires when the timer starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnStarted(object sender, SimulationModelEventArgs e)
        {
            UpdateTimer(e);
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SimulationStart));

        }

        /// <summary>
        /// Fires when the timer stops.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStopped(object sender, SimulationModelEventArgs e)
        {
            UpdateTimer(e);
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SimulationStop));
        }

        /// <summary>
        /// Fires when the timer resets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReset(object sender, SimulationModelEventArgs e)
        {
            var timer = sender as SimulationModel;
            UpdateTimer(e);
            Messenger.Default.Send(new SimpleMessage(SimpleMessage.MessageType.SimulationReset));
        }


        private void OnAttemptToStore(object sender, SimulationModelEventArgs e)
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.AttemptToStore, e.File));
        }

        private void OnAttemptToDelete(object sender, SimulationModelEventArgs e)
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.AttemptToDelete, e.File.Name));
        }

        private void OnAttemptToAccess(object sender, SimulationModelEventArgs e)
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.AttemptToAccess, e.File.Name));
        }

        private void OnAttemptToRelease(object sender, SimulationModelEventArgs e)
        {
            Messenger.Default.Send(new ActionMessage(ActionMessage.MessageType.AttemptToRelease, e.File.Name));
        }

        #endregion

    }
}