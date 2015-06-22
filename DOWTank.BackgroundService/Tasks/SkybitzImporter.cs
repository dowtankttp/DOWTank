using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.SqlClient;

using DOWTank.BackgroundService;
using BTT.TaskScheduler;

namespace DowTank.BackgroundService.Tasks
{
    public class SkybitzImporter : ScheduledTask
    {
        protected override void ExecuteInternal()
        {
            List<SkybitzRecord> sourceRecords = new List<SkybitzRecord>();
            try
            {
            //connect to skybitz import file
            using (var xlConn = new OdbcConnection(string.Format("DataSource={0};Driver=Microsoft Excel Driver", DOWTank.BackgroundService.Properties.Settings.Default.SkybitzFIleLocation)))
            {
                using(var cmd = new OdbcCommand("SELECT * FROM [LAABSearch(1)]", xlConn))
                {
                    xlConn.Open();
                    var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    {
                        while(dr.Read())
                        {
                            sourceRecords.Add(new SkybitzRecord()
                            {
                                EquipmentNumber = (string)dr["Asset Id"],
                                TimeOfObservation = (DateTime)dr["Time Of Observation (UTC)"],
                                ClosestLandmark = (string)dr["Closest Landmark"],
                                State = (string)dr["State"],
                                DistanceFromLandmark = (string)dr["Distance from Landmark "]

                            });
                        }
                    }
                }
            }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.TraceError(string.Format("Failure loading Skybitz file: {0}  \r\n\r\nStack: {1}", ex, ex.StackTrace));
            }
            System.Diagnostics.Trace.TraceInformation(string.Format("Found  {0}  skybitz records",  sourceRecords.Count));

            if (sourceRecords.Count == 0)
                return;

            //clear skybitz table
            using (var sqlConn = new SqlConnection(DOWTank.BackgroundService.Properties.Settings.Default.SkybitzFIleLocation))
            {
                sqlConn.Open();
                using (var deleteCmd = new SqlCommand("TRUNCATE TABLE  TANK1TS_SkybitzData", sqlConn))
                {
                    var rows = deleteCmd.ExecuteNonQuery();
                    System.Diagnostics.Trace.TraceInformation(string.Format("Deleted {0} TANK Skybitz records", rows));
                }
                using (var insertCmd = new SqlCommand("INSERT INTO TANK1TS_SkybitzData VALUES(@EquipmentNumber, @TimeOfObservation, @State, @ClosestLandmark, @DistanceFromLandmark)", sqlConn))
                {
                    foreach(var record in sourceRecords)
                    {
                        insertCmd.Parameters.Clear();
                        insertCmd.Parameters.AddWithValue("@EquipmentNumber", record.EquipmentNumber);
                        insertCmd.Parameters.AddWithValue("@TimeOfObservation", record.TimeOfObservation);
                        insertCmd.Parameters.AddWithValue("@State", record.State);
                        insertCmd.Parameters.AddWithValue("@ClosestLandmark", record.ClosestLandmark);
                        insertCmd.Parameters.AddWithValue("@DistanceFromLandmark", record.DistanceFromLandmark);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Execute()
        {
            this.ExecuteInternal();
        }
    }
}
