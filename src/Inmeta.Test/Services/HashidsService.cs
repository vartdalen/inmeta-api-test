using HashidsNet;
using Microsoft.Extensions.Configuration;
using Inmeta.Test.Services.Abstractions;

namespace Inmeta.Test.Services
{
    public class HashidsService : IHashidsService
    {
        private readonly IConfiguration _config;
        private readonly Dictionary<Type, IHashids> _hashids = new();

        public HashidsService(IConfiguration config)
        {
            _config = config;
        }

        public IHashids Get(Type entityType)
        {
            if (!_hashids.TryGetValue(entityType, out var hashids))
            {
                var salt = $"{_config["Secrets:Hashids:SaltSecret"]}-{entityType.Name}";
                var length = int.Parse(_config["AppSettings:Hashids:Length"]!);
                hashids = new Hashids(salt, length);
                _hashids.Add(entityType, hashids);
            }
            return hashids;
        }
    }
}