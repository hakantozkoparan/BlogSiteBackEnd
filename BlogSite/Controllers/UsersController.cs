using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogSite.Context;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity;
using BlogSite.Security;
using BlogSite.Models.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace BlogSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly NewsContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(NewsContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // Tüm kullanıcıların id, name ve surname ile listeleme
        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.Select(x => new { x.Id, x.Name, x.Surname });
            return Ok(users);
        }

        // id ile kullanıcı listeleme
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        // kayıt ol
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("Kayıt başarılı!");
                }

            return BadRequest(result);
            
            //throw new IdentityException(string.Join(", ", result.Errors.Select(e => e.Description)));
            
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
        }
        // giriş yap
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInModel logInModel)
        {
            var user = await _userManager.FindByEmailAsync(logInModel.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            //var result = await _signInManager.PasswordSignInAsync(user, logInModel.Password, logInModel.RememberMe, true);
            var result = await _signInManager.CheckPasswordSignInAsync(user, logInModel.Password, true);
            if (result.Succeeded)
            {
                // Kullanıcının rollerini al
                var roles = await _userManager.GetRolesAsync(user);

                // Token oluştur
                var token = TokenHandler.CreateToken(_configuration, user, roles);

                // Token'ı dön
                return Ok(token);
            }
            return Unauthorized();
        }

        // Mail adresiyle kullanıcı silme
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(String email)
        {
            var user = await _userManager.FindByEmailAsync($"{email}");
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Kullanıcı silindi");
        }
    }
}


//using BlogSite.Context;
//using BlogSite.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace BlogSite.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly NewsContext _newsContext;
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;

//        public UsersController(NewsContext newsContext)
//        {
//            _newsContext = newsContext;
//        }

//        [HttpGet("{Name}")]
//        public IActionResult Get(string Name)
//        {
//            var dene = _newsContext.Users.Where(u => u.Name == Name)
//                .Select(u => new { u.Id, u.Name, u.Surname })
//                .ToList();


//            //var user = _newsContext.Users.Select(x => new { x.Id, x.Name, x.Surname }).AsNoTracking().ToList(); // Id, isim ve soyisim yazdırma

//            //var userscount = _newsContext.Users.Count(); // Kaç üye var gibi

//            return Ok(dene);
//        }

//    }
//}
