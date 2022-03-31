﻿using CatalogoWebApp.DataAccess;
using CatalogoWebApp.Models;
using CatalogoWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoWebApp.Controllers
{
    public class AutoresController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IList<Models.NoSQL.Carrera> _carreras;

        public AutoresController(AppDbContext context, 
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            //_carreras = _context.Carreras.OrderBy(c => c.Nombre).ToList();
            _carreras = _unitOfWork.Carreras.GetAll()?.OrderBy(c => c.Nombre).ToList();
        }

        [HttpGet("api/autores")]
        public IActionResult GetAll()
        {
            var autores = _context.Autores.ToList();
            if (!autores.Any()) return BadRequest();
            return Ok(autores.Select(x => new
            {
                x.Codigo,
                x.Nombres,
                x.Apellidos,
                Carrera = x.Carrera.Nombre,
            }));
        }

        // GET: Autores
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.Autores.Include(a => a.Carrera);
            //return View(await appDbContext.ToListAsync());
            var autores = await _unitOfWork.Autores.GetAllAsync();
            var query = from a in autores
                join c in _carreras on a.CarreraId equals c.Codigo
                select new Models.NoSQL.Autor
                {
                    Id = a.Id,
                    Nombres = a.Nombres,
                    Codigo = a.Codigo,
                    Apellidos = a.Apellidos,
                    CarreraId = a.CarreraId,
                    Carrera = c
                };
            return View(query);

        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .Include(a => a.Carrera)
                .ThenInclude(a=>a.Facultad)
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_carreras, "CarreraId", "Nombre");
            return View();
        }

        // POST: Autores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,Codigo,Nombres,Apellidos,CarreraId")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_carreras, "CarreraId", "Nombre", autor.CarreraId);
            return View(autor);
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_carreras, "CarreraId", "Nombre", autor.CarreraId);
            return View(autor);
        }

        // POST: Autores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutorId,Codigo,Nombres,Apellidos,CarreraId")] Autor autor)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.AutorId))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_carreras, "CarreraId", "Nombre", autor.CarreraId);
            return View(autor);
        }

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var autor = await _context.Autores.FindAsync(id);
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.AutorId == id);
        }
    }
}
