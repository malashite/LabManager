using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabManager
{
    public partial class Form1 : Form
    {
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
            ReadExceptionsFile();
            UpdateLogBox();

            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Текущие дата и время: ";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            infoLabel.ForeColor = Color.WhiteSmoke;
            dateLabel.ForeColor = Color.WhiteSmoke;
            timeLabel.ForeColor = Color.WhiteSmoke;

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);

            timer = new System.Windows.Forms.Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void ReadExceptionsFile()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(Application.ExecutablePath);
            watcher.Filter = "exceptions.txt";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += (sender, e) =>
            {
                Thread.Sleep(1000);
                using (StreamReader streamReader = new StreamReader("exceptions.txt"))
                {

                    LogBox.Invoke((MethodInvoker)(() =>
                    {
                        LogBox.Text = streamReader.ReadToEnd();
                    }));
                }
            };
            watcher.EnableRaisingEvents = true;
        }

        private void UpdateLogBox()
        {
            using (StreamReader streamReader = new StreamReader("exceptions.txt"))
            {
                LogBox.Text = streamReader.ReadToEnd() ;
            }
        }

        private void ClearExceptionsFile()
        {
            File.WriteAllText("exceptions.txt", "");
            LogBox.Clear();
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            ClearExceptionsFile();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        private void lab1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lab1 newForm = new Lab1(this);
            newForm.Show();
        }
    }
}
