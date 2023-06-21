// --------------------------------------------------------------------------
// <copyright file="TestReadableRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using OdeyTech.Data.Repository;
using OdeyTech.Data.Test.Model;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Test.Repository
{
    internal class TestReadableRepository : ReadableRepository<TestModel>
    {
        public TestReadableRepository(IDbConnection dbConnection) : base(DatabaseType.SQLite, dbConnection) { }

        public void SetSqlExecutor(ISqlExecutor sqlExecutor) => SqlExecutor = sqlExecutor;

        public void SetSqlQueryGenerator(ISqlQueryGenerator sqlQueryGenerator) => SqlQueryGenerator = sqlQueryGenerator;
    }
}
