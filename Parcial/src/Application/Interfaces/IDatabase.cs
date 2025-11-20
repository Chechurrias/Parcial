namespace Application.Interfaces
{
    public interface IDatabase
    {
        void ExecuteNonQuery(string sql, object parameters);
    }
}
