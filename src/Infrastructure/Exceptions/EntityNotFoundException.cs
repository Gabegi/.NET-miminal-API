namespace Infrastructure.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found.
/// </summary>
public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string entityName, object id)
        : base(
            $"{entityName} with ID '{id}' not found.",
            "ENTITY_NOT_FOUND",
            new { EntityName = entityName, Id = id }
        )
    {
    }
}
