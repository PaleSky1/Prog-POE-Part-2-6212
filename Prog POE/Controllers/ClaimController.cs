using Prog_POE.Models;
using Prog_POE.Services;
using Microsoft.AspNetCore.Mvc;

namespace Prog_POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public ClaimController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        [HttpGet]
        public IActionResult AddClaims()
        {
            return View("SubmitClaims");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaims(Claims claim)
        {
            if (ModelState.IsValid)
            {
                claim.PartitionKey = "claimsPartition";
                claim.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddClaimsAsync(claim);
                return RedirectToAction("Index");
            }
            return View(claim);
        }
    }
}




