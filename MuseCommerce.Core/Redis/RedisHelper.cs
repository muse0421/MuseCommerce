using Common.Logging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Redis
{
    public class RedisHelper
    {
        #region 单体操作
        /// <summary>
        /// 设置单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Single_Set_Itme<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    return redis.Set<T>(key, t, new TimeSpan(1, 0, 0));
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 获取单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Single_Get_Itme<T>(string key) where T : class
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary>
        /// 移除单体
        /// </summary>
        /// <param name="key"></param>
        public static bool Single_Remove_Itme(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.Remove(key);
            }
        }


        #endregion

        #region List集合
        /// <summary>
        /// 新增一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void List_AddJson<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {

                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                redis.AddItemToList(key, value);
            }
        }


        /// <summary>
        /// 新增一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void List_Add<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    var redisTypedClient = redis.As<T>();
                    redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
                }
            }
            catch (Exception ex)
            {
                
                LogManager.Adapter.GetLogger("default").Error(typeof(T).ToString());
                LogManager.Adapter.GetLogger("default").Error(ex.ToString());
            }
        }

        public static void List_Add<T>(string key, List<T> t)
        {
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    redis.ExpireEntryIn(key, TimeSpan.FromHours(4));
                    var redisTypedClient = redis.As<T>();
                    t.ForEach(item =>
                    {
                        redisTypedClient.AddItemToList(redisTypedClient.Lists[key], item);
                    });

                }
            }
            catch (Exception ex)
            {
                LogManager.Adapter.GetLogger("default").Error(typeof(T).ToString());
                LogManager.Adapter.GetLogger("default").Error(ex.ToString());
            }
        }


        /// <summary>
        /// 移除单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool List_Remove<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    var redisTypedClient = redis.As<T>();
                    return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
                }
            }
            catch (Exception ex)
            {
                LogManager.Adapter.GetLogger("default").Error(typeof(T).ToString());
                LogManager.Adapter.GetLogger("default").Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 移除所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void List_RemoveAll<T>(string key)
        {
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    var redisTypedClient = redis.As<T>();
                    redisTypedClient.Lists[key].RemoveAll();
                }
            }
            catch (Exception ex)
            {
                LogManager.Adapter.GetLogger("default").Error(typeof(T).ToString());
                LogManager.Adapter.GetLogger("default").Error(ex.ToString());
            }
        }


        public static List<string> List_GetAllKey<T>()
        {
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    var redisTypedClient = redis.As<T>();
                    return redisTypedClient.GetAllKeys();
                }
            }
            catch (Exception ex)
            {                
                LogManager.Adapter.GetLogger("default").Error(typeof(T).ToString());
                LogManager.Adapter.GetLogger("default").Error(ex.ToString());
            }
            return new List<string>();
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long List_Count<T>(string key)
        {
            try
            {
                if (key.StartsWith("urn:"))
                {
                    return 0;
                }
                if (key.StartsWith("ids:"))
                {
                    return 0;
                }
                if (key.ToLower().StartsWith("redis:"))
                {
                    return 0;
                }

                using (IRedisClient redis = RedisManager.GetClient())
                {
                    var redisTypedClient = redis.As<T>();
                    return redisTypedClient.Lists[key].Count;
                }
            }
            catch (Exception ex)
            {
                LogManager.Adapter.GetLogger("default").Error(typeof(T).ToString());
                LogManager.Adapter.GetLogger("default").Error(key + "=" + ex.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 获取范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static List<T> List_GetRange<T>(string key, int start, int count)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var c = redis.As<T>();
                return c.Lists[key].GetRange(start, start + count - 1);
            }
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> List_GetList<T>(string key)
        {
            List<T> result = new List<T>();
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    var c = redis.As<T>();
                    result = c.Lists[key].GetRange(0, c.Lists[key].Count);
                }
            }
            catch (Exception ex)
            {
                LogManager.Adapter.GetLogger("default").Error(ex.ToString());
            }
            return result;
        }
        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> List_GetList<T>(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRange<T>(key, start, pageSize);
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void List_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
        #endregion

        #region Set信息
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void Set_Add<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Sets[key].Add(t);

            }
        }
        /// <summary>
        /// 包含
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Set_Contains<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Contains(t);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Set_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Remove(t);
            }
        }
        #endregion

        #region Hash

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Exist<T>(string key, string dataKey)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.HashContainsEntry(key, dataKey);
            }
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Set<T>(string key, string dataKey, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.SetEntryInHash(key, dataKey, value);
            }
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Remove(string key, string dataKey)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.RemoveEntryFromHash(key, dataKey);
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Remove(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.Remove(key);
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T Hash_Get<T>(string key, string dataKey)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = redis.GetValueFromHash(key, dataKey);
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> Hash_GetAll<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var list = redis.GetHashValues(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(value);
                    }
                    return result;
                }
                return null;
            }
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void Hash_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }

        #endregion

        #region SortedSet 分类
        /// <summary>
        ///  添加数据到 SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public static bool SortedSet_Add<T>(string key, T t, double score)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.AddItemToSortedSet(key, value, score);
            }
        }
        /// <summary>
        /// 移除数据从SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool SortedSet_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }
        /// <summary>
        /// 修剪SortedSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        public static long SortedSet_Trim(string key, int size)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.RemoveRangeFromSortedSet(key, size, 9999999);
            }
        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SortedSet_Count(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.GetSortedSetCount(key);
            }
        }

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetList<T>(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListALL<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListSearchKeys<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                List<string> keys = redis.GetAllKeys();
                List<T> result = new List<T>();
                keys.ForEach(k =>
                {
                    if (k.Contains(key))
                    {
                        var item = redis.Get<T>(k);
                        //var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item.ToString());
                        result.Add(item);
                    }
                });
                return result;
            }
            //return null;
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SortedSet_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }


        #endregion

    }
}
