using Kafka.Client;
using Kafka.Client.Cfg;
using Kafka.Client.Cluster;
using Kafka.Client.Consumers;
using Kafka.Client.Exceptions;
using Kafka.Client.Helper;
using Kafka.Client.Messages;
using Kafka.Client.Producers;
using Kafka.Client.Producers.Partitioning;
using Kafka.Client.Producers.Sync;
using Kafka.Client.Requests;
using Kafka.Client.Responses;
using Kafka.Client.Serialization;
using Kafka.Client.Utils;
using Kafka.Client.ZooKeeperIntegration;
using log4net.Config;
using Microsoft.KafkaNET.Library.Util;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MuseCommerce.Core.Common;
using MuseCommerce.Core.Redis;
using ProtoBuf;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
using Topshelf;
using ZooKeeperNet;

namespace MuseRemoteScheduler.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();

            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(configurationSource));

            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);

            Logger.SetLogWriter(logWriterFactory.Create());

            //GetTopicList();
            //BrokList();
            //CreateTopic();
            //SendMsg("mytopic", 5);


            //Console.ReadKey();
            //return;
            
            HostFactory.Run(x =>
            {
                x.Service<SchedulerServer>(s =>
                {
                    s.ConstructUsing(name => new SchedulerServer());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("SchedulerServer Topshelf Host");
                x.SetDisplayName("SchedulerServer");
                x.SetServiceName("SchedulerServer");
            });

           
        }

       

        public static void GetTopicList()
        {
            using (ZooKeeperClient zkClient = new ZooKeeperClient("mcafee-ips:2181",
                           ZooKeeperConfiguration.DefaultSessionTimeout, ZooKeeperStringSerializer.Serializer))
            {
                zkClient.Connect();
                List<string> topicList = new List<string>();
                string path = "/brokers/topics";
                try
                {
                    IEnumerable<string> ts = zkClient.GetChildren(path);
                    foreach (var p in ts)
                    {
                        topicList.Add(p);
                        Console.WriteLine(p);
                    }
                }
                catch (KeeperException e)
                {
                    if (e.ErrorCode == KeeperException.Code.NONODE)
                    {
                        throw new ApplicationException("Please make sure the path exists in zookeeper:  " + path, e);
                    }
                    else
                        throw;
                }
            }
        }

        public static void BrokList()
        {
            using (var zkClient = new ZooKeeperClient("mcafee-ips:2181",
                            ZooKeeperConfiguration.DefaultSessionTimeout, ZooKeeperStringSerializer.Serializer))
            {
                zkClient.Connect();
                var temp = ZkUtils.GetAllBrokersInCluster(zkClient);
                foreach (var item in temp)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        public static void CreateTopic()
        {
            using (ZooKeeperClient zkClient = new ZooKeeperClient("mcafee-ips:2181",
                            ZooKeeperConfiguration.DefaultSessionTimeout, ZooKeeperStringSerializer.Serializer))
            {
                zkClient.Connect();
                var temp = ZkUtils.GetAllBrokersInCluster(zkClient);

                List<BrokerConfiguration> brokersConfig = new List<BrokerConfiguration>();

                foreach (var item in temp)
                {
                    brokersConfig.Add(new BrokerConfiguration()
                    {

                        BrokerId = item.Id,
                        Host = item.Host,
                        Port = item.Port
                    });
                }

                var config2 = new ProducerConfiguration(brokersConfig);
                var prodycerpool = new SyncProducerPool(config2);

                List<string> topic = new List<string>();
                topic.Add("mytopic91");
                topic.Add("mytopic101");
                var meta = TopicMetadataRequest.Create(topic, 0, 0, "xp");
                prodycerpool.GetProducer(0).Send(meta);

            }
        }

        public static void SendMsg(string Topic, int SendTimes)
        {
            IKafkaConnection connection = new KafkaConnection("192.168.101.141", 9092, 100, 60000, 60000, 100);
            var config = new SyncProducerConfiguration { MaxMessageSize = 99 };
            var producer = new SyncProducer(config, connection);

            while (SendTimes > 0)
            {
                string msg = "hello=" + SendTimes;
                byte[] byte2 = System.Text.Encoding.UTF8.GetBytes(msg);
                producer.Send(new ProducerRequest(1, "client", 0, 0, new List<TopicData>()
                {
                    new TopicData("mytopic",
                        new List<PartitionData>()
                        {
                            new PartitionData(0, new BufferedMessageSet(new List<Message>() {new Message(byte2)}, 0))
                        })
                }));
                SendTimes--;
            }
        }

        public static void ConsumerLoopData()
        {

            KafkaSimpleManagerConfiguration config = new KafkaSimpleManagerConfiguration()
            {
                Zookeeper = "mcafee-ips:2181"
            };
            config.Verify();

            int partition = 0;
            string TOPIC = "mytopic";

            long earliestOffset = 0;
            long latestOffset = 0;
            using (KafkaSimpleManager<int, Message> kafkaSimpleManager = new KafkaSimpleManager<int, Message>(config))
            {
                var config2 = new ConsumerConfiguration("mcafee-ips", 9092) { NumberOfTries = 3000 };

                string ClientID = Assembly.GetExecutingAssembly().ManifestModule.ToString();


                TopicMetadata topicMetadata = kafkaSimpleManager.RefreshMetadata(0, ClientID, 1, TOPIC, true);

                Consumer consumer = kafkaSimpleManager.GetConsumer(TOPIC, 0); //new Consumer(config2, "mcafee-ips", 9092);

                List<string> topic = new List<string>();
                topic.Add("mytopic");
                var meta = TopicMetadataRequest.Create(topic, 0, 1, ClientID);
                var topicEnum = consumer.GetMetaData(meta);

                List<PartitionOffsetRequestInfo> tx1 = new List<PartitionOffsetRequestInfo>();
                foreach (var item in topicEnum)
                {
                    foreach (var item1 in item.PartitionsMetadata)
                    {
                        tx1.Add(new PartitionOffsetRequestInfo(item1.PartitionId, 60000, 60000));
                    }
                }

                Dictionary<string,
            List<PartitionOffsetRequestInfo>> requestInfo = new Dictionary<string, List<PartitionOffsetRequestInfo>>();
                requestInfo.Add(TOPIC, tx1);

                var offq = new OffsetRequest(requestInfo, 0, 1, ClientID);
                var tx = consumer.GetOffsetsBefore(offq);


                kafkaSimpleManager.RefreshAndGetOffset(0, ClientID, 1, TOPIC, 0, false, out earliestOffset, out latestOffset);
                int partitionIndex = 0;
                string s = string.Empty;

                int fetchSize = 1000;
                PartitionData partitionData = null;

                long offset = 0;
                long startOffet = 0;
                while (true)
                {

                    Console.WriteLine("Thread : while");

                    FetchResponse fetchResponse = consumer.Fetch(ClientID,      // client id
                        TOPIC,
                        0, //random.Next(int.MinValue, int.MaxValue),                        // correlation id
                        partitionIndex,
                        startOffet,
                        fetchSize,
                        50,
                        50);

                    // 拉取消息
                    partitionData = fetchResponse.PartitionData(TOPIC, partitionIndex);
                    if (partitionData == null)
                    {
                        throw new KeyNotFoundException(string.Format("PartitionData is null,fetchOffset={0},leader={1},topic={2},partition={3}",
                            earliestOffset, consumer.Config.Broker, topic, partitionIndex));
                    }

                    if (partitionData.Error == ErrorMapping.OffsetOutOfRangeCode)
                    {
                        s = "PullMessage OffsetOutOfRangeCode,change to Latest,topic={0},leader={1},partition={2},FetchOffset={3},retryCount={4},maxRetry={5}";
                        //Logger.ErrorFormat(s, topic, consumer.Config.Broker, partitionIndex, fetchOffset, retryCount, maxRetry);
                        return;
                    }

                    if (partitionData.Error != ErrorMapping.NoError)
                    {
                        s = "PullMessage ErrorCode={0},topic={1},leader={2},partition={3},FetchOffset={4},retryCount={5},maxRetry={6}";
                        Console.WriteLine(s, partitionData.Error, topic, consumer.Config.Broker, partitionIndex, earliestOffset, 0, 122);
                        return;
                    }

                    BufferedMessageSet messageSet = fetchResponse.MessageSet(TOPIC, partition);

                    foreach (MessageAndOffset messageAndOffset in messageSet)
                    {
                        Message mess = messageAndOffset.Message;

                        String msg = System.Text.Encoding.Default.GetString(mess.Payload);


                        offset = messageAndOffset.MessageOffset;
                        Console.WriteLine("partition : " + 3 + ", offset : " + offset + "  mess : " + msg);
                    }
                    // 继续消费下一批
                    startOffet = offset + 1;
                }

            }
        }

        static IRedisClient redis = RedisManager.GetClient();
        static void MessageQuene()
        {
            ThreadPool.UnsafeQueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream(Convert.FromBase64String(redis.BlockingPopItemFromList("MessageQuene", TimeSpan.FromHours(2))));

                        Console.WriteLine("while=" + Serializer.Deserialize<TestQuene>(ms));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }, null);
        }       

        private const String PATH = "D://Lession/KM_ERP/LOG/data.bin";  
        static void TestSerializer()
        {
            //生成数据  
            List<TestQuene> testData = new List<TestQuene>();
            for (int i = 0; i < 100; i++)
            {
                testData.Add(new TestQuene() { Id = i, data = new List<string>(new string[] { "1", "2", "3" }) });
            }
            //将数据序列化后存入本地文件  
            using (Stream file = File.Create(PATH))
            {
                Serializer.Serialize<List<TestQuene>>(file, testData);
                file.Close();
            }
            //将数据从文件中读取出来，反序列化  
            List<TestQuene> fileData;
            using (Stream file = File.OpenRead(PATH))
            {
                fileData = Serializer.Deserialize<List<TestQuene>>(file);
            }
            //打印数据  
            foreach (TestQuene data in fileData)
            {
                Debug.WriteLine(data);
            }
        } 
    }

}
