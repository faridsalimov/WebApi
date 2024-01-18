using Microsoft.AspNetCore.Mvc;
using WebApiTask1.Dtos;
using WebApiTask1.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        public static List<Player> Players { get; set; } = new List<Player>
        {
            new Player
            {
                Id = 1,
                City="Baku",
                PlayerName="John",
                Score=95
            },
             new Player
            {
                Id = 2,
                City="NYC",
                PlayerName="Jack",
                Score=98
            },
              new Player
            {
                Id = 3,
                City="Dubai",
                PlayerName="Louis",
                Score=67
            },
                new Player
            {
                Id = 4,
                City="Berlin",
                PlayerName="Patrick",
                Score=65
            },
                  new Player
            {
                Id = 5,
                City="Hong Kong",
                PlayerName="David",
                Score=56
            }
        };

        // GET: api/<PlayerController>
        [HttpGet]
        public IEnumerable<PlayerDto> Get()
        {
            var result = Players.Select(x =>
            {
                return new PlayerDto
                {
                    Id = x.Id,
                    PlayerName = x.PlayerName,
                };
            });
            return result;
        }

        [HttpGet("BestStudents")]
        public IEnumerable<PlayerDto> GetBestStudents()
        {
            var result = Players.Where(x=>x.Score>85).Select(x =>
            {
                return new PlayerDto
                {
                    Id = x.Id,
                    PlayerName = x.PlayerName,
                };
            });
            return result;
        }

        [HttpGet("Search")]
        public IEnumerable<PlayerExtendDto> Search(string key)
        {
            var keyResult = key.ToLower().Trim();
            var result = Players.Where(x => x.City.ToLower().Contains(keyResult)).Select(x =>
            {
                return new PlayerExtendDto
                {
                    Id = x.Id,
                    PlayerName = x.PlayerName,
                    City = x.City,
                    Score = x.Score,
                };
            });
            return result;
        }

        // GET api/<PlayerController>/5
        //[HttpGet("{id}")]
        //public PlayerDto? Get(int id)
        //{
        //    var player = Players.FirstOrDefault(x => x.Id == id);
        //    if (player != null)
        //    {
        //        var dataToReturn = new PlayerDto
        //        {
        //            Id = player.Id,
        //            PlayerName = player.PlayerName
        //        };
        //        return dataToReturn;
        //    }
        //    return null;
        //}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var player = Players.FirstOrDefault(x => x.Id == id);
            if (player != null)
            {
                var dataToReturn = new PlayerDto
                {
                    Id = player.Id,
                    PlayerName = player.PlayerName
                };
                return Ok(dataToReturn);
            }
            return NotFound();
        }

        // POST api/<PlayerController>
        [HttpPost]
        public IActionResult Post([FromBody] PlayerAddDto dto)
        {
            try
            {
                var player = new Player
                {
                    Id = dto.Id,
                    City = dto.City,
                    PlayerName = dto.PlayerName,
                    Score = dto.Score
                };
                Players.Add(player);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<PlayerController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PlayerAddDto dto)
        {
            try
            {
                var item = Players.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return NotFound();
                }
                item.PlayerName = dto.PlayerName;
                item.Score = dto.Score;
                item.City = dto.City;
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = Players.FirstOrDefault(p => p.Id == id);
                if(item == null)
                {
                    return NotFound();      
                }
                Players.Remove(item);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
