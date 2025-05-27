using System.Drawing.Printing;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using tehnologiinet.Repositories;
using tehnologiinet.Entities;

namespace tehnologiinet.Controllers;

[Route("Factorio")]
[ApiController]
[EnableCors]
public class CategoryController : ControllerBase
{
    private readonly IFactorioRepository _factorioRepository;

    private readonly ApplicationDbContext _context;

    public CategoryController(IFactorioRepository factorioRepository, ApplicationDbContext context)
    {
        _factorioRepository = factorioRepository;
        _context = context;
    }
}