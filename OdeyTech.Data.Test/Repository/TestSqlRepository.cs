// --------------------------------------------------------------------------
// <copyright file="TestSqlRepository.cs" author="Andrii Odeychuk">
//
// Copyright (c) Andrii Odeychuk. ALL RIGHTS RESERVED
// The entire contents of this file is protected by International Copyright Laws.
// </copyright>
// --------------------------------------------------------------------------

using System.Data;
using OdeyTech.Data.Repository;
using OdeyTech.SqlProvider.Entity.Database;
using OdeyTech.SqlProvider.Entity.Database.Checker;

namespace OdeyTech.Data.Test.Repository
{
    public class TestSqlRepository : SqlRepository
    {
        public TestSqlRepository(IDbConnection dbConnection, IDbChecker dbChecker) : base(DatabaseType.SQLite, dbConnection)
        {
            DbChecker = dbChecker;
        }
    }
}
