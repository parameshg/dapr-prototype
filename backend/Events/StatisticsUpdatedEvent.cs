namespace Backend.Events
{
    public class StatisticsUpdatedEvent
    {
        public double Mean { get; set; }

        public double Variance { get; set; }

        public double StandardDeviation { get; set; }
    }
}