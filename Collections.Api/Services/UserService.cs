using AutoMapper;
using Task4Back.Authorization;
using Task4Back.Entities;
using Task4Back.Helpers;
using Task4Back.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Task4Back.Services
{
    public interface IUserService
    {
        AuthenticationResponse Login(LoginRequest model);

        GetPageResponse GetPage(int page, int count);

        AuthenticationResponse Register(RegisterRequest model);

        void Block(List<int> ids);

        void Unblock(List<int> ids);

        void Delete(List<int> ids);

        User GetById(int id);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;

        private readonly IJwtUtils _jwtUtils;

        private readonly IMapper _mapper;

        public UserService(
            DataContext context,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public AuthenticationResponse Register(RegisterRequest model)
        {
            if (_context.Users.Any(x => x.Email == model.Email))
            {
                throw new BadHttpRequestException("Email '" + model.Email + "' is already taken");
            }

            User user = _mapper.Map<User>(model);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _ = _context.Users.Add(user);
            _ = _context.SaveChanges();

            AuthenticationResponse response = _mapper.Map<AuthenticationResponse>(user);
            response.JwtToken = _jwtUtils.GenerateToken(user);

            return response;
        }

        public AuthenticationResponse Login(LoginRequest model)
        {
            User user = _context.Users.SingleOrDefault(x => x.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                throw new BadHttpRequestException("Email or password is incorrect");
            }

            if (!user.Status)
            {
                throw new UnauthorizedException("User is blocked");
            }

            user.LastLogin = DateTime.Now.ToUniversalTime();

            _ = _context.Users.Update(user);
            _ = _context.SaveChanges();

            AuthenticationResponse response = _mapper.Map<AuthenticationResponse>(user);
            response.JwtToken = _jwtUtils.GenerateToken(user);
            return response;
        }

        public GetPageResponse GetPage(int page, int count)
        {
            int usersCount = _context.Users.Count();
            int pageCount = (int)Math.Ceiling(usersCount / (double)count);
            List<User> users = _context.Users.Skip((page - 1) * count).Take(count).ToList();

            return users.Count == 0
                ? throw new NotFoundException("No users found")
                : new GetPageResponse { PagesCount = pageCount, Users = _mapper.Map<List<UserModel>>(users) };
        }

        public void Block(List<int> ids)
        {
            UpdateStatus(ids, false);
        }

        public void Unblock(List<int> ids)
        {
            UpdateStatus(ids, true);
        }

        public void Delete(List<int> ids)
        {
            _ = _context.Database.ExecuteSqlRaw($"DELETE FROM \"Users\" WHERE \"Id\" IN ({string.Join(", ", ids)})");
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        private void UpdateStatus(List<int> ids, bool status)
        {
            _ = _context.Database.ExecuteSqlRaw($"UPDATE \"Users\" SET \"Status\" = {status} WHERE \"Id\" IN ({string.Join(", ", ids)})");
        }
    }
}