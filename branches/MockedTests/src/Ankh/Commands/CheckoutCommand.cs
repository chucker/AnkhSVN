// $Id$
using System;
using Ankh.UI;
using System.Windows.Forms;
using Utils;
using NSvn.Common;

namespace Ankh.Commands
{
    /// <summary>
    /// A command that lets you check out a repository directory.
    /// </summary>
    [VSNetCommand("Checkout", Tooltip="Checkout a repository directory", 
         Text = "Checkout a repository directory...", 
         Bitmap = ResourceBitmaps.CheckoutDirectory),
    VSNetControl( "Tools.AnkhSVN", Position = 1 ) ]
    public class CheckoutCommand : CommandBase
    {
        public CheckoutCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override EnvDTE.vsCommandStatus QueryStatus(IContext context)
        {
            return Enabled;
        }

        public override void Execute(IContext context, string parameters)
        {
            using(CheckoutDialog dlg = new CheckoutDialog())
            {
                if ( dlg.ShowDialog( context.HostWindow ) != DialogResult.OK )
                    return;

                using(OperationManager.RunOperation( "Checking out" ))
                {
                    CheckoutRunner runner = new CheckoutRunner(
                        dlg.LocalPath, dlg.Revision, dlg.Url, dlg.Recursive ? Recurse.Full : Recurse.None );
                    context.UIShell.RunWithProgressDialog( runner, "Checking out" );

                    // make sure it's remembered
                    RegistryUtils.CreateNewTypedUrl( dlg.Url );
                }
            }
        }
    }
}
