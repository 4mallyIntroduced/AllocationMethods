using System;

namespace AllocationMethods.Model
{
    public interface ISimulationModel
    {
        /// <summary>
        /// Return the timer interval that we're using.
        /// </summary>
        TimeSpan Interval { get; }

        /// <summary>
        /// Set or get the time completed so far
        /// </summary>
        TimeSpan CurrentTime { get; set; }

        /// <summary>
        /// The state of the model
        /// </summary>
        SimulationState Status { get; set; }

        /// <summary>
        /// Start the countdown.
        /// </summary>
        void Start();

        /// <summary>
        /// When Stored file fails
        /// </summary>
        void OnStoreFail(File file);

        /// <summary>
        /// Stop the countdown.
        /// </summary>
        void Stop();

        /// <summary>
        /// Stop the current countdown and reset.
        /// </summary>
        void Reset();

        /// <summary>
        /// Tick event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> Tick;

        /// <summary>
        /// Timer started event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> Started;

        /// <summary>
        /// Timer stopped event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> Stopped;

        /// <summary>
        /// Timer reset event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> TimerReset;

        /// <summary>
        /// File Store attempt event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> AttemptToStore;

        /// <summary>
        /// File Delete attempt event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> AttemptToDelete;

        /// <summary>
        /// File Access attmpt event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> AttemptToAccess;

        /// <summary>
        /// File Release attempt event
        /// </summary>
        event EventHandler<SimulationModelEventArgs> AttemptToRelease;


        void OnDeleteSuccess(File file);
    }
}
