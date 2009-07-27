﻿using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Ankh.UI;
using Ankh.ExtensionPoints.IssueTracker;

namespace Ankh.Services
{
    [GlobalService(typeof(IAnkhIssueService))]
    class AnkhIssueService : AnkhService, IAnkhIssueService
    {
        Dictionary<string, IIssueRepositoryConnector> _nameConnectorMap;
        IIssueRepository _repository;

        public AnkhIssueService(IAnkhServiceProvider context)
            : base(context)
        {
        }

        #region IAnkhIssueService Members

        public ICollection<IIssueRepositoryConnector> Connectors
        {
            get
            {
                if (_nameConnectorMap != null)
                {
                    IIssueRepositoryConnector[] result = new IIssueRepositoryConnector[_nameConnectorMap.Count];
                    _nameConnectorMap.Values.CopyTo(result, 0);
                    return result;
                }
                return new IIssueRepositoryConnector[]{};
            }
        }

        public bool TryGetConnector(string name, out IIssueRepositoryConnector connector)
        {
            connector = null;
            if (_nameConnectorMap != null
                && _nameConnectorMap.Count > 0)
            {
                return _nameConnectorMap.TryGetValue(name, out connector);
            }
            return false;
        }

        public IIssueRepository CurrentIssueRepository
        {
            get
            {
                if (_repository == null)
                {
                    IIssueRepositorySettings settings = ReadRepositorySettings();
                    if (settings != null)
                    {
                        string connectorName = settings.ConnectorName;
                        if (!string.IsNullOrEmpty(connectorName))
                        {
                            IIssueRepositoryConnector connector;
                            if (TryGetConnector(connectorName, out connector))
                            {
                                _repository = connector.Create(settings);
                            }
                        }
                    }
                }
                return _repository;
            }
            set
            {
                _repository = value;

                if (IssueRepositoryChanged != null)
                {
                    IssueRepositoryChanged(this, EventArgs.Empty);
                }

                SetSolutionSettings(_repository);
            }
        }
        #endregion

        protected override void OnPreInitialize()
        {
            base.OnPreInitialize();
            _nameConnectorMap = new Dictionary<string, IIssueRepositoryConnector>();
            ReadRegistry();
        }

        private void ReadRegistry()
        {
            IAnkhPackage ankhPackage = GetService<IAnkhPackage>(typeof(IAnkhPackage));
            if (ankhPackage != null)
            {
                using (RegistryKey key = ankhPackage.ApplicationRegistryRoot)
                {
                    using (RegistryKey aKey = key.OpenSubKey("IssueRepositoryConnectors"))
                    {
                        if (aKey == null)
                            return;

                        string[] connectorKeys = aKey.GetSubKeyNames();
                        foreach (string connectorKey in connectorKeys)
                        {
                            RegistryKey connector = aKey.OpenSubKey(connectorKey);
                            string serviceName = (string)connector.GetValue("");
                            IIssueRepositoryConnector descriptor = new AnkhIssueRepositoryConnectorDelegate(this, serviceName, connectorKey);
                            _nameConnectorMap.Add(serviceName, descriptor);
                        }
                    }
                }
            }
        }

        private IIssueRepositorySettings ReadRepositorySettings()
        {
            // TODO read solution settings
            return null;
        }

        private void SetSolutionSettings(IIssueRepositorySettings settings)
        {
            // TODO set solution settings
        }

        #region IAnkhIssueService Members
        public event EventHandler IssueRepositoryChanged;

        #endregion
    }

    /// <summary>
    /// This class acts as a proxy to the actual connector.
    /// This proxy serves "descriptive" properties w/o initializing the actual connector.
    /// The actual connector package initialization is delayed until a non-descriptive property is needed.
    /// Currently, "connector name" is the only descriptive property.
    /// </summary>
    class AnkhIssueRepositoryConnectorDelegate : IIssueRepositoryConnector
    {
        private IIssueRepositoryConnector _delegate = null;
        private IAnkhServiceProvider _context;
        private string _name = null;
        private string _delegateId = null;

        public AnkhIssueRepositoryConnectorDelegate(IAnkhServiceProvider context, string name, string delegateServiceId)
        {
            _context = context;
            _name = name;
            _delegateId = delegateServiceId;
        }

        private IIssueRepositoryConnector Delegate
        {
            get
            {
                if (_delegate == null
                    && !string.IsNullOrEmpty(_delegateId))
                {
                    Type serviceType = Type.GetTypeFromCLSID(new Guid(_delegateId));
                    if (serviceType != null)
                    {
                        _delegate = _context.GetService<IIssueRepositoryConnector>(serviceType);
                    }
                }
                return _delegate;
            }
        }

        #region IIssueRepositoryConnector Members

        public IIssueRepository Create(IIssueRepositorySettings settings)
        {
            IIssueRepositoryConnector dlg = Delegate;
            if (dlg != null)
            {
                return dlg.Create(settings);
            }
            return null;
        }

        public IIssueRepositoryConfigurationPage ConfigurationPage
        {
            get
            {
                IIssueRepositoryConnector dlg = Delegate;
                if (dlg != null)
                {
                    return dlg.ConfigurationPage;
                }
                return null;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion
    }

    class AnkhIssueRepositorySettings : IIssueRepositorySettings
    {
        private string _connectorName;
        private Uri _repositoryUri;
        private string _repositoryId;
        private Dictionary<string, object> _customProperties;

        #region IIssueRepositorySettings Members

        public string ConnectorName
        {
            get { return _connectorName; }
            internal set { _connectorName = value; }
        }

        public Uri RepositoryUri
        {
            get { return _repositoryUri; }
            internal set { _repositoryUri = value; }
        }

        public string RepositoryId
        {
            get { return _repositoryId; }
            internal set { _repositoryId = value; }
        }

        public Dictionary<string, object> CustomProperties
        {
            get { return _customProperties; }
            internal set { _customProperties = value; }
        }

        #endregion
    }
}
