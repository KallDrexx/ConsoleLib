using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConsoleLib.Console.Networking;
using System.ServiceModel;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleClient
{
    public partial class Form1 : Form
    {
        protected const int MS_BETWEEN_CONSOLE_UPDATES = 100;

        protected Callbacks _callbacks;
        protected IConsoleInterface _proxy;
        protected StringBuilder _cachedOutput;
        protected Timer _updateOutputTimer;
        protected int _startFormHeight;
        protected int _startInputHeight;
        protected bool _outputPaused;
        private object _outputUpdateLock = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _startFormHeight = this.Height;
            _startInputHeight = txtInput.Height;
            _cachedOutput = new StringBuilder();
            _updateOutputTimer = new Timer();
            _updateOutputTimer.Tick += TimerTick;
            _updateOutputTimer.Interval = MS_BETWEEN_CONSOLE_UPDATES;
            _updateOutputTimer.Start();
            unPauseOutputToolStripMenuItem.Enabled = false;

            ConnectToConsoleServer();  
        }

        protected void ConnectToConsoleServer()
        {
            _callbacks = new Callbacks();
            _callbacks.OutputReceivedHandlers += NewOutputReceived;

            var factory = new DuplexChannelFactory<IConsoleInterface>(_callbacks, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4000"));
            _proxy = factory.CreateChannel();

            // Has to be done in a seperate thread so it doesn't block the callback
            new Task(() => _proxy.Subscribe()).Start();
        }

        protected void NewOutputReceived(IEnumerable<string> text, string category)
        {
            // cache the output for performance reasons
            foreach (string line in text)
                lock(_outputUpdateLock)
                    _cachedOutput.AppendLine(line);
        }

        protected void TimerTick(object sender, EventArgs e)
        {
            lock (_outputUpdateLock)
            {
                // Add the cached output
                if (!_outputPaused && _cachedOutput.Length > 0)
                {
                    txtDisplay.AppendText(_cachedOutput.ToString());
                    _cachedOutput.Clear();
                }
            }
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Send the input to the proxy
            new Task(() => _proxy.ProcessInput(txtInput.Text)).Start();
            txtInput.SelectAll();
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                txtInput.SelectAll();
        }

        private void pauseOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _outputPaused = true;
            pauseOutputToolStripMenuItem.Enabled = false;
            unPauseOutputToolStripMenuItem.Enabled = true;
        }

        private void unPauseOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _outputPaused = false;
            unPauseOutputToolStripMenuItem.Enabled = false;
            pauseOutputToolStripMenuItem.Enabled = true;
        }

        private void saveScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    string fileName = dlg.FileName;
                    var file = File.CreateText(fileName);
                    file.Write(txtInput.Text);
                    file.Close();
                }
                catch (IOException ex)
                {
                    string message = "An error occurred while attempting to save: " + ex.Message;
                    MessageBox.Show(message, "Error saving script", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pause the output while saving
            bool wasPaused = _outputPaused;
            _outputPaused = true;

            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".txt";
            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    string fileName = dlg.FileName;
                    var file = File.CreateText(fileName);
                    file.Write(txtDisplay.Text);
                    file.Close();
                }
                catch (IOException ex)
                {
                    string message = "An error occurred while attempting to save: " + ex.Message;
                    MessageBox.Show(message, "Error saving script", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _outputPaused = wasPaused;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to exit?";
            var result = MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = "Are you sure you want to exit?";
            var result = MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void txtCategory_Leave(object sender, EventArgs e)
        {
            _proxy.ChangeCategory(txtCategory.Text.Trim());
            NewOutputReceived(
                new string[] { string.Concat("(Changed to category: ", txtCategory.Text.Trim(), ")") },
                txtCategory.Text.Trim());
        }
    }
}
