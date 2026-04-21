using ProjectWasel21.Data;
using ProjectWasel21.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly WaselContext _context;

    public UserService(WaselContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(CreateUserDTO dto)
    {
        var exists = await _context.Users.AnyAsync(x => x.Email == dto.Email);
        if (exists) throw new Exception("Email already exists");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Role = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> GetAll()
        => await _context.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetById(int id)
        => await _context.Users.FindAsync(id);
}