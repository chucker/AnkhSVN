// $Id$
using System;
using EnvDTE;
using System.Collections;


namespace Ankh.Commands
{
    /// <summary>
    /// Cleans up a working copy directory.
    /// </summary>
    [VSNetCommand( "Cleanup", Text="Cleanup", Tooltip = "Cleans up the working copy", 
         Bitmap = ResourceBitmaps.Cleanup ),
   VSNetItemControl( "Ankh", Position = 1 )]
    public class Cleanup : CommandBase
    {
        public Cleanup(IServiceProvider serviceProvider)
            :base(serviceProvider)
        { }
    
        public override EnvDTE.vsCommandStatus QueryStatus(IContext context)
        {
            return Enabled;
        }
    
        public override void Execute(IContext context, string parameters)
        {
            using (OperationManager.RunOperation("Running cleanup"))
            {
                IList resources = context.Selection.GetSelectionResources(false,
                    new ResourceFilterCallback(SvnItem.DirectoryFilter));
                foreach (SvnItem item in resources)
                    context.Client.Cleanup(item.Path);
            }
        }
    }
}



