using System;
using System.IO;
using NUnit.Framework;
using NSvn.Core;
using EnvDTE;
using Ankh.Commands;
using System.Collections;

namespace Ankh.Tests
{
    /// <summary>
    /// Test for the CommitItemCommand class.
    /// </summary>
    [TestFixture]
    public class CommitItemTest : NSvn.Core.Tests.TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            this.ExtractWorkingCopy();
            this.ExtractRepos();   
            TestUtils.ToggleAnkh( false, "7.1" );

            this.uiShell = new MyUIShellImpl();
            this.explorer = new MyExplorerImpl();
            this.ctx = new MyContextBase( this.explorer, this.uiShell );
            ctx.Client.SynchronizingObject = new ContextBase.NoSynch();

            this.cmd = new CommitItemCommand();

        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            TestUtils.ToggleAnkh( true, "7.1" );
        }
        
        [Test]
        public void TestQueryStatus()
        {            
            string path = Path.Combine( this.WcPath, "Class1.cs" );
            SvnItem item = this.ctx.StatusCache[path];
            Assert.AreEqual( StatusKind.Normal, item.Status.TextStatus );

            // modify the item
            this.Modify( path );
            item.Refresh( this.ctx.Client );
            Assert.AreEqual( StatusKind.Modified, item.Status.TextStatus );      

            // commit should not be enabled for no selection
            Assert.AreEqual( vsCommandStatus.vsCommandStatusSupported, 
                this.cmd.QueryStatus( this.ctx ) );

            this.explorer.Selection = new SvnItem[]{ item };

            // check that Commit is enabled on this modified item
            Assert.AreEqual( vsCommandStatus.vsCommandStatusEnabled |
            vsCommandStatus.vsCommandStatusSupported, this.cmd.QueryStatus( this.ctx ) );      
        }

        [Test]
        public void TestCommitSingleFile()
        {
            string path = Path.Combine( this.WcPath, "Class1.cs" );
            this.Modify( path );
            SvnItem item = this.ctx.StatusCache[path];
            int oldRev = item.Status.Entry.Revision;

            this.uiShell.CommitItems = this.explorer.Selection = new object[]{ item };
            this.uiShell.LogMessage = "42";

            this.cmd.Execute( this.ctx, "" );

            Assert.IsTrue( this.uiShell.ShowCommitDialogModalCalled );

            item.Refresh( this.ctx.Client );
            Assert.AreEqual( StatusKind.Normal, item.Status.TextStatus );

            ctx.Client.Log( new string[]{ item.Path }, Revision.Head, Revision.Head, false, false,
                new LogMessageReceiver( this.Receiver ) );

            Assert.AreEqual( "42", this.message.Message );
        }

        [Test]
        public void TestCommitAddedFile()
        {
            string path = Path.Combine( this.WcPath, "NewFile.cs" );
            this.Modify( path );
            this.ctx.Client.Add( path, false );

            SvnItem item = this.ctx.StatusCache[path];
            Assert.AreEqual( StatusKind.Added, item.Status.TextStatus );

            this.uiShell.CommitItems = this.explorer.Selection = new object[]{ item };
            this.uiShell.LogMessage = "42";

            this.cmd.Execute( this.ctx, "" );

            Assert.IsTrue( this.uiShell.ShowCommitDialogModalCalled );

            item.Refresh( this.ctx.Client );
            Assert.AreEqual( StatusKind.Normal, item.Status.TextStatus );

            ctx.Client.Log( new string[]{ item.Path }, Revision.Head, Revision.Head, false, false,
                new LogMessageReceiver( this.Receiver ) );

            Assert.AreEqual( "42", this.message.Message );
        }

        private void Modify( string path )
        {
            using( StreamWriter w = new StreamWriter( path ) )
                w.WriteLine( "Hi" );
        }

        private void Receiver( LogMessage message )
        {
            this.message = message;
        }


        /// <summary>
        /// Helper method refactored out of the test methods.
        /// </summary>
        /// <returns></returns>
        private ContextBase CreateContextAndLoad()
        {
            
            return ctx;
        }

        private UIHierarchy UIHierarchy
        {
            get
            {
                return (UIHierarchy)this.ctx.DTE.Windows.Item( 
                    Constants.vsWindowKindSolutionExplorer ).Object; 
            } 
        }

        private class MyContextBase : ContextBase
        {
            public MyContextBase( ISolutionExplorer explorer, IUIShell uiShell)
            {
                this.uiShell = uiShell;
                this.uiShell.Context = this;
                this.explorer = explorer;
            }
        }

        private class MyUIShellImpl : ContextBase.UIShellImpl
        {
            public override CommitContext ShowCommitDialogModal(CommitContext ctx)
            {
                ctx.LogMessage = this.LogMessage;
                ctx.Cancelled = this.Cancelled;
                ctx.CommitItems = this.CommitItems;

                this.ShowCommitDialogModalCalled = true;

                return ctx;
            }

            public bool ShowCommitDialogModalCalled = false;
            public string LogMessage = "";
            public bool Cancelled = false;
            public IList CommitItems = new object[]{};
        }

        private class MyExplorerImpl : ISolutionExplorer
        {
            #region ISolutionExplorer Members

            public System.Collections.IList GetItemResources(ProjectItem item, bool recursive)
            {
                // TODO:  Add MyExplorerImpl.GetItemResources implementation
                return null;
            }

            public void Unload()
            {
                // TODO:  Add MyExplorerImpl.Unload implementation
            }

            public void VisitSelectedNodes(Ankh.Solution.INodeVisitor visitor)
            {
                // TODO:  Add MyExplorerImpl.VisitSelectedNodes implementation
            }

            public ProjectItem GetSelectedProjectItem()
            {
                // TODO:  Add MyExplorerImpl.GetSelectedProjectItem implementation
                return null;
            }

            public void Refresh(ProjectItem item)
            {
                // TODO:  Add MyExplorerImpl.Refresh implementation
            }

            void Ankh.ISolutionExplorer.Refresh(Project project)
            {
                // TODO:  Add MyExplorerImpl.Ankh.ISolutionExplorer.Refresh implementation
            }

            #endregion

            #region ISelectionContainer Members

            public void RefreshSelectionParents()
            {
                // TODO:  Add MyExplorerImpl.RefreshSelectionParents implementation
            }

            public void SyncAll()
            {
                // TODO:  Add MyExplorerImpl.SyncAll implementation
            }

            public System.Collections.IList GetSelectionResources(bool getChildItems, Ankh.ResourceFilterCallback filter)
            {
                // TODO:  Add MyExplorerImpl.GetSelectionResources implementation
                return this.Selection;
            }

            System.Collections.IList Ankh.ISelectionContainer.GetSelectionResources(bool getChildItems)
            {
                return this.Selection;
            }

            public void RefreshSelection()
            {
                // TODO:  Add MyExplorerImpl.RefreshSelection implementation
            }

            public System.Collections.IList GetAllResources(Ankh.ResourceFilterCallback filter)
            {
                // TODO:  Add MyExplorerImpl.GetAllResources implementation
                return null;
            }

            #endregion

            public IList Selection = new object[]{};
        }

        private MyContextBase ctx;
        private MyExplorerImpl explorer;
        private MyUIShellImpl uiShell;
        private CommitItemCommand cmd;
        private LogMessage message;
    }
}
