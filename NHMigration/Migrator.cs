using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHMigration.Operations;
using NHMigration.Versioning;

namespace NHMigration
{
    public class Migrator
    {
        private readonly Configuration _cfg;
        private readonly ISessionFactory _sessionFactory;
        private readonly IVersioningStrategy _versioningStrategy;

        public Migrator(Configuration cfg, ISessionFactory sessionFactory, IVersioningStrategy versioningStrategy)
        {
            _cfg = cfg;
            _sessionFactory = sessionFactory;
            _versioningStrategy = versioningStrategy;
        }

        public void Execute(IEnumerable<IOperation> operations)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                foreach (var op in operations)
                {
                    op.Apply(session);
                }
            }
            
        }


    }

    
    
}
