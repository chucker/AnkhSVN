﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.0.3705.288
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=1.0.3705.288.
// 
namespace Ankh.Config {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ankhsvn.com/Config.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://ankhsvn.com/Config.xsd", IsNullable=false)]
    public class Config {
        
        /// <remarks/>
        public ConfigRepositoryExplorer RepositoryExplorer;
        
        /// <remarks/>
        public string MergeExePath;
        
        /// <remarks/>
        public string DiffExePath;
        
        /// <remarks/>
        public string LogMessageTemplate;
        
        /// <remarks/>
        public bool AutoAddNewFiles;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AutoAddNewFilesSpecified;
        
        /// <remarks/>
        public bool DisableSolutionReload;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DisableSolutionReloadSpecified;
        
        /// <remarks/>
        public ConfigSubversion Subversion;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ankhsvn.com/Config.xsd")]
    public class ConfigRepositoryExplorer {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ankhsvn.com/Config.xsd")]
    public class ConfigSubversion {
        
        /// <remarks/>
        public string ConfigDir;
        
        /// <remarks/>
        public string SvnExePath;
        
        /// <remarks/>
        public string AdminDirectoryName;
    }
}