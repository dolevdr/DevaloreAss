using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RandomUsersApi.Controllers
{
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static LinkedList<Users> users = new LinkedList<Users>();
        private string url = "https://randomuser.me/api/";
        private HttpClient client = new HttpClient();

        // GET: api/<ValuesController>/name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetUsersData(string name)
        {
            var response = await client.GetAsync($"{url}/?results=10?gender={name}");
            string responseBody = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseBody);
            var result = json["results"];
            return Ok(result);
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetMostPopularCountry()
        {
            var response = await client.GetAsync($"{url}/?results=5000");
            string responseBody = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseBody);
            var result = from item in json["results"]
                         select item["location"]?["country"];
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> ListOfMails()
        {
            var response = await client.GetAsync($"{url}/?results=30");
            string responseBody = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseBody);
            var result = from item in json["results"]
                         select item["email"];
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTheOldestUser()
        {
            var response = await client.GetAsync($"{url}/?results=1000");
            string responseBody = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseBody);
            var result = from item in json["results"]
                         orderby item["dob"]?["age"] descending
                         select (item["name"]?["first"], item["dob"]?["age"]);

            return Ok(result.First());
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void CreateNewUser([FromBody] Users value)
        {
            users.AddLast(value);
        }

        // PUT api/<ValuesController>/5
        [HttpGet]
        public IActionResult GetNewUser()
        {
            return Ok(users.Last());
        }

        // DELETE api/<ValuesController>/5
        [HttpPut]
        public void UpdateUserData([FromBody] Users value)
        {
            users.Last().email = value.email;
            users.Last().age = value.age;
            users.Last().name = value.name;
            users.Last().phone = value.phone;
            users.Last().gender = value.gender;
        }
    }
}
