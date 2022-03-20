    using Auth.WebAPI.Core.DTOS;
using Auth.WebAPI.DB;
using System.Threading.Tasks;

namespace Auth.WebAPI.Core.Service
{
    public interface IUserService
    {
        Task<AuthenticatedUser>SignUp(SignUpUserDTO signUpUserDTO);
        Task<AuthenticatedUser> SignIn(SignInUserDTO signInUserDTO);

    }
}
