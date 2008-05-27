﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ankh.UI.Presenters;
using Ankh.UI.Services;
using Microsoft.VisualStudio.Shell.Interop;
using Ankh.Selection;

namespace Ankh.UI.SvnLog
{
    public partial class LogToolControl : UserControl
    {
        IAnkhUISite _site;

        public LogToolControl()
        {
            InitializeComponent();
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                IAnkhUISite site = value as IAnkhUISite;

                if (site != null)
                {
                    _site = site;

                    logRevisionControl1.Site = site;

                    if (_site.GetService<LogToolControl>() == null)
                        _site.Package.AddService(typeof(LogToolControl), this);
                }
            }
        }

        LogMode _mode;
        public LogMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public void Start(ICollection<string> targets)
        {
            logRevisionControl1.LocalTargets = targets;
            logRevisionControl1.Reset();
            logRevisionControl1.Start(_site.GetService<IAnkhServiceProvider>(), Mode);
        }


        void CreatePresenter(IAnkhServiceProvider context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

        }
    }
}
