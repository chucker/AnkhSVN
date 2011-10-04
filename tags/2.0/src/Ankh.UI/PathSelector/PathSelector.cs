using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SharpSvn;
using System.Collections.Generic;
using Ankh.Scc;

namespace Ankh.UI
{
    public enum PathSelectorOptions
    {
        NoRevision,
        DisplaySingleRevision,
        DisplayRevisionRange
    }
    /// <summary>
    /// Summary description for PathSelector.
    /// </summary>
    public partial class PathSelector : System.Windows.Forms.Form
    {
        PathSelectorInfo _info;
        IAnkhServiceProvider _context;
        PathSelectorOptions _options;
        
        protected PathSelector()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            
            this.Options = PathSelectorOptions.NoRevision;
        }

        int _revStartOffset;
        int _revEndOffset;
        int _suppressOffset;
        int _buttonOffset;
        int _bottomSpacing;

        private void SaveSizes()
        {
            _revStartOffset = revisionStartGroupBox.Top - pathSelectionTreeView.Bottom;
            _revEndOffset = revisionEndGroupBox.Top - revisionStartGroupBox.Bottom;
            _suppressOffset = suppressGroupBox.Top - revisionEndGroupBox.Bottom;
            _buttonOffset = bottomPanel.Top - suppressGroupBox.Bottom;
            _bottomSpacing = ClientSize.Height - bottomPanel.Bottom;
        }

        public PathSelector(PathSelectorInfo info)
            : this()
        {
            _info = info;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                SaveSizes();

                EnsureSelection();
                UpdateLayout();                
            }
        }

        void EnsureSelection()
        {
            EnableRecursive = _info.EnableRecursive;
            Items = _info.VisibleItems;
            //selector.CheckedFilter = _info.CheckedFilter;
            Recursive = _info.Depth == SvnDepth.Infinity;
            SingleSelection = _info.SingleSelection;
            Caption = _info.Caption;

            // do we need go get a revision range?
            if (_info.RevisionStart == null && _info.RevisionEnd == null)
            {
                Options = PathSelectorOptions.NoRevision;
            }
            else if (_info.RevisionEnd == null)
            {
                RevisionStart = _info.RevisionStart;
                Options = PathSelectorOptions.DisplaySingleRevision;
            }
            else
            {
                RevisionStart = _info.RevisionStart;
                RevisionEnd = _info.RevisionEnd;
                Options = PathSelectorOptions.DisplayRevisionRange;
            }
            pathSelectionTreeView.CheckedFilter += _info.EvaluateChecked;
        }

        public IAnkhServiceProvider Context
        {
            get { return _context; }
            set
            {
                if (value != _context)
                {
                    _context = value;
                    OnContextChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnContextChanged(EventArgs eventArgs)
        {
            pathSelectionTreeView.Context = Context;
        }

        /// <summary>
        /// The text to display in the label area.
        /// </summary>
        public string Caption
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// The items to put in the treeview.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<SvnItem> Items
        {
            get { return this.pathSelectionTreeView.Items; }
            set { this.pathSelectionTreeView.Items = value; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public event Predicate<SvnItem> CheckedFilter
        //{
        //    add { pathSelectionTreeView.CheckedFilter += value; }
        //    remove { pathSelectionTreeView.CheckedFilter -= value; }
        //}

        /// <summary>
        /// The items checked in the treeview.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<SvnItem> CheckedItems
        {
            get { return this.pathSelectionTreeView.CheckedItems; }
        }

        /// <summary>
        /// Whether the "Recursive" checkbox should be enabled
        /// </summary>
        public bool EnableRecursive
        {
            get { return this.recursiveCheckBox.Enabled; }
            set { this.recursiveCheckBox.Visible = this.recursiveCheckBox.Enabled = value; }
        }

        /// <summary>
        /// Whether only a single item can be checked.
        /// </summary>
        public bool SingleSelection
        {
            get { return this.pathSelectionTreeView.SingleCheck; }
            set { this.pathSelectionTreeView.SingleCheck = value; }
        }

        /// <summary>
        /// Whether the selection in the treeview is recursive.
        /// </summary>
        public bool Recursive
        {
            get { return this.recursiveCheckBox.Checked; }
            set
            {
                this.recursiveCheckBox.Checked = value;
                this.pathSelectionTreeView.Recursive = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SvnRevision RevisionStart
        {
            get { return this.revisionPickerStart.Revision; }
            set { this.revisionPickerStart.Revision = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SvnRevision RevisionEnd
        {
            get { return this.revisionPickerEnd.Revision; }
            set { this.revisionPickerEnd.Revision = value; }
        }


        public PathSelectorOptions Options
        {
            get { return this._options; }
            set
            {
                _options = value;
                switch (value)
                {
                    case PathSelectorOptions.NoRevision:
                        revisionEndGroupBox.Visible = revisionEndGroupBox.Enabled = false;
                        revisionStartGroupBox.Visible = revisionStartGroupBox.Enabled = false;
                        break;
                    case PathSelectorOptions.DisplaySingleRevision:
                        revisionStartGroupBox.Text = "Revision";
                        revisionStartGroupBox.Visible = revisionStartGroupBox.Enabled = true;
                        revisionEndGroupBox.Visible = revisionEndGroupBox.Enabled = false;
                        break;
                    case PathSelectorOptions.DisplayRevisionRange:
                        revisionStartGroupBox.Visible = revisionStartGroupBox.Enabled = true;
                        revisionStartGroupBox.Text = "Revision start";
                        revisionEndGroupBox.Visible = revisionEndGroupBox.Enabled = true;
                        break;
                    default:
                        throw new ArgumentException("Invalid value for Options");
                }

                if (IsHandleCreated)
                    UpdateLayout();
            }
        }

        void UpdateLayout()
        {
            int y = ClientSize.Height - _bottomSpacing;

            if (bottomPanel.Visible)
            {
                if (y != bottomPanel.Bottom)
                    bottomPanel.Top = y - bottomPanel.Height;

                y = bottomPanel.Top - _bottomSpacing;
            }

            if (suppressGroupBox.Visible)
            {
                if (y != suppressGroupBox.Bottom)
                    suppressGroupBox.Top = y - suppressGroupBox.Height;

                y = suppressGroupBox.Top - _suppressOffset;
            }

            if (revisionEndGroupBox.Visible)
            {
                if (y != revisionEndGroupBox.Bottom)
                    revisionEndGroupBox.Top = y - revisionEndGroupBox.Height;

                y = revisionEndGroupBox.Top - _revEndOffset;
            }

            if (revisionStartGroupBox.Visible)
            {
                if (y != revisionStartGroupBox.Bottom)
                    revisionStartGroupBox.Top = y - revisionStartGroupBox.Height;

                y = revisionStartGroupBox.Top - _revStartOffset;
            }

            y -= _revStartOffset;

            if (y != pathSelectionTreeView.Bottom)
            {
                int n = pathSelectionTreeView.Bottom - y;
                pathSelectionTreeView.Height -= n;                

                if (n < 0)
                {
                    Height += n;
                }
            }

            int nv = pathSelectionTreeView.VisibleCount;

            if(nv > 5 && nv > _info.VisibleItems.Count * 2)
            {
                int height = (pathSelectionTreeView.Height * 3) / 2  / nv;

                height = Math.Max(5, _info.VisibleItems.Count+3) * height;

                if(height < pathSelectionTreeView.Height)
                    Height -= pathSelectionTreeView.Height - height;
            }
        }

        protected PathSelectionTreeView TreeView
        {
            get { return this.pathSelectionTreeView; }
        }

        protected Button OkButton
        {
            get { return this.okButton; }
        }

        protected Button DoCancelButton
        {
            get { return this.cancelButton; }
        }

        protected RevisionPicker RevisionPickerStart
        {
            get { return this.revisionPickerStart; }
        }

        protected RevisionPicker RevisionPickerEnd
        {
            get { return this.revisionPickerEnd; }
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void RecursiveCheckedChanged(object sender, System.EventArgs e)
        {
            this.pathSelectionTreeView.Recursive = this.recursiveCheckBox.Checked;
        }        
    }
}