using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BTT.TaskScheduler;

namespace DOWTank.BackgroundService
{
    public class DowTankBackgroundProcessor
    {
        BTT.TaskScheduler.TaskScheduler _TaskScheduler;

        public DowTankBackgroundProcessor(string serviceName)
        {
            string scheduleFilePath = GetScheduleFileName();
            XMLScheduleSource schedule = new XMLScheduleSource(scheduleFilePath);
            _TaskScheduler = new BTT.TaskScheduler.TaskScheduler("BTT Dow Tank Backgrround Processor", schedule);
        }

        public void Start()
        {
            _TaskScheduler.Start();
        }

        public void Stop()
        {
            _TaskScheduler.Stop();
        }

        private string GetScheduleFileName()
        {
            string fullPath = Properties.Settings.Default.ScheduleFileName;

            string fileName = System.IO.Path.GetFileName(fullPath);
            string fileFolder = System.IO.Path.GetDirectoryName(fullPath);

            Exception loadException = null;

            try
            {
                //if we have a full path and the file exists, use it.
                System.IO.FileInfo fi = new System.IO.FileInfo(fullPath);
                if (fi.Exists)
                {
                    return fullPath;
                }
            }
            catch (Exception ex)
            {
                loadException = ex;
            }

            try
            {
                //unable to load the file by path, so we will look for a file by that name in the local directory
                //string filePath = string.Format("{0}\\{1}", this.GetInstallDirectory(), fileName);

                string new_folder = this.GetInstallDirectory();

                fullPath = System.IO.Path.Combine(new_folder, fileName);

                System.IO.FileInfo fi = new System.IO.FileInfo(fullPath);
                if (fi.Exists)
                {
                    return fullPath;
                }
            }
            catch(Exception ex)
            {
                loadException = ex;
            }
            throw new Exception(string.Format("Unable to load schedule file: {0}", fileName), loadException);
        }

        private string GetInstallDirectory()
        {
            string exeName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            System.IO.FileInfo fi = new System.IO.FileInfo(System.Reflection.Assembly.GetEntryAssembly().CodeBase.Substring(8));
            return fi.DirectoryName;
        }
    }
}
