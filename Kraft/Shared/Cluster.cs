using System.Collections;
using System.Collections.Generic;

namespace Kraft.Shared
{
    public class Cluster
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Brokers { get; set; }
        public string ZkServers { get; set; }
    }

    public class KafkaCluster : Cluster
    {
        public List<Topic> Topics { get; set; }
        public List<ConsumerGroup> Groups { get; set; }
    }

    public class Partition
    {
        public int Id { get; set; }
        public long Earliest { get; set; }
        public long Latest { get; set; }
    }

    public class Topic
    {
        public string Name { get; set; }
        public List<Partition> Partitions { get; set; }
    }

    public class ConsumerGroup
    {
        public string GroupId { get; set; }
        public List<Topic> Topics { get; set; }
    }
}