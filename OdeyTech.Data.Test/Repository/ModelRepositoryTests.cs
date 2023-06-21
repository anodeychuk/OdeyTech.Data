// --------------------------------------------------------------------------
// <copyright file="ModelRepositoryTests.cs" author="Andrii Odeychuk">
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
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Entity.Table.Column.DataType;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Test.Repository
{
    [TestClass]
    public class ModelRepositoryTests
    {
        private TestModelRepository<TestModel> modelRepository;
        private Mock<ISqlQueryGenerator> sqlQueryGeneratorMock;
        private Mock<ISqlExecutor> sqlExecutorMock;

        [TestInitialize]
        public void Initialize()
        {
            var dbConnectionMock = GetConnection();
            this.sqlExecutorMock = GetSqlExecutor();
            this.sqlQueryGeneratorMock = GetSqlQueryGenerator();

            this.modelRepository = new TestModelRepository<TestModel>(dbConnectionMock.Object);
            this.modelRepository.SetSqlExecutor(this.sqlExecutorMock.Object);
            this.modelRepository.SetSqlQueryGenerator(this.sqlQueryGeneratorMock.Object);
            this.modelRepository.SetTableTemplate(GetSqlTable());
        }

        [TestMethod]
        public void ShouldCallInsert()
        {
            this.modelRepository.Insert(GetModel());
            this.sqlExecutorMock.Verify(dc => dc.Query(It.IsAny<string>(), It.IsAny<DbParameter[]>()), Times.Once);
            this.sqlQueryGeneratorMock.Verify(dc => dc.Insert(It.IsAny<SqlTable>()), Times.Once);
        }

        [TestMethod]
        public void ShouldCallUpdate()
        {
            this.modelRepository.Update(GetModel());
            this.sqlExecutorMock.Verify(dc => dc.Query(It.IsAny<string>(), It.IsAny<DbParameter[]>()), Times.Once);
            this.sqlQueryGeneratorMock.Verify(dc => dc.Update(It.IsAny<SqlTable>()), Times.Once);
        }

        [TestMethod]
        public void ShouldCallDelete()
        {
            this.modelRepository.Delete(GetModel());
            this.sqlExecutorMock.Verify(dc => dc.Query(It.IsAny<string>(), It.IsAny<DbParameter[]>()), Times.Once);
            this.sqlQueryGeneratorMock.Verify(dc => dc.Delete(It.IsAny<SqlTable>()), Times.Once);
        }

        private Mock<IDbConnection> GetConnection() => new();

        private Mock<ISqlExecutor> GetSqlExecutor()
        {
            var sqlExecutorMock = new Mock<ISqlExecutor>();
            sqlExecutorMock.Setup(s => s.Query(It.IsAny<string>(), It.IsAny<DbParameter[]>()));
            return sqlExecutorMock;
        }

        private Mock<ISqlQueryGenerator> GetSqlQueryGenerator()
        {
            var sqlQueryGeneratorMock = new Mock<ISqlQueryGenerator>();
            sqlQueryGeneratorMock.Setup(s => s.Insert(It.IsAny<SqlTable>())).Returns(string.Empty);
            sqlQueryGeneratorMock.Setup(s => s.Update(It.IsAny<SqlTable>())).Returns(string.Empty);
            sqlQueryGeneratorMock.Setup(s => s.Delete(It.IsAny<SqlTable>())).Returns(string.Empty);
            return sqlQueryGeneratorMock;
        }

        private TestModel GetModel() => new(1);

        private SqlTable GetSqlTable()
        {
            var table = new SqlTable();
            table.Columns.AddColumn(nameof(TestModel.Identifier), new SQLiteDataType(SQLiteDataType.DataType.Integer));
            return table;
        }
    }
}
