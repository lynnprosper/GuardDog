﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Utility;

namespace GuardDog
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource cancellSource = new CancellationTokenSource();

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.btStop.Enabled = false;
            //InitializePage();
            Debugger.SetOut(new MyWriter(SynchronizationContext.Current, SetTblog));
            Console.SetOut(Debugger.Out);
        }


        private void SetTblog(string msg)
        {
            this.tbLog.AppendText(msg + "\r\n");
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStart_Click(object sender, EventArgs e)
        {
            this.btStart.Enabled = false;
            this.btStop.Enabled = true;
            if (this.cancellSource.IsCancellationRequested)
            {
                this.cancellSource = new CancellationTokenSource();
            }

            Task.Run(() =>
            {
                foreach (var item in DogConfig.GetConfigServiceNames())
                {
                    var control = new ServiceMonitor(item);
                    control.StartAsync(this.cancellSource.Token);
                }
            });

            Task.Run(() =>
            {
                foreach (var item in DogConfig.GetConfigProcessPaths())
                {
                    var control = new ProcessMonitor(item);
                    control.StartAsync(this.cancellSource.Token);
                }
            });
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            this.btStart.Enabled = true;
            this.btStop.Enabled = false;
            this.cancellSource.Cancel();
        }
    }
}
