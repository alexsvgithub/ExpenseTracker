using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ExpenseTracker.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;

        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("No Token Found");
                return;
            }

            if (token != null)
            {
                // If token is present, validate it and set the User in context
                try
                {
                    var jwtSettings = _configuration.GetSection("JwtSettings");
                    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        ClockSkew = TimeSpan.Zero
                    };

                    // Validate the token and set the user in the context
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                    context.User = principal;  // Set user in context
                }
                catch (Exception)
                {
                    // Handle any validation error
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid or expired token.");
                    return;
                }
            }

            // Continue with the next middleware
            await _next(context);

        }

    }
}
