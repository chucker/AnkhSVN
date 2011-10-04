// $Id$
using System;
using System.Collections;
using Ankh.UI;

namespace Ankh
{
    /// <summary>
    /// Summary description for CommitContext.
    /// </summary>
    public class CommitContext
    {
        public CommitContext( LogMessageTemplate template, IList commitItems, 
            bool urlPaths )
        {
            this.LogMessageTemplate = template;
            this.CommitItems = commitItems;
            this.urlPaths = urlPaths;
            this.keepLocks = false;
            this.cancelled = false;
        }

        public string LogMessage
        {
            get{ return this.logMessage; }
            set{ this.logMessage = value; }
        }

        public LogMessageTemplate LogMessageTemplate
        {
            get{ return this.logMessageTemplate; }
            set{ this.logMessageTemplate = value; }
        }

        public string RawLogMessage
        {
            get{ return this.rawLogMessage; }
            set{ this.rawLogMessage = value; }
        }


        public IList CommitItems
        {
            get{ return this.commitItems; }
            set{ this.commitItems = value; }
        }

        public bool UrlPaths
        {
            get{ return this.urlPaths; }
            set{ this.urlPaths = value; }
        }

        public bool Cancelled
        {
            get{ return this.cancelled; }
            set{ this.cancelled = value; }
        }

        public bool KeepLocks
        {
            get{ return this.keepLocks; }
            set{ this.keepLocks = value; }                      
        }

        private bool cancelled = true;

        private bool urlPaths;
        private IList commitItems;
        private LogMessageTemplate logMessageTemplate;
        private string rawLogMessage = "";
        private string logMessage = "";
        private bool keepLocks;
        
    }
}