using AutoMapper;
using Collections.Api.Authorization;
using Collections.Api.Entities;
using Collections.Api.Helpers;
using Collections.Api.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Collections.Api.Services;

public interface IUserService
{
    Task<AuthenticationResponse> Login(LoginRequest model);

    Task<GetUsersResponse> GetUsers(int page, int count);

    Task<AuthenticationResponse> Register(RegisterRequest model);

    Task Block(IEnumerable<int> ids);

    Task Unblock(IEnumerable<int> ids);

    Task Delete(IEnumerable<int> ids);

    Task PromoteToAdmin(IEnumerable<int> ids);

    Task DemoteToUser(IEnumerable<int> ids);

    Task<User?> GetById(int id);
}

public class UserService : IUserService
{
    private readonly DataContext _context;

    private readonly IJwtUtils _jwtUtils;

    private readonly IMapper _mapper;

    public UserService(DataContext context, IJwtUtils jwtUtils, IMapper mapper)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> Register(RegisterRequest model)
    {
        if (await _context.Users.AnyAsync(x => x.Email == model.Email))
        {
            throw new BadHttpRequestException("Email '" + model.Email + "' is already taken");
        }
        var user = _mapper.Map<User>(model);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        var response = _mapper.Map<AuthenticationResponse>(user);
        response.JwtToken = _jwtUtils.GenerateToken(user);
        return response;
    }

    public async Task<AuthenticationResponse> Login(LoginRequest model)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            throw new BadHttpRequestException("Email or password is incorrect");
        }

        if (!user.Status)
        {
            throw new UnauthorizedException("User is blocked");
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        var response = _mapper.Map<AuthenticationResponse>(user);
        response.JwtToken = _jwtUtils.GenerateToken(user);
        return response;
    }

    public async Task<GetUsersResponse> GetUsers(int page, int count)
    {
        var usersCount = await _context.Users.CountAsync();
        var pageCount = (int)Math.Ceiling(usersCount / (double)count);
        var users = await _context.Users.Skip((page - 1) * count).Take(count).ToListAsync();
        return new GetUsersResponse
        {
            UsersCount = usersCount, PagesCount = pageCount, Users = _mapper.Map<List<UserData>>(users)
        };
    }

    public async Task Block(IEnumerable<int> ids)
    {
        await UpdateStatus(ids, false);
    }

    public async Task Unblock(IEnumerable<int> ids)
    {
        await UpdateStatus(ids, true);
    }

    public async Task Delete(IEnumerable<int> ids)
    {
        var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync();
    }

    public async Task PromoteToAdmin(IEnumerable<int> ids)
    {
        await UpdateRole(ids, true);
    }

    public async Task DemoteToUser(IEnumerable<int> ids)
    {
        await UpdateRole(ids, false);
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    private async Task UpdateStatus(IEnumerable<int> ids, bool status)
    {
        var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        users.ForEach(u => u.Status = status);
        await _context.SaveChangesAsync();
    }

    private async Task UpdateRole(IEnumerable<int> ids, bool admin)
    {
        var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        users.ForEach(u => u.Admin = admin);
        await _context.SaveChangesAsync();
    }
}