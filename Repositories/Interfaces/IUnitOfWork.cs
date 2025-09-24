namespace UsersCRUD.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task<int> CompleteAsync();
    }
}
