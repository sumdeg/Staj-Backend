using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Net;

namespace Staj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationDBController : ControllerBase
    {

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection("Server=localhost;Port=5432;Database=MyTestDB;User Id=postgres;Password=1234");
        }

        [HttpPost]
        public IActionResult Add(Location location)
        {
            using (var conn = GetConnection())
            {
      
                if (string.IsNullOrEmpty(location.name) && location.x == 0 && location.y == 0)
                {
                    return BadRequest("Name, X ve Y alanlarına veri giriniz!");
                }
                else if (string.IsNullOrEmpty(location.name))
                {
                    return BadRequest("Name alanına veri giriniz!");
                }
                else if (location.x == 0)
                {
                    return BadRequest("X alanı 0 olamaz!");
                }
                else if (location.y== 0)
                {
                    return BadRequest("Y alanı 0 olamaz!");
                }
                else
                {
                    var cmd = new NpgsqlCommand("INSERT INTO locations (Name,X,Y) VALUES (@Name,@X,@Y)", conn);
                    cmd.Parameters.AddWithValue("Name", location.name);
                    cmd.Parameters.AddWithValue("X", location.x);
                    cmd.Parameters.AddWithValue("Y", location.y);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok("Ekleme Başarılı");
                }
                
            }
        }

        [HttpGet]
        public List<Location> GetAll()
        {
            List<Location> locations = new List<Location>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM locations order by Id";
                var cmd = new NpgsqlCommand(sql, conn);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Location location = new Location();
                        location.id = reader.GetInt32("Id");
                        location.name = reader.GetString("Name");
                        location.x = reader.GetDouble("X");
                        location.y = reader.GetDouble("Y");

                        locations.Add(location);
                    }
                }
                conn.Close();
            }
            return locations;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var conn = GetConnection();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM locations WHERE Id=@Id",conn);
            cmd.Parameters.AddWithValue("Id", id);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Location location = new Location
                {
                    id = reader.GetInt32("Id"),
                    name = reader.GetString("Name"),
                    x = reader.GetDouble("X"),
                    y = reader.GetDouble("Y")
                };
                conn.Close();
                return Ok(location);
            }
            return BadRequest("Location bulunamadı.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            NpgsqlDataReader dataReader;
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd=new NpgsqlCommand("DELETE FROM locations WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    dataReader = cmd.ExecuteReader();

                    dataReader.Close();
                    conn.Close();
                }
            } 
            return Ok("Silme işlemi başarılı");            
        }

        [HttpPut]
        public IActionResult Update(Location location)
        {

            using (var conn = GetConnection())
            {
                NpgsqlDataReader dataReader;
                conn.Open();
                //if (!string.IsNullOrEmpty(location.name) && location.x!=0 && location.y==0)
                //{
                //    var cmd1 = new NpgsqlCommand("UPDATE locations SET Name = @Name, X = @X WHERE Id = @Id", conn);
                //    cmd1.Parameters.AddWithValue("Id", location.id);
                //    cmd1.Parameters.AddWithValue("Name", location.name);
                //    cmd1.Parameters.AddWithValue("X", location.x);
                //    dataReader = cmd1.ExecuteReader();
                //}
                //else if (!string.IsNullOrEmpty(location.name) && location.y != 0 && location.x==0)
                //{
                //    var cmd2 = new NpgsqlCommand("UPDATE locations SET Name = @Name, Y = @Y WHERE Id = @Id", conn);
                //    cmd2.Parameters.AddWithValue("Id", location.Id);
                //    cmd2.Parameters.AddWithValue("Name", location.Name);
                //    cmd2.Parameters.AddWithValue("Y", location.Y);
                //    dataReader= cmd2.ExecuteReader();
                    
                //}
                //else if(location.X != 0 && location.Y != 0 && string.IsNullOrEmpty(location.Name))
                //{
                //    var cmd3 = new NpgsqlCommand("UPDATE locations SET  X = @X, Y = @Y WHERE Id = @Id", conn);
                //    cmd3.Parameters.AddWithValue("Id", location.Id);
                //    cmd3.Parameters.AddWithValue("X", location.X);
                //    cmd3.Parameters.AddWithValue("Y", location.Y);
                //    dataReader = cmd3.ExecuteReader();
                    
                //}
                //else if (!string.IsNullOrEmpty(location.Name) && location.X == 0 && location.Y == 0)
                //{
                //    var cmd4 = new NpgsqlCommand("UPDATE locations SET Name = @Name WHERE Id = @Id", conn);
                //    cmd4.Parameters.AddWithValue("Id", location.Id);
                //    cmd4.Parameters.AddWithValue("Name", location.Name);
                //    dataReader = cmd4.ExecuteReader();
                   
                //}
                //else if (location.X != 0 && string.IsNullOrEmpty(location.Name) && location.Y == 0)
                //{
                //    var cmd5 = new NpgsqlCommand("UPDATE locations SET  X = @X WHERE Id = @Id", conn);
                //    cmd5.Parameters.AddWithValue("Id", location.Id);
                //    cmd5.Parameters.AddWithValue("X", location.X);
                //    dataReader = cmd5.ExecuteReader();
                    

                //}
                //else if (location.Y != 0 && string.IsNullOrEmpty(location.Name) && location.X == 0)
                //{
                //    var cmd6 = new NpgsqlCommand("UPDATE locations SET  Y =@Y WHERE Id = @Id", conn);
                //    cmd6.Parameters.AddWithValue("Id", location.Id);
                //    cmd6.Parameters.AddWithValue("Y", location.Y);
                //    dataReader = cmd6.ExecuteReader();                    
                //}
                //else
                //{
                    var cmd = new NpgsqlCommand("UPDATE locations SET Name = @Name, X = @X, Y = @Y WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("Id", location.id);
                    cmd.Parameters.AddWithValue("Name", location.name);
                    cmd.Parameters.AddWithValue("X", location.x);
                    cmd.Parameters.AddWithValue("Y", location.y);                 
                    dataReader = cmd.ExecuteReader();
                    
               // }             
                dataReader.Close();
                conn.Close();
                return Ok("Güncelleme işlemi başarılı");
            }
        }
    }
}
