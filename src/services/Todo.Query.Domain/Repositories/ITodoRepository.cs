namespace Todo.Query.Domain.Repositories;

public interface ITodoRepository
{
    Task Create(Entities.Todo todo);
    Task Update(Entities.Todo todo);
    Task<IList<Entities.Todo>> GetAll();
    Task<Entities.Todo> GetById(Guid id);
    Task<IList<Entities.Todo>> GetUndone();
    Task<IList<Entities.Todo>> GetDone();
}