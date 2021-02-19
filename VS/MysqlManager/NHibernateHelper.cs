using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace MysqlManager
{
    class NHibernateHelper
    {
        private static ISessionFactory factory = null;


        public static ISessionFactory getFactory()
        {
            if (factory == null)
            {
                Configuration configuation = new Configuration();
                configuation.Configure();
                configuation.AddAssembly("MysqlManager");
                return configuation.BuildSessionFactory();
            }
            else
            {
                return factory;
            }
        }
    }
}
