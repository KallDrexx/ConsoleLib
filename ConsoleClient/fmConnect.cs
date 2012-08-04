using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using ConsoleLib.Console.Networking;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public partial class fmConnect : Form
    {
        public DuplexChannelFactory<IConsoleInterface> WcfChannelFactory { get; protected set; }
        public IConsoleInterface Proxy { get; set; }
        public string ConnectedHost { get; protected set; }
        public ConsoleCallbacks CallBack { get; set; }

        public fmConnect()
        {
            InitializeComponent();
        }

        private void fmConnect_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHostName.Text))
            {
                MessageBox.Show("Invalid server address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Invalid port number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Attempt a connection
            txtHostName.Enabled = false;
            txtPort.Enabled = false;
            btnConnect.Enabled = false;

            ConnectedHost = string.Format("net.tcp://{0}:{1}", txtHostName.Text.Trim(), port);
            WcfChannelFactory = new DuplexChannelFactory<IConsoleInterface>(CallBack, new NetTcpBinding(), new EndpointAddress(ConnectedHost));
            Proxy = WcfChannelFactory.CreateChannel();
            
            var task = new Task(() => Proxy.Subscribe());
            task.ContinueWith((t) => ConnectionSuccessful(), TaskContinuationOptions.NotOnFaulted);
            task.ContinueWith((t) => ConnectionUnsuccessful(), TaskContinuationOptions.OnlyOnFaulted);
            task.Start();
        }

        protected void ConnectionSuccessful()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.Close();
            });
        }

        protected void ConnectionUnsuccessful()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                txtHostName.Enabled = true;
                txtPort.Enabled = true;
                btnConnect.Enabled = true;

                // Reset the properties
                WcfChannelFactory = null;
                ConnectedHost = null;
                Proxy = null;

                MessageBox.Show("Connection attempt failed", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }
}
