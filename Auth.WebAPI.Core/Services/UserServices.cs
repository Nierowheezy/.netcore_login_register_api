using Auth.WebAPI.Core.CustomExceptions;
using Auth.WebAPI.Core.DTOS;
using Auth.WebAPI.Core.Utilities;
using Auth.WebAPI.DB;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.WebAPI.Core.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;


        public UserService( AppDbContext context,IPasswordHasher passwordHasher, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<AuthenticatedUser> SignIn(SignInUserDTO signInUserDTO)
        {
            //map profile to the signInUserDTO
            var user = _mapper.Map<User>(signInUserDTO);

            //check for user in the db by username

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            //check if username or password match 

            if (dbUser == null || _passwordHasher.VerifyHashedPassword(dbUser.Password, signInUserDTO.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }


            return new AuthenticatedUser
            {
                UserName = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username)
            };

        }

        public async Task<AuthenticatedUser> SignUp(SignUpUserDTO signUpUserDTO)
        {
            //map profile to the signInUserDTO

            var user = _mapper.Map<User>(signUpUserDTO);

            //check for user in the db by username

            var checkUserExist = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(user.Username));
            
            //check of user already exist then throw exception

            if (checkUserExist != null)
            {
                throw new UsernameAlreadyExistsException("Username already exists");
            }

            //hash password
            user.Password = _passwordHasher.HashPassword(user.Password);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();


            return new AuthenticatedUser
            {
                UserName = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username)
            };


        }
    }
}
