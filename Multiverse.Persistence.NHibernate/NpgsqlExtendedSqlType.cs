using NHibernate.SqlTypes;
using NpgsqlTypes;
using System.Data;

namespace NHibernate.SqlTypes
{
    public class NpgsqlExtendedSqlType : SqlType
    {
        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType) : base(dbType)
        {
            this.npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length) : base(dbType, length)
        {
            this.npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale) : base(dbType, precision, scale)
        {
            this.npgDbType = npgDbType;
        }

        readonly NpgsqlDbType npgDbType;
        public NpgsqlDbType NpgDbType => this.npgDbType;
    }
}
