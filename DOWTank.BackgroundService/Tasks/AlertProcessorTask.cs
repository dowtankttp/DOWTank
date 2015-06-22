using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.SqlClient;

using BTT.TaskScheduler;

namespace DowTank.BackgroundService.Tasks
{
    class AlertProcessorTask : ScheduledTask
    {
        protected override void Execute ()
        {
            throw new NotImplementedException();
        }

        public void ExecuteInternal()
        {
            //using (var sqlConn = new SqlConnection(DOWTank.BackgroundService.Properties.Settings.Default.SkybitzFIleLocation))
            //{
            //    sqlConn.Open();
               
            //    using (var insertCmd = new SqlCommand("INSERT INTO TANK1TS_SkybitzData VALUES(@EquipmentNumber, @TimeOfObservation, @State, @ClosestLandmark, @DistanceFromLandmark)", sqlConn))
            //    {
            //        foreach (var record in sourceRecords)
            //        {
            //            insertCmd.Parameters.Clear();
            //            insertCmd.Parameters.AddWithValue("@EquipmentNumber", record.EquipmentNumber);
            //            insertCmd.Parameters.AddWithValue("@TimeOfObservation", record.TimeOfObservation);
            //            insertCmd.Parameters.AddWithValue("@State", record.State);
            //            insertCmd.Parameters.AddWithValue("@ClosestLandmark", record.ClosestLandmark);
            //            insertCmd.Parameters.AddWithValue("@DistanceFromLandmark", record.DistanceFromLandmark);
            //            insertCmd.ExecuteNonQuery();
            //        }
            //    }
            //}
        }
    }
}
