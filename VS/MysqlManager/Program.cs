using System;
using System.Collections.Generic;
using MysqlManager.NHibernateManager;
using MysqlManager.Entity;
using MysqlManager.Emun;
using MysqlManager.Enum;
using MysqlManager.Utils;

namespace MysqlManager
{
    class Program
    {
        public static void Main(String[] args) {
            SqlFileUtile.praseCardMysqlToFile();
        }
    }
}
