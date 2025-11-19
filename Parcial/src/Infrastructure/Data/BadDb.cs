using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data
{
    public static class BadDb
    {
        public static readonly string ConnectionString = BuildConnectionString();

        private static string BuildConnectionString()
        {
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") 
                ?? throw new InvalidOperationException("DB_PASSWORD environment variable not set");
            // Construir la cadena de conexión segura usando el password en variable ambiente
            return $"Server=localhost;Database=master;User Id=sa;Password={password};TrustServerCertificate=True";
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public static IDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            conn.Open();
            // Nota: El llamador debe cerrar esta IDataReader y conexión después del uso
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
