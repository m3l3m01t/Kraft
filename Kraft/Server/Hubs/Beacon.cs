using System;

namespace Kraft.Server.Hubs
{
    public class BeatEventArgs : EventArgs
    {
        public DateTime? TimeStamp { get; set; }
        public int Strength { get; set; }

        public long Beats { get; set; }
    }
    public class Beacon
    {
        public event EventHandler<BeatEventArgs> HeatBeat;
    }
}