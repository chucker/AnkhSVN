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
    [VSNetCommand( "ResolveConflict", Text="Resolve conflicted file...",  
         Bitmap = ResourceBitmaps.Default, 
         Tooltip = "Resolve conflicted file"),
     VSNetControl( "Item.Ankh", Position = 1 ),
     VSNetControl( "Project Node.Ankh", Position = 1 ),
    VSNetControl( "Cab Project Node.Ankh", Position = 1 ),
    VSNetControl( "Project.Ankh", Position = 1 ),
     VSNetControl( "Solution.Ankh", Position = 1)]
    internal class ResolveConflictCommand : CommandBase
    {    
        public override EnvDTE.vsCommandStatus QueryStatus(AnkhContext context)
        {
            int count = context.SolutionExplorer.GetSelectionResources(false, 
                new ResourceFilterCallback(ResolveConflictCommand.ConflictedFilter) ).Count;

            if ( count > 0 )
                return Enabled;
            else
                return Disabled;
        }

        public override void Execute(AnkhContext context, string parameters)
        {
            this.SaveAllDirtyDocuments( context );

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

            string mergeExe = context.Config.MergeExePath;
            if (mergeExe == null)
            {
                string selection;

                using( ConflictDialog dialog = new ConflictDialog(  ) )
                {
                    Entry entry = item.Status.Entry;
                    dialog.Filenames = new string[]{
                                                       entry.ConflictWorking,
                                                       entry.ConflictNew,
                                                       entry.ConflictOld,
                                                       item.Path
                                                   };

                    if ( dialog.ShowDialog( context.HostWindow ) != DialogResult.OK )
                        return;
                        
                    selection = dialog.Selection;
                }   
    
                if ( selection != item.Path )
                    this.Copy( item.Path, selection );

                context.Client.Resolved( item.Path, false );
                context.OutputPane.WriteLine( 
                    "Resolved conflicted state of {0}", item.Path );
            }
            else
            {
                string itemPath = Path.GetDirectoryName( item.Path );
                string oldPath = String.Format("\"{0}\"", Path.Combine( itemPath, item.Status.Entry.ConflictOld ));
                string newPath = String.Format("\"{0}\"", Path.Combine( itemPath, item.Status.Entry.ConflictNew ));
                string workingPath = String.Format("\"{0}\"", Path.Combine( itemPath, item.Status.Entry.ConflictWorking ));

                string mergeString = mergeExe;
                mergeString = mergeString.Replace( "%merged", item.Path );
                mergeString = mergeString.Replace( "%base", oldPath );
                mergeString = mergeString.Replace( "%theirs", newPath );
                mergeString = mergeString.Replace( "%mine", workingPath );

                // We can't use System.Diagnostics.Process here because we want to keep the
                // program path and arguments together, which it doesn't allow.
                Utils.Exec exec = new Utils.Exec();
                exec.ExecPath( mergeString );
                exec.WaitForExit();

                if ( MessageBox.Show( "Have all conflicts been resolved?",
                    "Resolve", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
                {
                    context.Client.Resolved( item.Path, false );
                }
            }
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




