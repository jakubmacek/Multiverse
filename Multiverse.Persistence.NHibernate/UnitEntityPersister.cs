using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Persistence.NHibernate
{
    public class UnitEntityPersister : SingleTableEntityPersister
    {
        public UnitEntityPersister(PersistentClass persistentClass, ICacheConcurrencyStrategy cache, ISessionFactoryImplementor factory, IMapping mapping)
            : base(persistentClass, cache, factory, mapping)
        {
        }

        public override string GetSubclassForDiscriminatorValue(object value)
        {
            return base.GetSubclassForDiscriminatorValue(value);
        }

        public override object Instantiate(object id)
        {
            return base.Instantiate(id);
        }
    }
}
