using System;

namespace Kraft.Shared
{
    public class Beat
    {
        public DateTime? TimeStamp { get; set; }
        public int Strength { get; set; }

        public long Beats { get; set; }

        public string Payload {get;set;}
    }
}