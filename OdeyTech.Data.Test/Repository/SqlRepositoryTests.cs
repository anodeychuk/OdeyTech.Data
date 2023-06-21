// --------------------------------------------------------------------------
// <copyright file="SqlRepositoryTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OdeyTech.SqlProvider.Entity.Database.Checker;

namespace OdeyTech.Data.Test.Repository
{
    [TestClass]
    public class SqlRepositoryTests
    {
        private Mock<IDbConnection> dbConnectionMock;
        private Mock<IDbChecker> dbCheckerMock;
        private TestSqlRepository sqlRepository;

        [TestInitialize]
        public void Initialize()
        {
            this.dbConnectionMock = new Mock<IDbConnection>();
            this.dbCheckerMock = new Mock<IDbChecker>();
            this.sqlRepository = new TestSqlRepository(this.dbConnectionMock.Object, this.dbCheckerMock.Object);
        }

        [TestMethod]
        public void CheckDatabase_ShouldCallDbChecker()
        {
            this.sqlRepository.CheckDatabase();
            this.dbCheckerMock.Verify(dc => dc.CheckDatabase(), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldDisposeDbConnectionAndSqlExecutor()
        {
            this.sqlRepository.Dispose();

            this.dbConnectionMock.Verify(dc => dc.Dispose(), Times.Once);
        }
    }
}
