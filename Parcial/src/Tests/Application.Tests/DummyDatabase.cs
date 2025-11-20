using Application.Interfaces;

namespace Application.Tests
{
    public class DummyDatabase : IDatabase
    {
        public int ExecuteNonQuery(string sql, object parameters) => 1;
        public string ConnectionString => "Dummy";
    }
}
