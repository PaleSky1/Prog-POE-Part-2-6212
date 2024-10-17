using Prog_POE.Models;
using System.Threading.Tasks;
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

        public async Task<IActionResult> SubmitClaims()
        {
            var claim = await _tableStorageService.GetAllClaimsAsync();
            return View(C);
        }

        [HttpGet]
        public IActionResult AddProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProfile(Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                profiles.PartitionKey = "profilePartition";
                profiles.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.addProfilesAsync(profiles);
                return RedirectToAction("Index");
            }
            return View(profiles);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfile(string partitionKey, string rowKey, Profiles profiles)
        {
            if (string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey))
            {
                // Handle error: partitionKey or rowKey is missing
                return BadRequest("PartitionKey or RowKey is missing.");
            }

            await _tableStorageService.DeleteProfilesAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}



