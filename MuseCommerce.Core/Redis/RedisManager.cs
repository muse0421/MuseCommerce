using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Redis
{
    public class RedisManager
    {
        public static string WriteServerList = System.Configuration.ConfigurationManager.AppSettings["rediswritepath"].ToString();
        public static string ReadServerList = System.Configuration.ConfigurationManager.AppSettings["redisreadpath"].ToString();
        public static int MaxWritePoolSize = 200;
        public static int MaxReadPoolSize = 600;


        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }


        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] writeServerList = SplitString(WriteServerList, ",");
            string[] readServerList = SplitString(ReadServerList, ",");

            prcm = new PooledRedisClientManager(writeServerList, readServerList,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = MaxWritePoolSize,
                                 MaxReadPoolSize = MaxReadPoolSize,
                                 AutoStart = true,
                             });

            prcm.ConnectTimeout = 1000;
            prcm.SocketSendTimeout = 1000;
            prcm.SocketReceiveTimeout = 1000;

        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();

            return prcm.GetClient();
        }

    }
}
