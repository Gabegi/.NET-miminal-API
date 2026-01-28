namespace MinimalAPI.Utilities;

/// <summary>
/// Utility for generating consistent cache keys across the application.
/// Ensures single source of truth for cache key naming conventions.
///
/// Cache Version: Bump when DTOs change to invalidate all cached entries.
/// Pattern: {version}:{entity}:{operation}[:{identifier}]
/// Examples:
///   - v1:product:all
///   - v1:product:id:123
///   - v1:product:page:1
///   - v1:order:customer:456
/// </summary>
public static class CacheKeyBuilder
{
    private const string Separator = ":";

    /// <summary>
    /// Set the cache version prefix globally.
    /// Bump this when your DTOs change to avoid deserialization errors.
    /// </summary>
    public static string Version { get; set; } = "v1";

    // ========== PRODUCTS ==========

    /// <summary>
    /// Cache key for all products list.
    /// Example: v1:product:all
    /// </summary>
    public static string ProductsAll() => $"{Version}{Separator}product{Separator}all";

    /// <summary>
    /// Cache key for specific product by ID.
    /// Example: v1:product:id:123
    /// </summary>
    public static string ProductById(int productId) => $"{Version}{Separator}product{Separator}id{Separator}{productId}";

    /// <summary>
    /// Cache key for paginated products.
    /// Example: v1:product:page:1
    /// </summary>
    public static string ProductPage(int pageNumber) => $"{Version}{Separator}product{Separator}page{Separator}{pageNumber}";

    // ========== CUSTOMERS ==========

    /// <summary>
    /// Cache key for all customers list.
    /// Example: v1:customer:all
    /// </summary>
    public static string CustomersAll() => $"{Version}{Separator}customer{Separator}all";

    /// <summary>
    /// Cache key for specific customer by ID.
    /// Example: v1:customer:id:456
    /// </summary>
    public static string CustomerById(int customerId) => $"{Version}{Separator}customer{Separator}id{Separator}{customerId}";

    /// <summary>
    /// Cache key for paginated customers.
    /// Example: v1:customer:page:1
    /// </summary>
    public static string CustomerPage(int pageNumber) => $"{Version}{Separator}customer{Separator}page{Separator}{pageNumber}";

    // ========== ORDERS ==========

    /// <summary>
    /// Cache key for all orders list.
    /// Example: v1:order:all
    /// </summary>
    public static string OrdersAll() => $"{Version}{Separator}order{Separator}all";

    /// <summary>
    /// Cache key for specific order by ID.
    /// Example: v1:order:id:789
    /// </summary>
    public static string OrderById(int orderId) => $"{Version}{Separator}order{Separator}id{Separator}{orderId}";

    /// <summary>
    /// Cache key for orders by customer.
    /// Example: v1:order:customer:456
    /// </summary>
    public static string OrdersByCustomer(int customerId) => $"{Version}{Separator}order{Separator}customer{Separator}{customerId}";

    /// <summary>
    /// Cache key for paginated orders.
    /// Example: v1:order:page:1
    /// </summary>
    public static string OrderPage(int pageNumber) => $"{Version}{Separator}order{Separator}page{Separator}{pageNumber}";

    // ========== UTILITY METHODS ==========

    /// <summary>
    /// Generate cache keys matching a pattern for bulk invalidation.
    /// Example: GetPatternFor("product") → "v1:product:*"
    /// </summary>
    public static string GetPatternFor(string entity) => $"{Version}{Separator}{entity}{Separator}*";

    /// <summary>
    /// Generate cache keys for all lists matching an entity.
    /// Example: GetListPatternFor("product") → "v1:product:all", "v1:product:page:*"
    /// </summary>
    public static string[] GetListPatternsFor(string entity) => new[]
    {
        $"{Version}{Separator}{entity}{Separator}all",
        $"{Version}{Separator}{entity}{Separator}page{Separator}*"
    };

    /// <summary>
    /// Check if a cache key matches a specific entity pattern.
    /// </summary>
    public static bool IsKeyForEntity(string cacheKey, string entity)
    {
        return cacheKey.StartsWith($"{Version}{Separator}{entity}{Separator}");
    }

    /// <summary>
    /// Check if a cache key is a list key (all or page).
    /// </summary>
    public static bool IsListKey(string cacheKey)
    {
        return cacheKey.Contains($"{Separator}all") || cacheKey.Contains($"{Separator}page{Separator}");
    }

    /// <summary>
    /// Extract entity type from cache key.
    /// Example: "v1:product:id:123" → "product"
    /// </summary>
    public static string? ExtractEntity(string cacheKey)
    {
        var parts = cacheKey.Split(Separator);
        return parts.Length >= 2 ? parts[1] : null;
    }
}
