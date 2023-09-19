using Dapper;
using DapperApi.Helper;
using DapperApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasitOgrenciController : ControllerBase
    {
        private readonly IDbConnection _connection;
        private readonly DatabaseHelper _databaseHelper;


        public BasitOgrenciController(IDbConnection connection)
        {
            _connection = connection;
            _databaseHelper = new DatabaseHelper(connection);

        }

        [HttpPost]
        [Route("GetOgrenciler")]
        public ActionResult<IEnumerable<Ogrenciler>> GetOgrenciler()
        {
            var ogrenciler = _connection.Query<Ogrenciler>("SELECT * FROM Ogrenciler");
            return Ok(ogrenciler);
        }

        [HttpPost]
        [Route("GetOgrenciById")]
        public ActionResult<Ogrenciler> GetOgrenciById(int id)
        {
            var ogrenci = _connection.QuerySingleOrDefault<Ogrenciler>("SELECT * FROM Ogrenciler WHERE id = @id", new { id = id });
            if (ogrenci == null)
            {
                return NotFound();
            }
            return Ok(ogrenci);
        }

        [HttpPost]
        [Route("PostOgrenci")]
        public ActionResult<Ogrenciler> PostOgrenci(Ogrenciler ogrenci)
        {
            string query = "INSERT INTO Ogrenciler (Ad, Yas) OUTPUT INSERTED.id VALUES (@Ad, @Yas)";
            _connection.Execute(query, ogrenci);
            return Ok();
        }

        [HttpPost]
        [Route("PostOgrenciDon")]
        public ActionResult<Ogrenciler> PostOgrenciDon(Ogrenciler ogrenci)
        {
            string query = "INSERT INTO Ogrenciler (Ad, Yas) OUTPUT INSERTED.id VALUES (@Ad, @Yas)";
            int newId = _connection.ExecuteScalar<int>(query, ogrenci);
            ogrenci.id = newId;
            return CreatedAtAction(nameof(GetOgrenciById), new { id = ogrenci.id }, ogrenci);
        }

        [HttpPost]
        [Route("UpdateOgrenci")]
        public IActionResult UpdateOgrenci(Ogrenciler ogrenci)
        {
            string query = "UPDATE Ogrenciler SET Ad = @Ad, Yas = @Yas WHERE id = @id";
            _connection.Execute(query, ogrenci);
            return Ok();
        }

        [HttpPost]
        [Route("DeleteOgrenci")]
        public IActionResult DeleteOgrenci(int id)
        {
            string query = "DELETE FROM Ogrenciler WHERE id = @id";
            _connection.Execute(query, new { id = id });
            return Ok();
        }

        [HttpPost]
        [Route("QueryToJsonveQueryToDataTableveExec")]
        public IActionResult QueryToJsonveQueryToDataTableveExec(string query)
        {
            string json = _databaseHelper.ExecuteQueryToJson(query);
            DataTable dataTable = _databaseHelper.ExecuteQueryToDataTable(query);
            bool sonuc = _databaseHelper.exec(query);

            return Ok(json);
        }

    }
}
