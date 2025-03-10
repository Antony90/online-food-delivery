using StackExchange.Redis;

public class CacheInvalidationHandler
{
    private readonly ICacheService _cache;
    private readonly ISubscriber _pubSub;

    public CacheInvalidationHandler(IConnectionMultiplexer redis, ICacheService cache)
    {
        _cache = cache;
        _pubSub = redis.GetSubscriber();
        _pubSub.Subscribe("cache-invalidate").OnMessage(async channel =>
        {
            var key = channel.Message;
            await _cache.RemoveAsync(key);
        });
    }
}
