using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlabLabs.Api.Data;
using SlabLabs.Api.Models;

namespace SlabLabs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    public UsersController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.User.ToListAsync();
    }
}