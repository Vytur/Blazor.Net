using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CryptoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoInfoController : ControllerBase
    {
        readonly IConfiguration _config;

        public CryptoInfoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet(Name = "GetCryptos")]
        public JsonResult GetCryptos()
        {
            string query = "select * from dbo.cryptos";
            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("CryptoAppCon");
            SqlDataReader reader;

            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    reader= cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost(Name = "AddCryptos")]
        public JsonResult AddCryptos([FromForm] string newName, [FromForm] string newShortcut)
        {
            string query = "insert into dbo.cryptos values(@newName, @newShortcut)";
            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("CryptoAppCon");
            SqlDataReader reader;

            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@newName", newName);
                    cmd.Parameters.AddWithValue("@newShortcut", newShortcut);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpDelete(Name = "DeleteCryptos")]
        public JsonResult DeleteCryptos(int id)
        {
            string query = "delete from dbo.cryptos where id=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("CryptoAppCon");
            SqlDataReader reader;

            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
