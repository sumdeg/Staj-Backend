using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Staj.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
   
    public class LocationDbContextController : ControllerBase
    {
        private readonly LocationDbContext context;
        public LocationDbContextController(LocationDbContext context)
        {
            this.context = context; 
        }

        [HttpPost]
        public IActionResult Add(Location location)
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
            else if (location.y == 0)
            {
                return BadRequest("Y alanı 0 olamaz!");
            }
            else
            {
                var added = context.Entry(location);
                added.State = EntityState.Added;
                context.SaveChanges();
                return Ok("Ekleme Başarılı");

            }
        }

        [HttpGet]
        public List<Location> GetAll()
        {           
            return context.locations.ToList();
        }

        [HttpGet("{id}")]
        public Location GetById(int id)
        {          
            return context.locations.Find(id);            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {           
            var deleted= context.locations.Find(id);
            if (deleted==null)
            {
                return BadRequest("Silmek istenilen location bulunamadı.");  
            }
            context.locations.Remove(deleted);
            context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Location location)
        {          
             location.id = id;
             context.Entry(location).State = EntityState.Modified;
             context.SaveChanges();
             return Ok("Güncelleme başarılı");            
           
        }
        
    }
}
