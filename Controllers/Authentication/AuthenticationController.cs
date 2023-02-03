using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CityInfo.Api.Models.Authtentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        public ActionResult<string> Authenticate(AuthenticationRequestBody requestBody)
        {
            // 1: Validate the credentials
            var user = ValidateUserCredentials(requestBody.Username, requestBody.Password);
            if (user == null) return Unauthorized();

            // 2: Create the token
            var securityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            // 3: Add claims
            var claimsForToken = new List<Claim>();
            // "sub" standarized key for the unique user identifier
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name",  user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));
            
            // 4: Create JWT
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);
            
            // 5: Pass it by the handler
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private CityInfoUser ValidateUserCredentials(string? requestBodyUsername, string? requestBodyPassword)
        {
            // TODO: Implement a DB user table for correct verification against real architecture
            return new CityInfoUser(
                1,
                requestBodyUsername ?? "",
                "Enrique",
                "Nunez",
                "New York");
        }
    }
}