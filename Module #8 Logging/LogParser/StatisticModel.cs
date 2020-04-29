namespace LogParser
{
    public class StatisticModel
    {
        public int ErrorCount { get; set; }

        public int InfoCount { get; set; }

        public int DebugCount { get; set; }

        public string[] Errors { get; set; }
    }
}
