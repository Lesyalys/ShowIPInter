using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Windows.Forms;




namespace ShowIPInter
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        private string voice = "Перезагрузить Radmin";

        public Form1()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            InitializeComponent();
            InitializeTrayIcon();

            string hostname = Dns.GetHostName();
            string userName = Environment.UserName;
            string UserDomainName = Environment.UserDomainName;
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
            notifyIcon.Text = ("User: " + userName + "\nIP: " + ipText + "\nDomain: " + UserDomainName);
            try {
                WraiteLog(); 
             }
            catch { 
            
            };

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

            ToolStripMenuItem restartItem = new ToolStripMenuItem(voice);
            ToolStripMenuItem restartItem2 = new ToolStripMenuItem("Статус Radmin");
            ToolStripMenuItem restartItem3 = new ToolStripMenuItem();

            restartItem.Click += new EventHandler(RestartOtherApp_Click);

            contextMenu.Items.Add(restartItem);
            contextMenu.Items.Add(restartItem2);
            contextMenu.Items.Add(restartItem3);

            notifyIcon.MouseClick += Notifyicon_MouseClick;
        }

        private void Notifyicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string statusResult = Refresh_Click();
                string wifiStatus = IsWiFiConnected();
                string InfoUser1 = InfoUser();

                notifyIcon.ContextMenuStrip.Items[1].Text = $"{statusResult}\n" + $"{wifiStatus}";
                notifyIcon.ContextMenuStrip.Items[2].Text = $"{InfoUser1}";

            }
        }


        public void RestartOtherApp_Click(object sender, EventArgs e)
        {
            string serviceName = "RServer3";
            var stopPathArgument = "/stop";
            var runPathArgument = "/start";
            var stopPath = Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\Windows\\SysWOW64\\rserver30\\rserver3.exe");
            var runPath = Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%\\Windows\\SysWOW64\\rserver30\\rserver3.exe");

            if (!System.IO.File.Exists(stopPath))
            {
                voice = ("Не найдена служба Radmin.");
                return;
            }

            try
            {
                ServiceController service = new ServiceController(serviceName);

                if (service.Status == ServiceControllerStatus.Running)
                {
                    Process.Start(stopPath, stopPathArgument);
                }

                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(60));

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    Process.Start(runPath, runPathArgument);
                }

                MessageBox.Show("Служба Radmin успешно перезагружена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при перезапуске службы: " + ex.Message);
            }
        }

        private string Refresh_Click()
        {
            string serviceName = "RServer3";
            ServiceController service = new ServiceController(serviceName);
            string radmStatus = service.Status == ServiceControllerStatus.Running ? "Включен ✓" : "Выключен ×";
            return "Статус Radmin: " + radmStatus;
        }

        private string IsWiFiConnected()
        {
            bool networkUo = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            string connectStatus = networkUo ? "Cетевое соеденение: В сети ✓" : "Cетевое соеденение: Нет сети × (позвоните 7097)";
            return connectStatus;


            //bool isConnected = false;
            //using (var tcpClient = new TcpClient())
            //    try
            //    {

            //            var thread = new Thread(() => tcpClient.Connect("178.210.92.7", 443));
            //            thread.Start();
            //            var completed = thread.Join(30);
            //            if (!completed) thread.Abort();

            //            //tcpClient.Connect("178.210.92.7", 443);
            //            isConnected = tcpClient.Connected;

            //    }
            //    catch
            //    {
            //        isConnected = false;
            //    }
            //string wifistat = isConnected ? "Интернет: В сети ✓" : "Интернет: Нет сети × (позвоните 7097)";
            //return wifistat + "\n" + connectStatus;
        }

        private string InfoUser()
        {
            string hostname = Dns.GetHostName();
            string userName = Environment.UserName;
            string UserDomainName = Environment.UserDomainName;
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
            return notifyIcon.Text = ("User: " + userName + "\nIP: " + ipText + "\nDomain: " + UserDomainName);

        }

        private void WraiteLog()
        {// \\dcnvaero\Barter\!Показать IP-адрес\ShowIpLog
            string LogFilePath = @"\\dcnvaero\Barter\!Показать IP-адрес\ShowIpLog\Log.txt";

            string hostname = Dns.GetHostName();
            string userName = Environment.UserName;
            string UserDomainName = Environment.UserDomainName;
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

            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}" + " | " + UserDomainName + " | "+ hostname + " | " + userName + " | " + ipText);
            }

        }
    }
}