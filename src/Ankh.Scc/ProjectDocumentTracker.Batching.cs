﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio;

namespace Ankh.Scc
{
    partial class ProjectDocumentTracker
    {
        bool _batchOk;
        bool _inBatch;
        /// <summary>
        /// Indicates that a project is about start a batch query process.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"></see>. If it fails, it returns an error code.
        /// </returns>
        public int OnBeginQueryBatch()
        {
            _inBatch = _batchOk = true;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// This method is called to indicate that a batch query process has been canceled.
        /// </summary>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"></see>. If it fails, it returns an error code.
        /// </returns>
        public int OnCancelQueryBatch()
        {
            _inBatch = _batchOk = false;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Determines whether it is okay to proceed with the actual batch operation after successful completion of a batch query process.
        /// </summary>
        /// <param name="pfActionOK">[out] Returns nonzero if it is okay to continue with the proposed batch process. Returns zero if the proposed batch process should not proceed.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"></see>. If it fails, it returns an error code.
        /// </returns>
        public int OnEndQueryBatch(out int pfActionOK)
        {
            pfActionOK = _inBatch && _batchOk ? 1 : 0;
            _inBatch = _batchOk = false;
            return VSConstants.S_OK;
        }
    }
}
