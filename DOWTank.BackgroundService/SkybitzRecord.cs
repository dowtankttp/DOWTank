using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.BackgroundService
{
    public class SkybitzRecord
    {
        public string EquipmentNumber { get; set; }
        public DateTime? TimeOfObservation { get; set; }
        public string State { get; set; }
        public string ClosestLandmark { get; set; }
        public string DistanceFromLandmark { get; set; }
    }
}
