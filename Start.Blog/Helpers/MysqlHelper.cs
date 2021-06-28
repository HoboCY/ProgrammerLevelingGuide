using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Start.Blog.Helpers
{
    public class MysqlHelper<T> : ISqlHelper<T>
    {
        private readonly string _connectionString;
        private readonly string _typeName;

        public MysqlHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
            _typeName = typeof(T).Name;
        }

        public async Task<T> GetAsync(int id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var sql = $"SELECT * FROM {_typeName} WHERE Id = @Id";
            return await conn.QuerySingleOrDefaultAsync<T>(sql, new { id });
        }

        public async Task<T> GetAsync(DynamicParameters parameters)
        {
            if(parameters == null || !parameters.ParameterNames.Any()) throw new NullReferenceException("Invalid parameters");
            await using var conn = new MySqlConnection(_connectionString);
            var sql = new StringBuilder($"SELECT * FROM {_typeName} WHERE ");

            var parameterNames = parameters.ParameterNames.ToList();

            foreach(var name in parameterNames)
            {
                sql.Append($"{name}=@{name}");
                if(parameterNames.Last() != name)
                    sql.Append(" AND ");
            }

            return await conn.QuerySingleOrDefaultAsync<T>(sql.ToString(), parameters);
        }

        public async Task<IQueryable<T>> GetAsync(Func<T, bool> expression = null)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var sql = $"SELECT * FROM {_typeName}";
            IQueryable<T> entities;
            if(expression != null)
            {
                entities = (await conn.QueryAsync<T>(sql)).Where(expression).AsQueryable();
            }

            entities = (await conn.QueryAsync<T>(sql)).AsQueryable();

            return entities;
        }

        public async Task AddAsync(T entity)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id").Select(p => p.Name).ToList();
            var sql = $"INSERT INTO {_typeName} ({string.Join(",", properties)}) VALUES ({string.Join(",", properties.Select(p => $"@{p}"))})";
            var count = await conn.ExecuteAsync(sql, entity);
            if(count <= 0) throw new InvalidOperationException("INSERT FAILED");
        }

        public async Task UpdateAsync(T entity)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id").Select(p => $"{p.Name}=@{p.Name}").ToList();
            var sql = $"UPDATE {_typeName} SET {string.Join(",", properties)} WHERE Id = @Id";
            var count = await conn.ExecuteAsync(sql, entity);
            if(count <= 0) throw new InvalidOperationException("UPDATE FAILED");
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var sql = $"DELETE FROM {_typeName} WHERE Id = @Id";
            var count = await conn.ExecuteAsync(sql, new { id });
            if(count <= 0) throw new InvalidOperationException("DELETE FAILED");
        }
    }
}
