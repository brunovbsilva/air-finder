namespace AirFinder.Application.Common
{
    public interface IGenericService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Insert(T item);
        Task<BaseResponse> Delete(int id);
    }
}
