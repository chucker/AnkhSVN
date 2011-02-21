// $Id$
using System;
using EnvDTE;

using Ankh.UI;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;
using NSvn.Core;

namespace Ankh.Commands
{
    /// <summary>
    /// Allows the user to resolve a conflicted file.
    /// </summary>
    [VSNetCommand( "ResolveConflict", Text="Resolve",  Bitmap = ResourceBitmaps.Default, 
         Tooltip = "Resolve conflicted file"),
     VSNetControl( "Item.Ankh", Position = 1 ),
     VSNetControl( "Project.Ankh", Position = 1 ),
     VSNetControl( "Solution.Ankh", Position = 1)]
    internal class ResolveConflictCommand : CommandBase
    {    
        public override EnvDTE.vsCommandStatus QueryStatus(AnkhContext context)
        {
            int count = context.SolutionExplorer.GetSelectionResources(false, 
                new ResourceFilterCallback(ResolveConflictCommand.ConflictedFilter) ).Count;

            if ( count > 0 )
                return vsCommandStatus.vsCommandStatusEnabled |
                    vsCommandStatus.vsCommandStatusSupported;
            else
                return vsCommandStatus.vsCommandStatusSupported;
        }

        public override void Execute(AnkhContext context, string parameters)
        {
            context.StartOperation( "Resolving" );

            try
            {
                IList items = context.SolutionExplorer.GetSelectionResources(false, 
                     new ResourceFilterCallback(ResolveConflictCommand.ConflictedFilter) );

                foreach( SvnItem item in items )
                {
                    this.Resolve( context, item );
                    item.Refresh( context.Client );
                }
            }
            finally
            {
                context.EndOperation();
            }
        }

        /// <summary>
        /// Resolve an item.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="item"></param>
        private void Resolve(AnkhContext context, SvnItem item)
        {
            int oldRev, newRev;
            this.GetRevisions( item, out oldRev, out newRev );

            ConflictDialog.Choice selection;

            using( ConflictDialog dialog = new ConflictDialog(  ) )
            {
                dialog.OldRev = oldRev;
                dialog.NewRev = newRev;
                dialog.Filename = item.Path;

                if ( dialog.ShowDialog( context.HostWindow ) != DialogResult.OK )
                    return;
                    
                selection = dialog.Selection;
            }       

            // should we copy one of the files over the original?
            switch( selection )
            {
                case ConflictDialog.Choice.OldRev:
                    this.Copy( item.Path, item.Status.Entry.ConflictOld );
                    break;
                case ConflictDialog.Choice.NewRev:
                    this.Copy( item.Path, item.Status.Entry.ConflictNew  );
                    break;
                case ConflictDialog.Choice.Mine:
                    this.Copy( item.Path, item.Status.Entry.ConflictWorking );
                    break;
                default:
                    break;
            }

            context.Client.Resolved( item.Path, false );
            context.OutputPane.WriteLine( 
                "Resolved conflicted state of {0}", item.Path );
        }

        /// <summary>
        /// Retrieve the conflicted revisions.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="oldRev"></param>
        /// <param name="newRev"></param>
        private void GetRevisions( SvnItem item, out int oldRev, out int newRev )
        {
            oldRev = int.Parse( NUMBER.Match( item.Status.Entry.ConflictOld ).Groups[1].Value );
            newRev = int.Parse( NUMBER.Match( item.Status.Entry.ConflictNew ).Groups[1].Value );
        }  

        private void Copy( string toPath, string fromFile )
        {
            string dir = Path.GetDirectoryName( toPath );
            string fromPath = Path.Combine( dir, fromFile );
            File.Copy( fromPath, toPath,  true );
        }

        /// <summary>
        /// Filter for conflicted items.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool ConflictedFilter( SvnItem item )
        {
            return item.Status.TextStatus == StatusKind.Conflicted; 
        }
       
        private readonly Regex NUMBER = new Regex( @".*\.r(\d+)" );

    }
}



