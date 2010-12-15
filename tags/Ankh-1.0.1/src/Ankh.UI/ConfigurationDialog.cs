using System;
using System.Text;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Utils;

namespace Ankh.UI
{
    /// <summary>
    /// Configuration dialog for Ankh.
    /// </summary>
    public class ConfigurationDialog : Form
    {
        /// <summary>
        /// Constructor, who constructs the object which is constructed in the constructor.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        public ConfigurationDialog(object configuration)
        {
            this.InitializeComponent();

            this.propertyGrid.SelectedObject = configuration;
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.Panel panel1;
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add( this.okButton );
            panel1.Controls.Add( this.cancelButton );
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point( 0, 541 );
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size( 557, 32 );
            panel1.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.okButton.Location = new System.Drawing.Point( 404, 3 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 26 );
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelButton.Location = new System.Drawing.Point( 479, 3 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 26 );
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            // 
            // propertyGrid
            // 
            this.propertyGrid.CommandsVisibleIfAvailable = false;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point( 0, 0 );
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size( 557, 541 );
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // ConfigurationDialog
            // 
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 557, 573 );
            this.Controls.Add( this.propertyGrid );
            this.Controls.Add( panel1 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AnkhSVN Configuration";
            panel1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        private Button cancelButton;
        private Button okButton;
        private PropertyGrid propertyGrid;

        [STAThread]
        static void Main( string[] args )
        {
        //    object config = new Config();
        //    do
        //    {
        //        ConfigurationDialog dlg = new ConfigurationDialog( config );
        //        dlg.ShowInTaskbar = true;
        //        dlg.ShowDialog();

        //        foreach ( PropertyDescriptor desc in TypeDescriptor.GetProperties( config ) )
        //        {
        //            object val = desc.GetValue( config );
        //            Console.WriteLine( "{0}: '{1}'", desc.DisplayName, val == null ? "{null}" : val );
        //        }
        //        Console.WriteLine();
        //    } while ( Console.ReadKey(true).Key != ConsoleKey.Q );
        }
    }

    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:2.0.50727.42
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    // 
    // This source code was auto-generated by xsd, Version=2.0.50727.42.
    // 
    



        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute( "code" )]
        [System.Xml.Serialization.XmlRootAttribute( Namespace = "http://ankhsvn.com/Config.xsd", IsNullable = false )]
        public class Config
        {

            private ConfigRepositoryExplorer repositoryExplorerField;

            private string mergeExePathField;

            private string diffExePathField;

            private string logMessageTemplateField = "";

            private bool autoAddNewFilesField = true;

            private bool autoReuseCommentField = false;

            private bool chooseDiffMergeManualField = false;

            private bool disableSolutionReloadField = false;

            private ConfigSubversion subversionField;

            /// <remarks/>
            [TypeConverter( typeof( ExpandableObjectConverter ) )]
            [Browsable( false )]
            public ConfigRepositoryExplorer RepositoryExplorer
            {
                get
                {
                    return this.repositoryExplorerField;
                }
                set
                {
                    this.repositoryExplorerField = value;
                }
            }

            /// <remarks/>
            //[Editor( typeof( MultilineStringEditor ), typeof( UITypeEditor ) )]
            [Category( "Diff/Merge" )]
            [DefaultValue( null )]
            [Description( @"This command line will be used for spawning an external merge program. " +
    "The options are %base %theirs %mine %merged, which will be replaced with the respective paths when a merge is executed." )]
            [TypeConverter( typeof( NullableStringTypeConverter ) )]
            [Editor(typeof(Ankh.UI.Config.MergeExeTypeEditor), typeof(UITypeEditor))]
            public string MergeExePath
            {
                get
                {
                    return this.mergeExePathField;
                }
                set
                {
                    this.mergeExePathField = value;
                }
            }

            private class MergeExeTypeEditor : StringTypeEditorWithTemplates
            {
                public MergeExeTypeEditor()
                {
                    this.SetTitle( "MergeExe" );
                }

                protected override StringEditorTemplate[] GetTemplates()
                {
                    return new StringEditorTemplate[]{
                        new StringEditorTemplate("%mine", "My version", "My version (%mine)"),
                        new StringEditorTemplate("%base", "The base version", "The base version (%base)"),
                        new StringEditorTemplate("%yours", "The other version", "The other version (%yours)"),
                        new StringEditorTemplate("%merged", "The output file", "The output file (%merged)")
                        }; 
                }
            }

            /// <remarks/>
            //[Editor( typeof( MultilineStringEditor ), typeof( UITypeEditor ) )]
            [DefaultValue( null )]
            [Category( "Diff/Merge" )]
            [Description( @"This command line will be used for spawning an external diff program. " +
                "The options are %base and %mine, which will be replaced with the respective paths when a diff is executed." )]
            [TypeConverter(typeof(NullableStringTypeConverter))]
            [Editor(typeof(Ankh.UI.Config.DiffExeTypeEditor), typeof(UITypeEditor))]
            public string DiffExePath
            {
                get
                {
                    return this.diffExePathField;
                }
                set
                {
                    this.diffExePathField = value;
                }
            }

            private class DiffExeTypeEditor : StringTypeEditorWithTemplates
            {
                public DiffExeTypeEditor()
                {
                    this.SetTitle( "DiffExe" );
                }

                protected override StringEditorTemplate[] GetTemplates()
                {
                    return new StringEditorTemplate[]{
                        new StringEditorTemplate("%base", "The base version", "The base version (%base)"),
                        new StringEditorTemplate("%mine", "My version", "My version (%mine)"),
                        };
                }
            }


            /// <remarks/>
            //[Editor( typeof( MultilineStringEditor ), typeof( UITypeEditor ) )]
            [Category( "Log message" )]
            [DefaultValue("")]
            [Editor(typeof(Ankh.UI.Config.LogMessageTypeEditor), typeof(UITypeEditor))]
            public string LogMessageTemplate
            {
                get
                {
                    return this.logMessageTemplateField;
                }
                set
                {
                    this.logMessageTemplateField = value;
                }
            }

            private class LogMessageTypeEditor : StringTypeEditorWithTemplates
            {
                public LogMessageTypeEditor()
                {
                    this.SetTitle( "LogMessage" );
                }

                protected override StringEditorTemplate[] GetTemplates()
                {
                    return new StringEditorTemplate[]{
                        new StringEditorTemplate("#", "Comment", "Comment (#)"),
                        new StringEditorTemplate("***", "For each file", "For each file (***)"),
                        new StringEditorTemplate("%path%", "The file path", "The file path (%path%)")
                    };
                }
            }

            /// <remarks/>
            [DefaultValue( true )]
            [Description( @"Whether new files should be automatically added to Subversion." )]
            public bool AutoAddNewFiles
            {
                get
                {
                    return this.autoAddNewFilesField;
                }
                set
                {
                    this.autoAddNewFilesField = value;
                }
            }

            /// <remarks/>
            [DefaultValue( false )]
            [Category( "Log message" )]
            [Description( @"Whether to automatically reuse the last comment if a commit failed." )]
            public bool AutoReuseComment
            {
                get
                {
                    return this.autoReuseCommentField;
                }
                set
                {
                    this.autoReuseCommentField = value;
                }
            }
            /// <remarks/>
            [DefaultValue( false )]
            [Category( "Diff/Merge" )]
            public bool ChooseDiffMergeManual
            {
                get
                {
                    return this.chooseDiffMergeManualField;
                }
                set
                {
                    this.chooseDiffMergeManualField = value;
                }
            }

            /// <remarks/>
            [DefaultValue( false )]
            [Description( @"Whether AnkhSVN should offer to automatically reload a solution if a project file is modified." )]
            public bool DisableSolutionReload
            {
                get
                {
                    return this.disableSolutionReloadField;
                }
                set
                {
                    this.disableSolutionReloadField = value;
                }
            }

            /// <remarks/>
            [Browsable( false )]
            public ConfigSubversion Subversion
            {
                get
                {
                    if ( this.subversionField == null )
                    {
                        this.subversionField = new ConfigSubversion();
                    }
                    return this.subversionField;
                }
                set
                {
                    this.subversionField = value;
                }
            }

            [Category( "Subversion" )]
            [XmlIgnore]
            [Description( @"The path to the directory to use for the Subversion configuration data. " +
                "If not specified, it will use the same config area as other Subversion tools, " +
                "usually %APPDATA%\\Subversion. You can use environment variables in the path, quoted with %." )]
            [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
            [TypeConverter( typeof( NullableStringTypeConverter ) )]
            [DefaultValue((string)null)]
            public string ConfigDir
            {
                get { return this.Subversion.ConfigDir; }
                set { this.Subversion.ConfigDir = value; }
            }

            [Category( "Subversion" )]
            [XmlIgnore]
            [Description( @"This path will be used for spawning svn.exe through the public " +
    "svn command in the VS.NET command window. It is optional. " +
    "If left out, Ankh will attempt to spawn svn.exe from PATH." )]
            [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
            [DefaultValue((string)null)]
            [TypeConverter( typeof( NullableStringTypeConverter ) )]
            public string SvnExePath
            {
                get { return this.Subversion.SvnExePath; }
                set { this.Subversion.SvnExePath = value; }
            }

            [Category( "Subversion" )]
            [XmlIgnore]
            [Description( @"This is the name of the administrative subdirectory used by " +
    "Subversion to maintain metadata for the working copy. You should " +
    "*ONLY* modify this if you know what you are doing and you are " +
    "experiencing specific problems with Subversion's use of \".svn\" for " +
    @"the meta-data directory. 
Since Subversion 1.3, there is an environment variable (called SVN_ASP_DOT_NET_HACK)" +
    "you can set in order to switch *ALL* svn clients to the _svn directory. Since that release " +
    "the AdminDirectoryName serves as an override (if set), and may *only* contain _svn or .svn." )]
            [TypeConverter(typeof(Ankh.UI.Config.AdminDirectoryNameTypeConverter))]
            [DefaultValue((string)null)]
            public string AdminDirectoryName
            {
                get { return this.Subversion.AdminDirectoryName; }
                set 
                { 
                    Console.WriteLine( "AdminDirectoryName set to '{0}'", value != null ? value : "{null}");
                    this.Subversion.AdminDirectoryName = value; 
                }
            }

            private class AdminDirectoryNameTypeConverter : StandardStringsTypeConverter
            {
                public AdminDirectoryNameTypeConverter() : base(new string[]{".svn", "_svn", null}, true)
                {
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute( "code" )]
        public class ConfigRepositoryExplorer
        {

            private ConfigRepositoryExplorerUrl[] mruUrlsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute( "Url" )]
            public ConfigRepositoryExplorerUrl[] MruUrls
            {
                get
                {
                    return this.mruUrlsField;
                }
                set
                {
                    this.mruUrlsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute( "code" )]
        public class ConfigRepositoryExplorerUrl
        {

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute( "code" )]
        public class ConfigSubversion
        {

            private string configDirField;

            private string svnExePathField;

            private string adminDirectoryNameField;

            /// <remarks/>
            [DefaultValue( null )]
            public string ConfigDir
            {
                get
                {
                    return this.configDirField;
                }
                set
                {
                    this.configDirField = value;
                }
            }

            /// <remarks/>
            [DefaultValue( null )]
            public string SvnExePath
            {
                get
                {
                    return this.svnExePathField;
                }
                set
                {
                    this.svnExePathField = value;
                }
            }

            /// <remarks/>
            [DefaultValue( null )]
            public string AdminDirectoryName
            {
                get
                {
                    return this.adminDirectoryNameField;
                }
                set
                {
                    this.adminDirectoryNameField = value;
                }
            }
        }

}