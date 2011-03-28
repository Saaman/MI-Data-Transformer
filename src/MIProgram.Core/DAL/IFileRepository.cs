namespace MIProgram.DataAccess
{
    public interface IFileRepository<T>
    {
        bool TestDbAccess(ref string message);
        T GetData();
        void WriteData(T data);
    }
}