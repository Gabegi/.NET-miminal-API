namespace MinimalAPI.Configuration;

/// <summary>
/// Configuration settings for production-grade hybrid caching (L1 in-memory + L2 Redis).
/// </summary>
public class CacheSettings
{
    public const string SectionName = "CacheSettings";

    /// <summary>
    /// Enable or disable caching globally.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// TTL settings (in minutes) for different entity types.
    /// </summary>
    public CacheTtlSettings Ttl { get; set; } = new();

    /// <summary>
    /// Maximum size in bytes for a single cache entry (default: 1MB).
    /// </summary>
    public int MaxPayloadBytes { get; set; } = 1024 * 1024;

    /// <summary>
    /// Maximum key length (default: 256 characters).
    /// </summary>
    public int MaxKeyLength { get; set; } = 256;

    /// <summary>
    /// Redis connection string (optional for distributed caching).
    /// If null, only L1 (in-memory) caching is used (development mode).
    /// Set for production to enable L2 distributed caching.
    /// </summary>
    public string? RedisConnectionString { get; set; }

    /// <summary>
    /// Whether to log cache operations (hit/miss, duration, etc.).
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    /// Target cache hit rate percentage (used for monitoring and alerting).
    /// </summary>
    public int TargetHitRatePercent { get; set; } = 80;

    /// <summary>
    /// L1 (in-memory) cache TTL as a ratio of L2 (Redis) TTL.
    /// Example: 0.5 means L1 expires at 50% of L2 duration.
    /// This ensures L1 syncs with L2 for consistency.
    /// </summary>
    public double L1ToL2Ratio { get; set; } = 0.5;

    /// <summary>
    /// Circuit breaker: Max consecutive Redis failures before fallback to L1 only.
    /// </summary>
    public int RedisFailureThreshold { get; set; } = 5;

    /// <summary>
    /// Circuit breaker: Seconds to wait before retrying Redis after circuit opens.
    /// </summary>
    public int CircuitBreakerTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Redis operation timeout in milliseconds.
    /// Prevents hanging on slow/dead Redis connections.
    /// </summary>
    public int RedisTimeoutMs { get; set; } = 5000;

    /// <summary>
    /// Serialization format for cache entries: "Json" or "MessagePack".
    /// Json: Better for debugging, slower serialization
    /// MessagePack: 5-10x faster, better for high-traffic
    /// </summary>
    public string SerializationFormat { get; set; } = "Json";

    /// <summary>
    /// Cache key version prefix (bump when DTOs change to avoid deserialization errors).
    /// Changing this invalidates all existing cached entries.
    /// </summary>
    public string CacheVersion { get; set; } = "v1";

    /// <summary>
    /// Max time (milliseconds) to wait for cache lock during stampede protection.
    /// Prevents multiple simultaneous DB hits when cache expires.
    /// </summary>
    public int StampedeLockTimeoutMs { get; set; } = 10000;
}

/// <summary>
/// TTL configuration for different entity types and operations.
/// Differentiated based on update frequency and data stability.
/// </summary>
public class CacheTtlSettings
{
    // PRODUCTS (Stable data)
    /// <summary>
    /// TTL for product list cache (minutes).
    /// Shorter TTL because full list changes when any product is added/modified.
    /// </summary>
    public int ProductsListMinutes { get; set; } = 5;

    /// <summary>
    /// TTL for individual product cache (minutes).
    /// Longer TTL because individual items don't change as frequently.
    /// </summary>
    public int ProductsItemMinutes { get; set; } = 60;

    // CUSTOMERS (Stable data)
    /// <summary>
    /// TTL for customer list cache (minutes).
    /// Shorter TTL for list consistency.
    /// </summary>
    public int CustomersListMinutes { get; set; } = 5;

    /// <summary>
    /// TTL for individual customer cache (minutes).
    /// </summary>
    public int CustomersItemMinutes { get; set; } = 60;

    // ORDERS (Frequently changing data)
    /// <summary>
    /// TTL for order list cache (minutes).
    /// Shorter TTL due to frequent status changes and order additions.
    /// </summary>
    public int OrdersListMinutes { get; set; } = 5;

    /// <summary>
    /// TTL for individual order cache (minutes).
    /// Shorter than products because orders change status frequently.
    /// </summary>
    public int OrdersItemMinutes { get; set; } = 10;

    /// <summary>
    /// TTL for orders filtered by customer (minutes).
    /// Related to order item TTL since same invalidation triggers apply.
    /// </summary>
    public int OrdersByCustomerMinutes { get; set; } = 10;
}
