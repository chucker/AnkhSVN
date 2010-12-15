// $Id$
using System;
using System.IO;
using NUnit.Framework;

namespace NSvn.Core.Tests
{
    /// <summary>
    /// Tests the Client.Commit method.
    /// </summary>
    [TestFixture]
    public class CommitTest : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            this.ExtractRepos();
            this.ExtractWorkingCopy();
        }

        /// <summary>
        /// Modifies a file in the working copy and commits the containing directory.
        /// </summary>
        [Test]
        public void TestBasicCommit()
        {
            string filepath = Path.Combine( this.WcPath, "Form.cs" );
            using ( StreamWriter w = new StreamWriter( filepath ) )
                w.Write( "Moo" );

            ClientContext ctx = new ClientContext( new NotifyCallback( this.NotifyCallback ) );
            Client.Commit( new string[]{ this.WcPath }, false, ctx );
           
            char status = this.GetSvnStatus( filepath );
            Assertion.AssertEquals( "File not committed", '-', 
                status );
        }

        /// <summary>
        /// Commits a single file
        /// </summary>
        [Test]
        public void TestCommitFile()
        {
            string filepath = Path.Combine( this.WcPath, "Form.cs" );
            using ( StreamWriter w = new StreamWriter( filepath ) )
                w.Write( "Moo" );

            ClientContext ctx = new ClientContext();
            CommitInfo info = Client.Commit( new string[]{ filepath }, true, ctx );

            char status = this.GetSvnStatus( filepath );
            Assertion.AssertEquals( "File not committed", '-', 
                status );

        }

        [Test]
        public void TestCommitWithLogMessage()
        {
            this.filepath = Path.Combine( this.WcPath, "Form.cs" );
            using ( StreamWriter w = new StreamWriter( filepath ) )
                w.Write( "Moo" );
            ClientContext ctx = new ClientContext();

            AuthenticationBaton baton = new AuthenticationBaton();
            baton.Add( AuthenticationProvider.GetUsernameProvider() );
            baton.SetParameter( AuthenticationBaton.ParamDefaultUsername, Environment.UserName );

            ctx.AuthBaton = baton;

            ctx.LogMessageCallback = new LogMessageCallback( this.LogMessageCallback );
            this.logMessage = "Moo ";
            CommitInfo info = Client.Commit( new string[]{ this.WcPath }, false, ctx );

            Assertion.AssertEquals( "Wrong username", Environment.UserName, info.Author );
            string output = this.RunCommand( "svn", "log " + this.filepath + " -r HEAD" );
            
            Assertion.Assert( "Log message not set", 
                output.IndexOf( this.logMessage ) >= 0 );

        } 

        [Test]
        public void TestCommitWithNonAnsiCharsInLogMessage()
        {
            this.filepath = Path.Combine( this.WcPath, "Form.cs" );
            using ( StreamWriter w = new StreamWriter( filepath ) )
                w.Write( "Moo" );
            ClientContext ctx = new ClientContext();
            ctx.LogMessageCallback = new LogMessageCallback( this.LogMessageCallback );
            this.logMessage = "� e i a � �. M����! �ber";
            CommitInfo info = Client.Commit( new string[]{ this.WcPath }, false, ctx );
        }
        
        /// <summary>
        /// Tests that you can cancel a commit.
        /// </summary>
        [Test]
        public void TestCancelledCommit()
        {
            string path = Path.Combine( this.WcPath, "Form.cs" );
            using( StreamWriter w = new StreamWriter( path ) )
                w.Write( "MOO" );
            ClientContext ctx = new ClientContext();
            ctx.LogMessageCallback = new LogMessageCallback( this.CancelLogMessage );
            CommitInfo info = Client.Commit( new string[]{ path }, true, ctx );

            Assertion.AssertNull( "info should be null for a cancelled commit", 
                info );

            string output = this.RunCommand( "svn", "st " + this.WcPath ).Trim();
            Assertion.AssertEquals( "File committed even for a cancelled log message", 'M', 
                output[0] );        
   

        }

        [Test]
        [ExpectedException( typeof(WorkingCopyLockedException) )]
        public void TestLockedWc()
        {
            string lockPath = Path.Combine( this.WcPath, @".svn\lock" );
            using( File.CreateText( lockPath ) )
            {
                Client.Commit( new string[]{ this.WcPath }, true, 
                    new ClientContext() );            
            }
        }

        private string LogMessageCallback( CommitItem[] items )
        {
            Assertion.AssertEquals( "Wrong number of commit items", 1, items.Length );
            Assertion.Assert( "Wrong path", items[0].Path.IndexOf( 
                this.filepath ) >= 0 );
            Assertion.AssertEquals( "Wrong kind", NodeKind.File, items[0].Kind );
            Assertion.AssertEquals( "Wrong revision", 6, items[0].Revision );

            return this.logMessage;
        }

        private string CancelLogMessage( CommitItem[] items )
        {
            return null;
        }


        private string logMessage;
        private string filepath;
    }
}