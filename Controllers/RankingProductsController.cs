using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChannelEngineAssignment.Data;
using ChannelEngineConsoleApp.Data;
using ChannelEngineConsoleApp.Controllers;
using ChannelEngineConsoleApp;
using Newtonsoft.Json;

namespace ChannelEngineAssignment.Controllers
{
    public class RankingProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static string API_PATH = "https://api-dev.channelengine.net/api/";
        private static string API_KEY = "541b989ef78ccb1bad630ea5b85c6ebff9ca3322";
        private static DataController dataController;
        private RankingTaskSolver rankingTaskSolver;
        private ModifyStockTaskSolver modifyStockTaskSolver;

        public RankingProductsController(ApplicationDbContext context)
        {
            _context = context;
            dataController = new DataController(API_PATH, API_KEY);
            rankingTaskSolver = new RankingTaskSolver(dataController);
        }

        // GET: RankingProducts
        public async Task<IActionResult> Index()
        {
              return _context.RankingProduct != null ? 
                          View(await _context.RankingProduct.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.RankingProduct'  is null.");
        }

        // Prints the result of the top-5 ranking on a web page.
        public async Task<IActionResult> GetRanking() {
            await rankingTaskSolver.SolveTask();

            return _context.RankingProduct != null ?
                        View(rankingTaskSolver.Top5Products) :
                        Problem("Entity set 'ApplicationDbContext.RankingProduct'  is null.");
        }

        // Prints the return message of the modify stock PUT request on a web page.
        public async Task<IActionResult> ShowStockOutput() {
            List<Line> products = await dataController.GetInProgressProducts();

            modifyStockTaskSolver = new ModifyStockTaskSolver(
                products[0].MerchantProductNo, products[0].StockLocation.Id, 25, dataController);
            await modifyStockTaskSolver.SolveTask();

            List<RankingProduct> rankingProducts = await dataController.GetRankingProducts();

            return _context.RankingProduct != null ?
                        View(rankingProducts) :
                        Problem("Entity set 'ApplicationDbContext.RankingProduct'  is null.");
        }

        // GET: RankingProducts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.RankingProduct == null)
            {
                return NotFound();
            }

            var rankingProduct = await _context.RankingProduct
                .FirstOrDefaultAsync(m => m.Name == id);
            if (rankingProduct == null)
            {
                return NotFound();
            }

            return View(rankingProduct);
        }

        // GET: RankingProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RankingProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Gtin,Quantity")] RankingProduct rankingProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rankingProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rankingProduct);
        }

        // GET: RankingProducts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.RankingProduct == null)
            {
                return NotFound();
            }

            var rankingProduct = await _context.RankingProduct.FindAsync(id);
            if (rankingProduct == null)
            {
                return NotFound();
            }
            return View(rankingProduct);
        }

        // POST: RankingProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Gtin,Quantity")] RankingProduct rankingProduct)
        {
            if (id != rankingProduct.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rankingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankingProductExists(rankingProduct.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rankingProduct);
        }

        // GET: RankingProducts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.RankingProduct == null)
            {
                return NotFound();
            }

            var rankingProduct = await _context.RankingProduct
                .FirstOrDefaultAsync(m => m.Name == id);
            if (rankingProduct == null)
            {
                return NotFound();
            }

            return View(rankingProduct);
        }

        // POST: RankingProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.RankingProduct == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RankingProduct'  is null.");
            }
            var rankingProduct = await _context.RankingProduct.FindAsync(id);
            if (rankingProduct != null)
            {
                _context.RankingProduct.Remove(rankingProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankingProductExists(string id)
        {
          return (_context.RankingProduct?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
