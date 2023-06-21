// --------------------------------------------------------------------------
// <copyright file="TestDependentModelRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using OdeyTech.Data.Repository;
using OdeyTech.Data.Test.Model;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Table;
using OdeyTech.SqlProvider.Executor;
using OdeyTech.SqlProvider.Query;

namespace OdeyTech.Data.Test.Repository
{
    internal class TestDependentModelRepository : DependentModelRepository<TestDependentModel>
    {
        public TestDependentModelRepository(DatabaseType databaseType, IDbConnection dbConnection) : base(databaseType, dbConnection) { }

        protected override string DependenceColumnName => "ParentId";

        public void SetSqlExecutor(ISqlExecutor sqlExecutor) => SqlExecutor = sqlExecutor;

        public void SetSqlQueryGenerator(ISqlQueryGenerator sqlQueryGenerator) => SqlQueryGenerator = sqlQueryGenerator;

        public void SetTableTemplate(SqlTable tableSource) => TableTemplate = tableSource;
    }
}
