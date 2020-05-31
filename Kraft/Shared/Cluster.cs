using System;
using System.Collections.Generic;
using System.Text;

namespace Kraft.Shared
{
    public class Cluster
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brokers { get; set; }
        public string ZkServers { get; set; }
        public string Description { get; set; }
    }
}
