using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.SqlCommand;
using System;

namespace Multiverse.Persistence.NHibernate
{
    class CustomInterceptor : EmptyInterceptor
    {
        readonly ILogger _logger;

        public CustomInterceptor(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override SqlString OnPrepareStatement(SqlString sql)
        {
            _logger.LogDebug(sql.ToString());
            return sql;
        }

        public override object Instantiate(string clazz, object id)
        {
            return base.Instantiate(clazz, id);
        }

        // Swaps Interfaces for Implementations
        //public override object Instantiate(string clazz, EntityMode entityMode, object id)
        //{
        //    //var handler = TypeHandler.GetByInterface(clazz);
        //    //if (handler == null || !handler.Interface.IsInterface) return base.Instantiate(clazz, entityMode, id);
        //    //var poco = handler.Poco;
        //    //if (poco == null) return base.Instantiate(clazz, entityMode, id);

        //    //// Return Poco for Interface
        //    //var instance = FormatterServices.GetUninitializedObject(poco);
        //    //SessionFactory.GetClassMetadata(clazz).SetIdentifier(instance, id, entityMode);

        //    //return instance;
        //}
    }
}
