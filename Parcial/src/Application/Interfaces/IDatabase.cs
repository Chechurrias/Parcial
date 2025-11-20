namespace Application.Interfaces
{
    public interface IDatabase
    {
        int ExecuteNonQuery(string sql, object parameters);
    }
}
