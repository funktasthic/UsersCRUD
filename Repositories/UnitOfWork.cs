using UsersCRUD.Data;
using UsersCRUD.Repositories.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public IUserRepository Users { get; }

    public UnitOfWork(DataContext context, IUserRepository usersRepository)
    {
        _context = context;
        Users = usersRepository;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
