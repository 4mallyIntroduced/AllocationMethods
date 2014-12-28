namespace AllocationMethods.Messaging
{
    /// <summary>
    /// A simple message class that we use to pass messages around the application
    /// via the Messenger singleton, but keep the parts sufficiently decoupled
    /// for the MVVM style of work.
    /// </summary>
    public class SimpleMessage
    {
        public SimpleMessage(MessageType type)
            : this(type, string.Empty)
        {
        }

        public SimpleMessage(MessageType type, string message)
        {
            Type = type;
            Message = message;
        }

        public SimpleMessage(MessageType type, int passedInt)
        {
            Type = type;
            PassedInt = passedInt;
        }

        public enum MessageType
        {
            SettingsChanged,
            SimulationStop,
            SimulationStart,
            SimulationTick,
            SimulationReset,
            StatisticDiskAddHit,
            StatisticsDiskAddMiss,
            StatisticsRandomActionCount,
            DirectoryCreated,
            DirectoryDeleted,
            SendOccupiedBlockCount,
            SendEmptyBlockCount

        }

        public MessageType Type { get; set; }

        public string Message { get; set; }

        public int PassedInt { get; set; }

    }
}
