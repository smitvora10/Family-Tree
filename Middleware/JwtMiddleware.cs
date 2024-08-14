//using FamilyTree.Data;
//using System.Text;

//namespace FamilyTree.Middleware
//{
//    public class JwtMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly IConfiguration _configuration;

//        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
//        {
//            _next = next;
//            _configuration = configuration;
//        }

//        public async Task Invoke(HttpContext context, DataContext dbContext)
//        {
//            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//            if (token != null)
//            {
//                AttachUserToContext(context, dbContext, token);
//            }

//            await _next(context);
//        }

//        private void AttachUserToContext(HttpContext context, DataContext dbContext, string token)
//        {
//            try
//            {
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    // Set clock skew to zero so tokens expire exactly at token expiration time
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                var jwtToken = (JwtSecurityToken)validatedToken;
//                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

//                // Attach the user to the context
//                var user = dbContext.User.Include(u => u.UserRole).FirstOrDefault(u => u.Id == userId);
//                context.Items["User"] = user;
//            }
//            catch
//            {
//                // Do nothing if JWT validation fails and user is not attached
//            }
//        }
//    }

//}
