using System;
using System.Collections;
using Ankh.UI;
using System.Windows.Forms;
using NSvn.Core;
using NSvn.Common;
using System.IO;

namespace Ankh.Commands
{
    /// <summary>
    /// A command that performs a reverse merge on an item/items, allowing
    /// the user to go back to a previous version.
    /// </summary>
    [VSNetCommand( "ReverseMerge", Text="Revert to revision...", 
         Tooltip="Go back to a previous version of this item.", 
         Bitmap = ResourceBitmaps.Diff),
    VSNetProjectItemControl( "Ankh", Position=2 ),
    VSNetProjectNodeControl( "Ankh", Position = 2 ),
    VSNetControl( "Solution.Ankh", Position = 2 ),
    VSNetFolderNodeControl( "Ankh", Position = 2)]
    public class ReverseMergeCommand : CommandBase
    {
        public override EnvDTE.vsCommandStatus QueryStatus(IContext context)
        {
            IList resources = context.SolutionExplorer.GetSelectionResources(
                true, new ResourceFilterCallback(SvnItem.VersionedFilter) );
            if ( resources.Count > 0 )
                return Enabled;
            else
                return Disabled;
        }

        public override void Execute(IContext context, string parameters)
        {
            this.SaveAllDirtyDocuments( context );

            IList resources = context.SolutionExplorer.GetSelectionResources(
                true, new ResourceFilterCallback(SvnItem.VersionedFilter) );
            context.StartOperation( "Merging" );
            try
            {
                using ( ReverseMergeDialog dlg = new ReverseMergeDialog() )
                {
                    dlg.GetPathInfo += new GetPathInfoDelegate(SvnItem.GetPathInfo);
                    dlg.Items = resources;
                    dlg.CheckedItems = resources;
                    dlg.Recursive = true;

                    if ( dlg.ShowDialog( context.HostWindow ) != DialogResult.OK )
                        return;

                    ReverseMergeRunner runner = new ReverseMergeRunner(
                        dlg.CheckedItems, dlg.Revision, dlg.Recursive ? Recurse.Full : Recurse.None,
                        dlg.DryRun );

                    using ( ProjectFileWatcherScope scope = new ProjectFileWatcherScope(context) )
                    {
                        context.UIShell.RunWithProgressDialog( runner, "Merging" ); 
                    }
                }
            }
            finally
            {
                context.EndOperation();
            }
        }
        
        /// <summary>
        /// A progressrunner for reverse merges.
        /// </summary>
        private class ReverseMergeRunner : IProgressWorker
        {
            public ReverseMergeRunner(  IList items, Revision revision,
                Recurse recurse, bool dryRun ) 
            {
                this.items = items;
                this.revision = revision;
                this.recurse = recurse;
                this.dryRun = dryRun;
            }

            public void Work( IContext context )
            {
                foreach( SvnItem item in this.items )
                {
                    context.Client.Merge( 
                        item.Path, Revision.Working,
                        item.Path, this.revision,
                        item.Path, this.recurse,
                        false, false, this.dryRun );
                }
            }

            private IList items;
            private Revision revision;
            private Recurse recurse;
            private bool dryRun;
        }
    }
}