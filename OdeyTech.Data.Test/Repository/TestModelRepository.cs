// --------------------------------------------------------------------------
// <copyright file="TestModelRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using OdeyTech.Data.Model.Interface;
using OdeyTech.Data.Repository;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Test.Repository
{
    internal class TestModelRepository<T> : ModelRepository<T> where T : IModel
    {
        public TestModelRepository(IDbConnection dbConnection) : base(DatabaseType.SQLite, dbConnection) { }

        public void SetSqlExecutor(ISqlExecutor sqlExecutor) => SqlExecutor = sqlExecutor;

        public void SetSqlQueryGenerator(ISqlQueryGenerator sqlQueryGenerator) => SqlQueryGenerator = sqlQueryGenerator;

        public void SetTableTemplate(SqlTable tableSource) => TableTemplate = tableSource;

        protected override void SaveItem(SqlTable table, T item) { }
    }
}
