using System;
using System.Net;
using System.Windows.Forms;
using System.Net.Sockets;

namespace ShowIPInter
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        
        public Form1()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            InitializeComponent();
            InitializeTrayIcon();

            string hostname = Dns.GetHostName();
            string userName = Environment.UserName;
            string hostName = Environment.UserDomainName;
            IPAddress[] addresses = Dns.GetHostAddresses(hostname);

            string ipText = string.Empty;

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                   ipText += address.ToString() + "\n";
                }
            }
            if (!string.IsNullOrEmpty(ipText))
            {
                ipText = ipText.TrimEnd('\n');
            }

            notifyIcon.Text = ("User: " + userName + "\nIP: " + ipText + "\nDomain: " + hostName);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.lovedsgn;
            notifyIcon.Visible = true;

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            notifyIcon.ContextMenuStrip = contextMenu;
        }
    }
}
