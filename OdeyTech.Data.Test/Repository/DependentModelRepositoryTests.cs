// --------------------------------------------------------------------------
// <copyright file="DependentModelRepositoryTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OdeyTech.Data.Test.Model;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Test.Repository
{
    [TestClass]
    public class DependentModelRepositoryTests
    {
        private Mock<IDbConnection> dbConnectionMock;
        private SqlTable sqlTable;
        private Mock<ISqlQueryGenerator> sqlQueryGeneratorMock;
        private Mock<ISqlExecutor> sqlExecutorMock;
        private TestDependentModelRepository dependentModelRepository;

        [TestInitialize]
        public void Initialize()
        {
            this.dbConnectionMock = GetConnection();
            this.sqlExecutorMock = GetSqlExecutorMock();
            this.sqlQueryGeneratorMock = GetSqlQueryGenerator();
            this.sqlTable = GetSqlTable();

            this.dependentModelRepository = new TestDependentModelRepository(DatabaseType.SQLite, this.dbConnectionMock.Object);
            this.dependentModelRepository.SetSqlExecutor(this.sqlExecutorMock.Object);
            this.dependentModelRepository.SetSqlQueryGenerator(this.sqlQueryGeneratorMock.Object);
            this.dependentModelRepository.SetTableTemplate(this.sqlTable);
        }

        [TestMethod]
        public void SelectByParent_ShouldCallSelect()
        {
            // Act
            TestModel parentItem = GetParent();
            this.dependentModelRepository.SelectByParent(parentItem);

            // Assert
            this.sqlExecutorMock.Verify(dc => dc.Select(It.IsAny<string>(), It.IsAny<DbParameter[]>()), Times.Once);
            this.sqlQueryGeneratorMock.Verify(dc => dc.Select(It.IsAny<SqlTable>()), Times.Once);
        }

        [TestMethod]
        public void DeleteByParent_ShouldCallDelete()
        {
            // Act
            TestModel parentItem = GetParent();
            this.dependentModelRepository.DeleteByParent(parentItem);

            // Assert
            this.sqlExecutorMock.Verify(dc => dc.Query(It.IsAny<string>(), It.IsAny<DbParameter[]>()), Times.Once);
            this.sqlQueryGeneratorMock.Verify(dc => dc.Delete(It.IsAny<SqlTable>()), Times.Once);
        }

        private Mock<IDbConnection> GetConnection() => new();

        private DataTable GetTestData() => new();

        private TestModel GetParent() => new(1);

        private SqlTable GetSqlTable() => new();

        private Mock<ISqlQueryGenerator> GetSqlQueryGenerator()
        {
            var sqlQueryGeneratorMock = new Mock<ISqlQueryGenerator>();
            sqlQueryGeneratorMock.Setup(s => s.Select(It.IsAny<SqlTable>())).Returns(string.Empty);
            sqlQueryGeneratorMock.Setup(s => s.Delete(It.IsAny<SqlTable>()));
            return sqlQueryGeneratorMock;
        }

        private Mock<ISqlExecutor> GetSqlExecutorMock()
        {
            var sqlExecutorMock = new Mock<ISqlExecutor>();
            sqlExecutorMock.Setup(s => s.Select(It.IsAny<string>(), It.IsAny<DbParameter[]>())).Returns(GetTestData());
            return sqlExecutorMock;
        }
    }
}
