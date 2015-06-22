using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.BackgroundService
{
    public partial class Service1 : ServiceBase
    {
        DOWTank.BackgroundService.DowTankBackgroundProcessor _processor = new DowTankBackgroundProcessor("DOW Tank Processor");

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _processor.Start();
        }

        protected override void OnStop()
        {
            _processor.Stop();
        }
    }
}
