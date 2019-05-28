using System;
using StackExchange.Redis;

namespace UnitSense.Extensions.Redis
{
    /// <summary>
    /// Utilitaire pour le cache Redis
    /// </summary>
    public static class RedisManager
    {
        private static ConnectionMultiplexer clientsManager;
        private static ConfigurationOptions redisConfig;

        /// <summary>
        /// Initialisation du cache Redis
        /// </summary>
        /// <param name="config"></param>
        public static void Initialize(ConfigurationOptions config)
        {
            redisConfig = config;
            clientsManager = ConnectionMultiplexer.Connect(config);
        }

        public static ConnectionMultiplexer GetClientManager()
        {
            if (redisConfig == null || clientsManager == null)
                throw new Exception("Not initialized. Please call Initialize before any operations.");

            return clientsManager;
        }
    }
    
}
