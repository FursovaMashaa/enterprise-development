namespace BikeRental.Application.Contracts;

/// <summary>
/// Generic service interface defining basic CRUD (Create, Read, Update, Delete) operations for entities.
/// Serves as a base interface for all entity-specific service interfaces in the system.
/// </summary>
/// <typeparam name="TDto">DTO type for entity representation</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO type for create/update operations</typeparam>
/// <typeparam name="TId">Type of the entity's primary key</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TId>
    where TDto : class
    where TCreateUpdateDto : class
{
    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="dto">Data for creating the entity</param>
    /// <returns>The created entity DTO</returns>
    public Task<TDto> Create(TCreateUpdateDto dto);
    
    /// <summary>
    /// Retrieves a specific entity by its identifier
    /// </summary>
    /// <param name="dtoId">Entity identifier</param>
    /// <returns>The entity DTO or null if not found</returns>
    public Task<TDto?> Get(TId dtoId);
    
    /// <summary>
    /// Retrieves all entities
    /// </summary>
    /// <returns>List of all entity DTOs</returns>
    public Task<IList<TDto>> GetAll();
    
    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="dto">Updated data for the entity</param>
    /// <param name="dtoId">Identifier of the entity to update</param>
    /// <returns>The updated entity DTO</returns>
    public Task<TDto> Update(TCreateUpdateDto dto, TId dtoId);
    
    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    /// <param name="dtoId">Identifier of the entity to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public Task<bool> Delete(TId dtoId);
}