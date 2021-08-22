using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortLaMontagne.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
    }
}
