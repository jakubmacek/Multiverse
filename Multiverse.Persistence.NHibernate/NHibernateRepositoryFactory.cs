using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Multiverse.Persistence.NHibernate
{
    public class NHibernateRepositoryFactory : IRepositoryFactory
    {
        private readonly string _universeType;

        private readonly ISessionFactory _sessionFactory;

        private readonly Configuration _configuration;

        public NHibernateRepositoryFactory(string universeType, Configuration configuration, ISessionFactory sessionFactory)
        {
            _configuration = configuration;
            _sessionFactory = sessionFactory;
            _universeType = universeType;
        }

        public IRepository Create(int worldId)
        {
            //InitializeStorage();

#pragma warning disable DF0010 // Marks undisposed local variables. Session is disposed in the Repository.
            var session = _sessionFactory.OpenSession();
#pragma warning restore DF0010 // Marks undisposed local variables.
            //session.FlushMode = FlushMode.Auto;

            var world = session.Get<World>(worldId);
            if (world == null)
            {
                world = new World()
                {
                    Id = worldId,
                    Universe = _universeType,
                    Timestamp = 0,
                };
                session.Save(world);
                session.Flush();
            }

            return new Repository(session, world);
        }

        public void InitializeStorage()
        {
            var schemaExport = new SchemaExport(_configuration);
            schemaExport.Execute(false, true, false);
        }
    }
}
