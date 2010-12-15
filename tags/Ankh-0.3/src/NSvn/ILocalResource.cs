// $Id$
using System;
using NSvn.Core;

namespace NSvn
{
    /// <summary>
    /// TODO: doc-comment here
    /// </summary>
    public interface ILocalResource
    {
        /// <summary>
        /// Accepts an ILocalResourceVisitor.
        /// </summary>
        /// <param name="visitor"></param>
        void Accept( ILocalResourceVisitor visitor );

        /// <summary>
        /// The file system path to the item.
        /// </summary>
        string Path
        { 
            get;
        }

        /// <summary>
        /// Is this a directory?
        /// </summary>
        bool IsDirectory
        {
            get;
        }

        /// <summary>
        /// Is this a versioned item?
        /// </summary>
        bool IsVersioned
        {
            get;
        }

        /// <summary>
        /// The status of this item.
        /// </summary>
        Status Status
        {
            get;
        }

        
        /// <summary>
        /// The context object used in version control operations.
        /// </summary>
        NSvnContext Context
        {
            get;
            set;
        }
    }
}