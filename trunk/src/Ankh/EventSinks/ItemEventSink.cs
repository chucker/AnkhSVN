// $Id$
using System;
using NSvn;
using EnvDTE;
using System.IO;

namespace Ankh.EventSinks
{
    /// <summary>
    /// Base class serving as event sink for all item related events.
    /// </summary>
    internal abstract class ItemEventSink : EventSink
    {
        protected ItemEventSink( AnkhContext context ) : base( context )
        {
            // empty
        }
     
 

        protected void ItemAdded( ProjectItem item )
        {
            try
            {
                this.Context.SolutionExplorer.Refresh( item.ContainingProject );
            }
            catch( Exception ex )
            {
                Error.Handle( ex );
                throw;
            }
        }

        /// <summary>
        /// Schedules a Projectitem for removal on commit.
        /// </summary>
        /// <param name="item">Projectitem to be scheduled for removal.</param>
        protected void ItemRemoved( ProjectItem item )
        {
            try
            {
                //this.Context.OutputPane.StartActionText( "Delete" );
                this.Context.SolutionExplorer.VisitResources( 
                    item, new RemoveProjectVisitor(), false );
                this.Context.SolutionExplorer.Refresh ( item.ContainingProject );
                //this.Context.OutputPane.EndActionText();

            }
            catch ( Exception ex )
            {
                Error.Handle( ex );
                throw;
            }
        }

        protected void ItemRenamed( ProjectItem item, string oldName )
        {
            try
            {
                // rename on a folder will call rename on all subitems.
                if ( item.Name == oldName )
                    return;

                // assume there is only one filename
                string newPath = item.get_FileNames(1);
                RenameVisitor v = new RenameVisitor( oldName, newPath );
                this.Context.SolutionExplorer.VisitResources( item, v, false );

                // we need to refresh the parents, since the actual treenode is replaced.
               this.Context.SolutionExplorer.RefreshSelectionParents();
            }
            catch( Exception ex )
            {
                Error.Handle( ex );
                throw;
            }
        }

        /// <summary>
        /// A visitor that schedules a remove of visited item on commit
        /// </summary>
        private class RemoveProjectVisitor : LocalResourceVisitorBase
        {
            public override void VisitWorkingCopyResource(NSvn.WorkingCopyResource resource)
            {
                // Checks if file doesn't exists. 
                if ( !File.Exists( resource.Path ) )
                {
                    resource.Remove( true );
                }
            }
        }

        /// <summary>
        /// A visitor that renames an item.
        /// </summary>
        private class RenameVisitor : LocalResourceVisitorBase
        {
            public RenameVisitor( string oldName, string newPath )
            {
                this.oldName = oldName; 
                this.newPath = newPath;
            }

            public override void VisitWorkingCopyFile(WorkingCopyFile file)
            {

                // we need to rename the file back to  its original name
                string dir = Path.GetDirectoryName( file.Path );
                string oldPath = Path.Combine( dir, oldName );
                File.Move( this.newPath, oldPath );

                // now have SVN rename it.
                file.Move( this.newPath, true );
            }

            public override void VisitWorkingCopyDirectory(WorkingCopyDirectory dir)
            {
                // strip off the trailing \ if necessary
                string dirNoTrailing = dir.Path[ dir.Path.Length - 1 ] == '\\' ? dir.Path.Substring( 0, dir.Path.Length-1 ) : 
                    dir.Path;
                string parentDir = Path.GetDirectoryName( dirNoTrailing );
                string oldPath = Path.Combine( parentDir, oldName );
                Directory.Move( this.newPath, oldPath );

                dir.Move( this.newPath, true );
            }



            private string oldName;
            private string newPath;

        }
    }
}
