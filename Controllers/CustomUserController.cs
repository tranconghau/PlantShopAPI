using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShopAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlantShopAPI.Controllers
{
/*    [Route("api/[controller]")]*/
    [ApiController]
    public class CustomUserController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly CustomDbContext _context;

        public CustomUserController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager,
            CustomDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> register(RegisterUser reUser)
        {
            var user = new CustomUser
            {
                UserName = reUser.Email,
                Email = reUser.Email,
                Role = reUser.Role,
                PasswordHash = reUser.Password
            };

            var result = await _userManager.CreateAsync(user, user.PasswordHash!);
            string message = "Registed successfully";
            if (result.Succeeded)
            {
                return Ok(message);
            }
            return BadRequest("Error occured");
        }

        [HttpPost("/user/login")]
        public async Task<IActionResult> login(string usrname, string pw)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                userName : usrname,
                password : pw,
                isPersistent: false,
                lockoutOnFailure: true);

            if (signInResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(usrname);
                JsonResult json = new JsonResult(new Response
                {
                    status = true,
                    message = "Logged in successfully",
                    data = new UserResponse
                    {
                        username = usrname,
                        role = (user != null ? user.Role : "")
                    }
                });
                /*var token = await _userManager.GetAuthenticationTokenAsync(user, );*/
                return json;
            }
            return BadRequest("Error occured");
        }

        [HttpPost("/logout")]
        public async Task logout()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpPost("/remove_user/{usr}")]
        public async Task<IActionResult> remove(string usr)
        {
            var user = await _userManager.FindByNameAsync(usr);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok("Removed");
                }
                else
                {
                    return BadRequest("Error occured");
                }
            }
            return BadRequest("Error occured");
        }

        /*// GET: api/<CustomUserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomUserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
