using System;

namespace AllocationMethods.Model
{
    /// <summary>
    /// Simple EventArgs class for the timer model.
    /// </summary>
    public class SimulationModelEventArgs : EventArgs
    {
        private TimeSpan _currentTime;
        private File _file;
        private SimulationState state = SimulationState.Ready;

        public TimeSpan CurrentTime
        {
            get
            {
                return _currentTime;
            }
            set
            {
                if (_currentTime == value)
                    return;
                _currentTime = value;
            }
        }

        public SimulationState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public File File 
        {
            get 
            {
                return _file;
            }
            set
            {
                _file = value;
            }
        }

        public SimulationModelEventArgs(TimeSpan currentTime, SimulationState state)
        {
            CurrentTime = currentTime;
            State = state;
        }
        public SimulationModelEventArgs(TimeSpan currentTime, SimulationState state, File file)
        {
            CurrentTime = currentTime;
            State = state;
            File = file;
        }

    }
}
