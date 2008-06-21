﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ankh.UI
{
    partial class LockDialog
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.suppressLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.stealLocksCheckBox = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pathSelectionTreeView = new Ankh.UI.PathSelectionTreeView();
            this.logMessageEditor = new Ankh.UI.PendingChanges.LogMessageEditor();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // suppressLabel
            // 
            this.suppressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.suppressLabel.Location = new System.Drawing.Point(12, 329);
            this.suppressLabel.Name = "suppressLabel";
            this.suppressLabel.Size = new System.Drawing.Size(352, 20);
            this.suppressLabel.TabIndex = 0;
            this.suppressLabel.Text = "You can suppress this dialog by holding down the Shift key";
            this.suppressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lock message:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(289, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(208, 362);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 24);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            // 
            // stealLocksCheckBox
            // 
            this.stealLocksCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stealLocksCheckBox.AutoSize = true;
            this.stealLocksCheckBox.Location = new System.Drawing.Point(12, 362);
            this.stealLocksCheckBox.Name = "stealLocksCheckBox";
            this.stealLocksCheckBox.Size = new System.Drawing.Size(82, 17);
            this.stealLocksCheckBox.TabIndex = 2;
            this.stealLocksCheckBox.Text = "&Steal Locks";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 9);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pathSelectionTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.logMessageEditor);
            this.splitContainer1.Size = new System.Drawing.Size(352, 315);
            this.splitContainer1.SplitterDistance = 157;
            this.splitContainer1.TabIndex = 0;
            // 
            // pathSelectionTreeView
            // 
            this.pathSelectionTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pathSelectionTreeView.CheckBoxes = true;
            this.pathSelectionTreeView.Context = null;
            this.pathSelectionTreeView.Location = new System.Drawing.Point(0, 0);
            this.pathSelectionTreeView.Name = "pathSelectionTreeView";
            this.pathSelectionTreeView.Recursive = false;
            this.pathSelectionTreeView.SingleCheck = false;
            this.pathSelectionTreeView.Size = new System.Drawing.Size(352, 154);
            this.pathSelectionTreeView.TabIndex = 0;
            this.pathSelectionTreeView.UrlPaths = false;
            this.pathSelectionTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.pathSelectionTreeView_AfterCheck);
            // 
            // logMessageEditor
            // 
            this.logMessageEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.logMessageEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logMessageEditor.Location = new System.Drawing.Point(0, 25);
            this.logMessageEditor.Name = "logMessageEditor";
            this.logMessageEditor.Size = new System.Drawing.Size(352, 129);
            this.logMessageEditor.TabIndex = 1;
            this.logMessageEditor.Text = null;
            // 
            // LockDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(373, 398);
            this.Controls.Add(this.suppressLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.stealLocksCheckBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.btnCancel);
            this.MinimumSize = new System.Drawing.Size(100, 36);
            this.Name = "LockDialog";
            this.Text = "Lock";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        protected Ankh.UI.PathSelectionTreeView pathSelectionTreeView;

        protected System.Windows.Forms.Label suppressLabel;

        private Ankh.UI.PendingChanges.LogMessageEditor logMessageEditor;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox stealLocksCheckBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
