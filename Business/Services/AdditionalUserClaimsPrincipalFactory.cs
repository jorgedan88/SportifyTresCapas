using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sportify_Back.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sportify_back.Identity
{
    public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public AdditionalUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor) { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Agregar claim personalizado según el perfil del usuario
            if (user.Email != null && user.Email.EndsWith("@sportify.com", StringComparison.OrdinalIgnoreCase))
            {
                identity.AddClaim(new Claim("Profile", "Administrador"));
            }
            else
            {
                identity.AddClaim(new Claim("Profile", "Usuario Final"));
            }

            return identity;
        }
    }
}
