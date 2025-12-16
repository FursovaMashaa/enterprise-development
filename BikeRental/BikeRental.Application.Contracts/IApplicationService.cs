namespace BikeRental.Application.Contracts;

public interface IApplicationService<TDto, TCreateUpdateDto, TId>
    where TDto : class
    where TCreateUpdateDto : class
{
    public Task<TDto> Create(TCreateUpdateDto dto);
    
    public Task<TDto?> Get(TId dtoId);
    
    public Task<IList<TDto>> GetAll();
    
    public Task<TDto> Update(TCreateUpdateDto dto, TId dtoId);
    
    public Task<bool> Delete(TId dtoId);
}