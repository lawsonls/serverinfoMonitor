using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace Monitor
{
    /// <summary>
    /// 主窗口
    /// </summary>
    public partial class Form1 : Form
    {
        static bool Asc = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowProcess();

            ShowPort();
        }
        /// <summary>
        /// 显示端口占用信息
        /// </summary>
        public void ShowPort()
        {
            string content = CMDHelper.RunCmd("netstat -bno");
            List<NetConnData> netData = AnalysNetstat.Analysis(content);
            this.listViewPort.Items.Clear();
            int i = 1;
            foreach (NetConnData info in netData)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();
                i = i + 1;
                item.SubItems.AddRange(new string[] { info.Protocal, info.LocalIP, info.LocalPort, info.ForeignIP,info.ForeignPort,
                    info.Status, info.PID, info.ProcessName,info.ProcessLocation});

                this.listViewPort.Items.Add(item);
            }
        }
        /// <summary>
        /// 显示当前进程信息
        /// </summary>
        public void ShowProcess()
        {
            this.listView1.Items.Clear();
            List<ProcessShowInfo> data = GetProcess();

            //data.Sort(new ProcessShowCompare(0, 1, Asc));
            int i = 1;
            foreach (ProcessShowInfo info in data)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();
                i = i + 1;
                item.SubItems.AddRange(new string[] { info.PID, info.Name, info.ThreadNum, info.Location });

                this.listView1.Items.Add(item);
            } 
        }

        /// <summary>
        /// 获取当前系统进程
        /// </summary>
        /// <returns></returns>
        public List<ProcessShowInfo> GetProcess()
        {
            List<ProcessShowInfo> result = new List<ProcessShowInfo>();
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                ProcessShowInfo info = new ProcessShowInfo();
                try
                {
                    info.PID = p.Id.ToString();
                    info.Name = p.ProcessName;
                    info.ThreadNum = p.Threads.Count.ToString();
                    info.Location = p.MainModule.FileName;
                }
                catch (Exception ex)
                { }

                result.Add(info);
            }

            return result;
        }

        /// <summary>
        /// 为进程排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">点击事件</param>
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.Items.Clear();
            List<ProcessShowInfo> data = GetProcess();
            if (e.Column == 1 || e.Column == 3)
            {
                data.Sort(new ProcessShowCompare(e.Column, 1, Asc));
            }
            else
            {
                data.Sort(new ProcessShowCompare(e.Column, 0, Asc)); 
            }
            Asc = !Asc;
            int i = 1;
            foreach (ProcessShowInfo info in data)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();
                i = i + 1;
                item.SubItems.AddRange(new string[] { info.PID, info.Name, info.ThreadNum, info.Location });
                this.listView1.Items.Add(item);
            }
        }

        /// <summary>
        /// 为端口占用情况排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPort_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listViewPort.Items.Clear();

            string content = CMDHelper.RunCmd("netstat -bno");
            List<NetConnData> netData = AnalysNetstat.Analysis(content);
            this.listViewPort.Items.Clear();

            if (e.Column == 3 || e.Column == 5 || e.Column == 7)
            {
                netData.Sort(new NetConnDataCompare(e.Column, 1, Asc));
            }
            else
            {
                netData.Sort(new NetConnDataCompare(e.Column, 0, Asc));
            }
            //每次点击互换正序反序
            Asc = !Asc;

            int i = 1;
            foreach (NetConnData info in netData)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();
                i = i + 1;
                item.SubItems.AddRange(new string[] { info.Protocal, info.LocalIP, info.LocalPort, info.ForeignIP,info.ForeignPort,
                    info.Status, info.PID, info.ProcessName,info.ProcessLocation});

                this.listViewPort.Items.Add(item);
            }
        }

        /// <summary>
        /// 刷新当前面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowProcess();

            ShowPort();
        }

        private void lbContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:liusen@travelsky.com");
        }
    }
    
}

