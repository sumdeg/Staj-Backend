using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Staj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private static List<Location> _Location = new List<Location>();


        [HttpPost]
        public string Add(Location location)
        {
            location.id = new Random().Next();
            _Location.Add(location);
            return "Ekleme başarılı";
        }

        [HttpGet]
        public List<Location> GetAll()
        {
            return _Location;
        }


        [HttpGet("{id}")]
        public Location Get(int id)
        {
            return _Location.Find(x => x.id == id);
        }


        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var result = _Location.Find(x => x.id == id);
            _Location.Remove(result);
            return "Silme başarılı";
        }


        [HttpPut("{id}")]
        public string Update(int id, Location location)
        {
            var result = _Location.Find(x => x.id == id);
            result.name = location.name;
            result.x = location.x;
            result.y = location.y;

            return "Güncelleme başarılı";
        }
       

    }
}
