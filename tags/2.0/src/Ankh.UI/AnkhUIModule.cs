﻿using System;
using System.Collections.Generic;
using System.Text;
using Ankh.UI.PendingChanges;
using Ankh.UI.Services;

namespace Ankh.UI
{
    public class AnkhUIModule : Module
    {
        public AnkhUIModule(AnkhRuntime runtime)
            : base(runtime)
        {

        }

        public override void OnPreInitialize()
        {
            Runtime.CommandMapper.LoadFrom(typeof(AnkhUIModule).Assembly);

            // Instantiate the logmessage language service
            LogMessageLanguageService ls = new LogMessageLanguageService(Context);
            Container.AddService(typeof(LogMessageLanguageService), ls, true);
            ls.SetSite(Container);
        }

        public override void OnInitialize()
        {
            
        }
    }
}