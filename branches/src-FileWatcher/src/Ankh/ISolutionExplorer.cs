// $Id$
using EnvDTE;
using Ankh.Solution;
using System.Collections;

namespace Ankh
{
    public interface ISolutionExplorer : ISelectionContainer
    {
        /// <summary>
        /// Refreshes all subnodes of a specific project.
        /// </summary>
        /// <param name="project"></param>
        void Refresh( Project project );

        /// <summary>
        /// Updates the status of the given item.
        /// </summary>
        /// <param name="item"></param>
        void Refresh( ProjectItem item );

        /// <summary>
        /// Removes the project from Ankh's control.
        /// </summary>
        /// <param name="project"></param>
        void RemoveProject( Project project );

        /// <summary>
        /// Retrieves the resources associated with a project item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        IList GetItemResources( ProjectItem item, bool recursive );

        /// <summary>	 	
        /// Visits all the selected nodes.	 	
        /// </summary>	 	
        /// <param name="visitor"></param>	 	
        void VisitSelectedNodes( INodeVisitor visitor );

        /// <summary>
        /// Returns the selected ProjectItem
        /// </summary>
        /// <returns></returns>
        ProjectItem GetSelectedProjectItem();

        /// <summary>
        /// Notify the Solution Explorer to unload.
        /// </summary>
        void Unload();

        /// <summary>
        /// Whether a rename is currently in progress.
        /// </summary>
        bool RenameInProgress { get; }

        /// <summary>
        /// Set up a refresh for the project after a delay.
        /// </summary>
        /// <param name="project"></param>
        void SetUpDelayedProjectRefresh( Project project );

        /// <summary>
        /// Set up a refresh for the entire solution after a delay.
        /// </summary>
        void SetUpDelayedSolutionRefresh();
    }
}