using NHibernate.Driver;
using NHibernate.SqlTypes;
using Npgsql;
using System;
using System.Data.Common;

namespace NHibernate.Driver
{
    public class NpgsqlExtendedDriver : NpgsqlDriver
    {
        protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType)
        {
            if (sqlType is NpgsqlExtendedSqlType && dbParam is NpgsqlParameter)
            {
                InitializeParameter(dbParam as NpgsqlParameter, name, sqlType as NpgsqlExtendedSqlType);
            }
            else
            {
                base.InitializeParameter(dbParam, name, sqlType);
            }
        }

        protected virtual void InitializeParameter(NpgsqlParameter? dbParam, string name, NpgsqlExtendedSqlType? sqlType)
        {
            if (sqlType == null || dbParam == null)
                throw new QueryException(string.Format("No type assigned to parameter '{0}'", name));

            dbParam.ParameterName = FormatNameForParameter(name);
            dbParam.DbType = sqlType.DbType;
            dbParam.NpgsqlDbType = sqlType.NpgDbType;
        }
    }
}
