// $Id$
using System;
using System.IO;
using NUnit.Framework;

namespace NSvn.Core.Tests
{
    /// <summary>
    /// Tests for the Client::Export method
    /// </summary>
    [TestFixture]
    public class ExportTest : TestBase
    {
        [SetUp]
        public override void SetUp() 
        {
            base.SetUp();
            this.ExtractRepos();
            this.ExtractWorkingCopy();
            this.newWc = this.FindDirName( Path.Combine( Path.GetTempPath(), "moo" ) );
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Directory.Delete( this.newWc, true );
        }

        /// <summary>
        /// Test export operation from a repository
        /// </summary>
        [Test]
        public void TestExportRepos()
        {            
            this.Client.Export( this.ReposUrl, this.newWc, Revision.Head, false );

            Assertion.Assert( "Exported file not there", 
                File.Exists( Path.Combine( this.newWc, "Form.cs" ) ) );
            Assertion.Assert( ".svn directory found", 
                !Directory.Exists( Path.Combine( this.newWc, Client.AdminDirectoryName ) ) );
        }
        /// <summary>
        /// Test export operation from a working copy
        /// </summary>
        [Test]
        public void TestExportWc()
        {   
            this.Client.Export( this.WcPath, this.newWc, Revision.Head, false );

            Assertion.Assert( "Exported file not there", 
                File.Exists( Path.Combine( this.newWc, "Form.cs" ) ) );
            Assertion.Assert( ".svn directory found", 
                !Directory.Exists( Path.Combine( this.newWc, Client.AdminDirectoryName ) ) );
        }

        [Test]
        public void TestExportNonRecursive()
        {
            this.Client.Export( this.WcPath, this.newWc, Revision.Unspecified, Revision.Head,
                false, false, false, null );
            Assert.AreEqual( 0, Directory.GetDirectories( this.newWc ).Length );
        }       

        private string newWc;

    }
}
