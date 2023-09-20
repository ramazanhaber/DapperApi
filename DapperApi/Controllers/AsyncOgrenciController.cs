using DapperApi.Helper;
using DapperApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
namespace DapperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncOgrenciController : ControllerBase
    {
        private readonly IDbConnection _connection;
        private readonly AsyncDatabaseHelper _databaseHelper;
        private readonly IConfiguration _configuration;
        public AsyncOgrenciController(DatabaseConnections connections, IConfiguration configuration)
        {
            _connection = connections.DefaultConnection; // ilk veri tabanı
            _databaseHelper = new AsyncDatabaseHelper(connections.SecondConnection); // ikinci veri tabanı
            _configuration = configuration;
        }
        [HttpPost]
        [Route("GetOgrenciler")]
        public async Task<ActionResult<IEnumerable<Ogrenciler>>> GetOgrenciler()
        {
            var ogrenciler = await _connection.QueryAsync<Ogrenciler>("SELECT * FROM Ogrenciler");
            return Ok(ogrenciler);
        }
        [HttpPost]
        [Route("GetOgrencilerGenelModel")]
        public async Task<ActionResult<GenelModel>> GetOgrencilerGenelModel()
        {
            var ogrenciler = await _connection.QueryAsync<Ogrenciler>("SELECT * FROM Ogrenciler");
            GenelModel genelModel = new GenelModel();
            genelModel.Data = ogrenciler;
            return Ok(genelModel);
        }
        [HttpPost]
        [Route("GetOgrenciById")]
        public async Task<ActionResult<Ogrenciler>> GetOgrenciById(int id)
        {
            var ogrenci = await _connection.QuerySingleOrDefaultAsync<Ogrenciler>("SELECT * FROM Ogrenciler WHERE id = @id", new { id = id });
            if (ogrenci == null)
            {
                return NotFound();
            }
            return Ok(ogrenci);
        }
        [HttpPost]
        [Route("PostOgrenci")]
        public async Task<IActionResult> PostOgrenci(Ogrenciler ogrenci)
        {
            string query = "INSERT INTO Ogrenciler (Ad, Yas) OUTPUT INSERTED.id VALUES (@Ad, @Yas)";
            await _connection.ExecuteAsync(query, ogrenci);
            return Ok();
        }
        [HttpPost]
        [Route("PostOgrenciDon")]
        public async Task<ActionResult<Ogrenciler>> PostOgrenciDon(Ogrenciler ogrenci)
        {
            string query = "INSERT INTO Ogrenciler (Ad, Yas) OUTPUT INSERTED.id VALUES (@Ad, @Yas)";
            int newId = await _connection.ExecuteScalarAsync<int>(query, ogrenci);
            ogrenci.id = newId;
            return CreatedAtAction(nameof(GetOgrenciById), new { id = ogrenci.id }, ogrenci);
        }
        [HttpPost]
        [Route("UpdateOgrenci")]
        public async Task<IActionResult> UpdateOgrenci(Ogrenciler ogrenci)
        {
            string query = "UPDATE Ogrenciler SET Ad = @Ad, Yas = @Yas WHERE id = @id";
            await _connection.ExecuteAsync(query, ogrenci);
            return Ok();
        }
        [HttpPost]
        [Route("DeleteOgrenci")]
        public async Task<IActionResult> DeleteOgrenci(int id)
        {
            string query = "DELETE FROM Ogrenciler WHERE id = @id";
            await _connection.ExecuteAsync(query, new { id = id });
            return Ok();
        }
        [HttpPost]
        [Route("QueryToJsonveQueryToDataTableveExec")]
        public async Task<IActionResult> QueryToJsonveQueryToDataTableveExec(string query)
        {
            string json = await _databaseHelper.ExecuteQueryToJsonAsync(query);
            DataTable dataTable = await _databaseHelper.ExecuteQueryToDataTableAsync(query);
            bool sonuc = await _databaseHelper.ExecAsync(query);
            return Ok(json);
        }
        [HttpPost]
        [Route("dinamikconnection")]
        public async Task<IActionResult> dinamikconnection(string query)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using IDbConnection dbConnection = new SqlConnection(connectionString);
            var ogrenciler = await dbConnection.QueryAsync<Ogrenciler>("SELECT * FROM Ogrenciler");
            return Ok(ogrenciler);
        }
    }
}
