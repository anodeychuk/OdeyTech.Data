// --------------------------------------------------------------------------
// <copyright file="ReadableRepositoryTests.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OdeyTech.Data.Test.Model;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Test.Repository
{
    [TestClass]
    public class ReadableRepositoryTests
    {
        private TestReadableRepository readableRepository;

        [TestInitialize]
        public void Initialize()
        {
            var dbConnectionMock = GetConnection();
            var sqlExecutorMock = GetSqlExecutor();
            var sqlQueryGeneratorMock = GetSqlQueryGenerator();

            this.readableRepository = new TestReadableRepository(dbConnectionMock.Object);
            this.readableRepository.SetSqlExecutor(sqlExecutorMock.Object);
            this.readableRepository.SetSqlQueryGenerator(sqlQueryGeneratorMock.Object);
        }

        [TestMethod]
        public void Select_ShouldReturnItems()
        {
            // Act
            IEnumerable<TestModel> items = this.readableRepository.Select();

            // Assert
            Assert.AreEqual(2, items.Count());
        }

        private Mock<IDbConnection> GetConnection() => new();

        private Mock<ISqlExecutor> GetSqlExecutor()
        {
            var sqlExecutorMock = new Mock<ISqlExecutor>();
            sqlExecutorMock.Setup(s => s.Select(It.IsAny<string>(), It.IsAny<DbParameter[]>())).Returns(GetTestData());
            return sqlExecutorMock;
        }

        private Mock<ISqlQueryGenerator> GetSqlQueryGenerator()
        {
            var sqlQueryGeneratorMock = new Mock<ISqlQueryGenerator>();
            sqlQueryGeneratorMock.Setup(s => s.Select(It.IsAny<SqlTable>())).Returns(string.Empty);
            return sqlQueryGeneratorMock;
        }

        private DataTable GetTestData()
        {
            var dt = new DataTable();
            dt.Columns.Add(nameof(TestModel.Identifier), typeof(ulong));
            dt.Rows.Add(1);
            dt.Rows.Add(2);

            return dt;
        }
    }
}
