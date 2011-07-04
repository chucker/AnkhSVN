﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ankh.Services.PendingChanges {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class PccStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PccStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ankh.Services.PendingChanges.PccStrings", typeof(PccStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only commit from one working copy at a time.
        /// </summary>
        internal static string CommitSingleWc {
            get {
                return ResourceManager.GetString("CommitSingleWc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Committed:.
        /// </summary>
        internal static string CommittedPrefix {
            get {
                return ResourceManager.GetString("CommittedPrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Committing.
        /// </summary>
        internal static string CommitTitle {
            get {
                return ResourceManager.GetString("CommitTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Diffing.
        /// </summary>
        internal static string DiffTitle {
            get {
                return ResourceManager.GetString("DiffTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory deletes must be recursive and this would commit other files:.
        /// </summary>
        internal static string DirectoryDeleteAndNodeToKeep {
            get {
                return ResourceManager.GetString("DirectoryDeleteAndNodeToKeep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleted directory: {0}.
        /// </summary>
        internal static string DirectoryDeleteX {
            get {
                return ResourceManager.GetString("DirectoryDeleteX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory to commit: {0}.
        /// </summary>
        internal static string DirectoryToCommit {
            get {
                return ResourceManager.GetString("DirectoryToCommit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to save open documents before committing.
        /// </summary>
        internal static string FailedToSaveBeforeCommit {
            get {
                return ResourceManager.GetString("FailedToSaveBeforeCommit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Subversion does not allow to commit this combination of files and directories at once..
        /// </summary>
        internal static string InvalidCommitCombination {
            get {
                return ResourceManager.GetString("InvalidCommitCombination", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You haven&apos;t entered an issue number.
        ///Are you sure you want to commit without an issue number?.
        /// </summary>
        internal static string NoIssueNumber {
            get {
                return ResourceManager.GetString("NoIssueNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are trying to commit conflicted items. Please review the conflicted files and resolve them, or remove the check when retrying the commit..
        /// </summary>
        internal static string OneOrMoreItemsConflicted {
            get {
                return ResourceManager.GetString("OneOrMoreItemsConflicted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please update the out of date items and then retry the commit..
        /// </summary>
        internal static string OutOfDateCaption {
            get {
                return ResourceManager.GetString("OutOfDateCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Repository Post-Commit hook Result.
        /// </summary>
        internal static string PostCommitError {
            get {
                return ResourceManager.GetString("PostCommitError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not to commit: {0}.
        /// </summary>
        internal static string ShouldNotCommitX {
            get {
                return ResourceManager.GetString("ShouldNotCommitX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Working Copy: {0}.
        /// </summary>
        internal static string WorkingCopyX {
            get {
                return ResourceManager.GetString("WorkingCopyX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can download the latest AnkhSVN release on http://ankhsvn.net/downloads.
        /// </summary>
        internal static string YouCanDownloadAnkh {
            get {
                return ResourceManager.GetString("YouCanDownloadAnkh", resourceCulture);
            }
        }
    }
}
