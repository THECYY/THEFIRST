using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace HearthStoneServer
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
                configuation.AddAssembly("HearthStoneServer");
                return configuation.BuildSessionFactory();
            }
            else
            {
                return factory;
            }
        }
    }
}
