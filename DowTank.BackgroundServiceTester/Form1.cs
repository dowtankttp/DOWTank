using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DOWTank.BackgroundService;

namespace DowTank.BackgroundServiceTester
{
    public partial class Form1 : Form
    {
        DowTankBackgroundProcessor _processor = new DowTankBackgroundProcessor("DOW Tank Processor");
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _processor.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _processor.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DowTank.BackgroundService.Tasks.SkybitzImporter task = new BackgroundService.Tasks.SkybitzImporter();
            task.Execute();
        }
    }
}
