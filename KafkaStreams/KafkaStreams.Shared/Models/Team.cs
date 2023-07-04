using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KafkaStreams.Shared.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
