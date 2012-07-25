// $Id$
//
// Copyright 2008-2009 The AnkhSVN Project
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

using Ankh.Commands;
using Ankh.Scc.ProjectMap;
using Ankh.Selection;
using System.IO;
using SharpSvn;

namespace Ankh.Scc
{
    partial class AnkhSccProvider
    {
        readonly Dictionary<IVsSccProject2, SccProjectData> _projectMap = new Dictionary<IVsSccProject2, SccProjectData>();
        bool _managedSolution;
        HybridCollection<string> _delayedDelete;
        bool _isDirty;

        public bool IsSolutionManaged
        {
            get { return _managedSolution; }
        }

        public void LoadingManagedSolution(bool asPrimarySccProvider)
        {
            // Called by the package when a solution is loaded which is marked as managed by us
            _managedSolution = asPrimarySccProvider;
        }

        public bool IsProjectManaged(SvnProject project)
        {
            if (!IsActive)
                return false;

            if (project == null)
                return IsSolutionManaged;

            return IsProjectManagedRaw(project.RawHandle);
        }

        public bool IsProjectManagedRaw(object project)
        {
            if (!IsActive)
                return false;

            if (project == null)
                return IsSolutionManaged;

            IVsSccProject2 sccProject = project as IVsSccProject2;

            if (sccProject == null)
                return false;

            SccProjectData data;

            if (_projectMap.TryGetValue(sccProject, out data))
                return data.IsManaged;

            return false;
        }

        public void SetProjectManaged(SvnProject project, bool managed)
        {
            if (!IsActive)
                return; // Perhaps allow clearing management settings?

            if (project == null)
                SetProjectManagedRaw(null, managed);
            else
                SetProjectManagedRaw(project.RawHandle, managed);
        }

        public void EnsureCheckOutReference(SvnProject project)
        {
            // NOOP for today
        }

        public void SetProjectManagedRaw(object project, bool managed)
        {
            if (!IsActive)
                return;

            if (project == null)
            {
                // We are talking about the solution

                if (managed != IsSolutionManaged)
                {
                    _managedSolution = managed;
                    IsSolutionDirty = true;

                    foreach (SccProjectData p in _projectMap.Values)
                    {
                        if (p.IsSolutionFolder || p.IsWebSite)
                        {
                            p.SetManaged(managed);

                            if (managed)
                                p.NotifyGlyphsChanged();
                        }
                    }
                }
                return;
            }

            IVsSccProject2 sccProject = project as IVsSccProject2;

            if (sccProject == null)
            {
                if (project is IVsSolution)
                    SetProjectManagedRaw(null, managed);

                return;
            }

            SccProjectData data;

            if (!_projectMap.TryGetValue(sccProject, out data))
                return;

            if (managed == data.IsManaged)
                return; // Nothing to change

            data.SetManaged(managed);
        }

        internal SccProjectData GetSccProject(Guid projectId)
        {
            foreach (SccProjectData pd in _projectMap.Values)
            {
                if (pd.ProjectGuid == projectId)
                    return pd;
            }

            return null;
        }

        public bool IsSolutionDirty
        {
            // TODO: Only return true if the solution was not previously managed by Ankh
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        public bool IsSolutionLoaded
        {
            get { return _solutionLoaded; }
        }

        /// <summary>
        /// Called by ProjectDocumentTracker when a solution is opened 
        /// </summary>
        internal void OnSolutionOpened(bool onLoad)
        {
            _solutionLoaded = true;
            _solutionFile = _solutionDirectory = null;

            if (!IsActive)
            {
                IAnkhCommandStates states = GetService<IAnkhCommandStates>();

                if (states == null || states.OtherSccProviderActive)
                    return;
            }

            if (!IsSolutionManaged)
            {
                string dir = SolutionDirectory;

                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    if (!SvnTools.IsBelowManagedPath(dir))
                        return; // Not for us

                    if (!IsActive)
                        RegisterAsPrimarySccProvider(); // Set us active; we know there is a .svn

                    // BH: Many users seem to have .load and .noload files checked in
                    // so we can't just remove them without issues.
                }
            }

            if (!IsActive)
                return;

            foreach (SccProjectData data in _projectMap.Values)
            {
                if (data.IsSolutionFolder)
                {
                    // Solution folders don't save their Scc management state
                    // We let them follow the solution settings

                    if (IsSolutionManaged)
                        data.SetManaged(true);
                }

                if (data.IsSolutionFolder || data.IsWebSite)
                {
                    // Flush the glyph cache of solution folders
                    // (Well known VS bug: Initially clear)
                    data.NotifyGlyphsChanged();
                }
            }

            IPendingChangesManager mgr = GetService<IPendingChangesManager>();
            if (mgr != null && mgr.IsActive)
                mgr.FullRefresh(false);

            UpdateSolutionGlyph();
        }

        string _solutionFile;
        string _solutionDirectory;
        public string SolutionFilename
        {
            get
            {
                if (_solutionFile == null)
                    LoadSolutionInfo();

                return _solutionFile.Length > 0 ? _solutionFile : null;
            }
        }

        public string SolutionDirectory
        {
            get
            {
                if (_solutionDirectory == null)
                    LoadSolutionInfo();

                return _solutionDirectory;
            }
        }

        void LoadSolutionInfo()
        {
            string dir, path, user;

            IVsSolution sol = GetService<IVsSolution>(typeof(SVsSolution));

            if (sol == null ||
                !ErrorHandler.Succeeded(sol.GetSolutionInfo(out dir, out path, out user)))
            {
                _solutionDirectory = _solutionFile = "";
                return;
            }

            if (string.IsNullOrEmpty(dir) || string.IsNullOrEmpty(path))
            {
                // Cache negative result; will be returned as null
                _solutionDirectory = _solutionFile = "";
            }
            else
            {
                if (SvnItem.IsValidPath(dir))
                    _solutionDirectory = SvnTools.GetTruePath(dir, true) ?? SvnTools.GetNormalizedFullPath(dir);
                else
                    _solutionDirectory = "";

                if (SvnItem.IsValidPath(path))
                    _solutionFile = SvnTools.GetTruePath(path, true) ?? SvnTools.GetNormalizedFullPath(path);
                else
                    _solutionFile = "";
            }
        }

        /// <summary>
        /// Called by ProjectDocumentTracker just before a solution is closed
        /// </summary>
        /// <remarks>At this time the closing can not be canceled.</remarks>
        internal void OnStartedSolutionClose()
        {
            foreach (SccProjectData pd in _projectMap.Values)
            {
                pd.Dispose();
            }

#if !DEBUG
            // Skip file by file cleanup of the project<-> file mapping
            // Should proably always be enabled around the release of AnkhSVN 2.0
            _projectMap.Clear();
            _fileMap.Clear();
#endif
        }

        /// <summary>
        /// Called by ProjectDocumentTracker when a solution is closed
        /// </summary>
        internal void OnSolutionClosed()
        {
            Debug.Assert(_projectMap.Count == 0);
            Debug.Assert(_fileMap.Count == 0);

            _solutionFile = _solutionDirectory = null;
            _projectMap.Clear();
            _fileMap.Clear();
            _unreloadable.Clear();
            StatusCache.ClearCache();

            // Clear status for reopening solution
            _managedSolution = false;
            _isDirty = false;
            _solutionLoaded = false;
            _sccExcluded.Clear();
            Translate_ClearState();

            IPendingChangesManager mgr = GetService<IPendingChangesManager>();
            if (mgr != null)
                mgr.Clear();
        }

        /// <summary>
        /// Called by ProjectDocumentTracker when a scc-capable project is loaded
        /// </summary>
        /// <param name="project"></param>
        internal void OnProjectLoaded(IVsSccProject2 project)
        {
            if (!_projectMap.ContainsKey(project))
                _projectMap.Add(project, new SccProjectData(Context, project));
        }

        internal void OnProjectRenamed(IVsSccProject2 project)
        {
            if (string.IsNullOrEmpty(SolutionFilename))
                return;

            SccProjectData data;
            if (!_projectMap.TryGetValue(project, out data))
                return;

            string oldLocation = data.ProjectLocation;
            try
            {
                using (SccProjectData newData = new SccProjectData(Context, project))
                {
                    string newLocation = newData.ProjectLocation;

                    if (newLocation == null && oldLocation == null)
                        return; // No need to do anything for this case (e.g. solution folders)

                    SccStore.OnProjectRenamed(oldLocation, newLocation);
                }
            }
            finally
            {
                // Mark the sln file edited, so it shows up in Pending Changes/Commit
                if (!string.IsNullOrEmpty(SolutionFilename))
                    DocumentTracker.CheckDirty(SolutionFilename);

                data.Reload(); // Reload project name, etc.
            }
        }

        /// <summary>
        /// Called by ProjectDocumentTracker when a scc-capable project is opened
        /// </summary>
        /// <param name="project">The loaded project</param>
        /// <param name="added">The project was added after opening</param>
        internal void OnProjectOpened(IVsSccProject2 project, bool added)
        {
            SccProjectData data;
            if (!_projectMap.TryGetValue(project, out data))
                _projectMap.Add(project, data = new SccProjectData(Context, project));

            if (data.IsSolutionFolder || data.IsWebSite)
            {
                if (IsSolutionManaged)
                {
                    // We let them follow the solution settings (See OnSolutionOpen() for the not added case
                    if (added && data.IsSolutionFolder)
                        data.SetManaged(true);
                }

                // Solution folders are projects without Scc state
                data.NotifyGlyphsChanged();
            }

            data.OnOpened();
            _syncMap = true;

            // Don't take the focus from naming the folder. The rename will perform the .Load()
            // and dirty check
            if (added && data.IsSolutionFolder)
                return;

            if (added && !string.IsNullOrEmpty(SolutionFilename))
            {
                DocumentTracker.CheckDirty(SolutionFilename);
            }

            RegisterForSccCleanup();
        }

        /// <summary>
        /// Called when a project is explicitly refreshed
        /// </summary>
        /// <param name="project"></param>
        internal void RefreshProject(IVsSccProject2 project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            SccProjectData data;
            if (_projectMap.TryGetValue(project, out data))
            {
                data.Reload();
            }
        }

        internal bool TrackProjectChanges(IVsSccProject2 project)
        {
            bool trackCopies;

            return TrackProjectChanges(project, out trackCopies);
        }

        IVsSolutionBuildManager2 _buildManager;
        IVsSolutionBuildManager2 BuildManager
        {
            get { return _buildManager ?? (_buildManager = GetService<IVsSolutionBuildManager2>(typeof(SVsSolutionBuildManager))); }
        }

        internal bool TrackProjectChanges(IVsSccProject2 project, out bool trackCopies)
        {
            // We can be called with a null project
            SccProjectData data;

            if (project != null && _projectMap.TryGetValue(project, out data))
            {
                trackCopies = true;

                if (data.IsWebSite)
                {
                    int busy;
                    if (BuildManager != null &&
                        ErrorHandler.Succeeded(BuildManager.QueryBuildManagerBusy(out busy)) &&
                        busy != 0)
                    {
                        trackCopies = false;
                    }
                }
                else if (_syncMap)
                    data.Load();


                return data.TrackProjectChanges(); // Allows temporary disabling changes
            }

            trackCopies = false;
            return false;
        }

        /// <summary>
        /// Called by ProjectDocumentTracker when a scc-capable project is closed
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="removed">if set to <c>true</c> the project is being removed or unloaded from the solution.</param>
        internal void OnProjectClosed(IVsSccProject2 project, bool removed)
        {
            SccProjectData data;
            if (_projectMap.TryGetValue(project, out data))
            {
                data.OnClose();
                _projectMap.Remove(project);
            }

            if (removed)
            {
                if (!string.IsNullOrEmpty(SolutionFilename))
                    DocumentTracker.CheckDirty(SolutionFilename);
            }
        }

        internal void OnProjectBeforeUnload(IVsSccProject2 project, IVsHierarchy pStubHierarchy)
        {
            SccProjectData data;
            if (_projectMap.TryGetValue(project, out data))
            {
                data.Unloading = true;
                Monitor.ScheduleMonitor(data.GetAllFiles()); // Keep track of changes in files of unloaded project
            }
        }

        bool _registeredSccCleanup;
        internal void OnSccCleanup(CommandEventArgs e)
        {
            _registeredSccCleanup = false;

            EnsureLoaded();

            if (_delayedDelete != null)
            {
                IEnumerable<string> files = _delayedDelete;
                _delayedDelete = null;

                using (SvnSccContext svn = new SvnSccContext(Context))
                {
                    foreach (string node in files)
                    {
                        if (!SvnItem.PathExists((node)))
                            svn.MetaDelete(node);
                    }
                }
            }
        }

        public void EnsureLoaded()
        {
            if (_syncMap)
            {
                _syncMap = false;

                foreach (SccProjectData pd in _projectMap.Values)
                    pd.Load();
            }
        }

        /// <summary>
        /// The node may be just removed from the project. Check later.
        /// Some projects delete the file before (C#) and some after (C++) calling OnProjectFileRemoved
        ///
        /// And when renaming a C# project (VS11 Beta) we sometimes even get a delete before a rename.
        /// </summary>
        /// <param name="path"></param>
        internal void AddDelayedDelete(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (_delayedDelete == null)
                _delayedDelete = new HybridCollection<string>();

            if (!_delayedDelete.Contains(path))
                _delayedDelete.Add(path);

            RegisterForSccCleanup();
        }

        public void ScheduleSvnRefresh(List<SvnClientAction> sccRefreshItems)
        {
            if (sccRefreshItems == null)
                throw new ArgumentNullException("sccRefreshItems");

            CommandService.PostIdleAction(
                delegate
                {
                    bool sorted = false;
                    foreach (SccProjectData project in _projectMap.Values)
                    {
                        if (project.RequiresForcedRefresh() && !string.IsNullOrEmpty(project.ProjectDirectory))
                        {
                            string dir = project.ProjectDirectory;

                            foreach (SvnClientAction action in sccRefreshItems)
                            {
                                if (SvnItem.IsBelowRoot(action.FullPath, dir))
                                {
                                    if (!sorted)
                                    {
                                        sorted = true;
                                        sccRefreshItems.Sort();
                                    }

                                    project.PerformRefresh(sccRefreshItems);
                                    break;
                                }
                            }
                        }
                    }
                });
        }

        IAnkhCommandService _commandService;
        IAnkhCommandService CommandService
        {
            get { return _commandService ?? (_commandService = GetService<IAnkhCommandService>()); }
        }

        void RegisterForSccCleanup()
        {
            if (_registeredSccCleanup)
                return;

            IAnkhCommandService cmd = CommandService;

            if (cmd != null)
                cmd.PostTickCommand(ref _registeredSccCleanup, AnkhCommand.SccFinishTasks);
        }
    }
}
