using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace DapperApi.Helper
{
    public class DatabaseHelper
    {
        private readonly IDbConnection _connection;

        public DatabaseHelper(IDbConnection connection)
        {
            _connection = connection;
        }

        public DataTable ExecuteQueryToDataTable(string query)
        {
            var reader = _connection.ExecuteReader(query);
            var resultTable = new DataTable();
            resultTable.Load(reader);
            return resultTable;
        }

        public string ExecuteQueryToJson(string query)
        {
            var result = _connection.Query<dynamic>(query).ToList();
            string jsonResult = JsonConvert.SerializeObject(result);
            return jsonResult;
        }

        public bool exec(string query)
        {
            try
            {
                _connection.Execute(query);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string ConvertDataTableToJson(DataTable table)
        {
            return JsonConvert.SerializeObject(table);
        }
    }
}
