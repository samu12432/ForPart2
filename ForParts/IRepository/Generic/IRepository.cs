namespace ForParts.IRepository.Generic
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Delete(int id);
        void Update(T item, int id);
        T FindById(int id);
        IEnumerable<T> FindAll();

    }

 

}
