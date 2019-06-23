using _0._2_FDMC.Data;
using _0._2_FDMC.ViewModels.Cats;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.ModelBinding;

namespace _0._2_FDMC.Controllers
{
    public class CatController
    {
        private readonly FDMCDbContext db;

        public CatController(FDMCDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Create()
        {
          

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CatCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cat = new Cat
            {
                Name = model.Name,
                Age = model.Age,
                ImageUrl = model.ImageUrl,
                BreedId = int.Parse(model.BreedId),
                DateAdded = DateTime.UtcNow
            };

            this.db.Add(cat);
            this.db.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var cat = this.db.Cats
                .Include(c => c.Breed)
                .FirstOrDefault(c => c.Id == id);

            if (cat == null)
            {
                return NotFound(id);
            }

            var model = new CatDetailsViewModel
            {
                Id = cat.Id,
                Breed = cat.Breed.Name,
                Age = cat.Age,
                Name = cat.Name,
                ImageUrl = cat.ImageUrl
            };

            return View(model);
        }
    }
}
