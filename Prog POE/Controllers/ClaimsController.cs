using Microsoft.AspNetCore.Mvc;
using Prog_POE.Models;
using Prog_POE.Services;

public class ClaimsController : Controller
{
    private readonly TableStorageService _tableStorageService;

    public ClaimsController(TableStorageService tableStorageService)
    {
        _tableStorageService = tableStorageService;
    }

    [HttpGet]
    public IActionResult SubmitClaim()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitClaim(Claims claim, List<IFormFile> files)
    {
        if (ModelState.IsValid)
        {
            claim.PartitionKey = "claimsPartition";
            claim.RowKey = Guid.NewGuid().ToString();

            claim.LectureName = HttpContext.Session.GetString("LectureName");

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var tempPath = Path.GetTempPath();
                        var filePath = Path.Combine(tempPath, file.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        claim.FileName = file.FileName;
                        claim.FileLink = filePath;
                    }
                }
            }

            await _tableStorageService.AddClaimAsync(claim);
            return RedirectToAction("AllClaims");
        }

        return View(claim);
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var claims = await _tableStorageService.GetAllClaimsAsync();
        return View(claims);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteClaim(string partitionKey, string rowKey)
    {
        if (string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey))
        {
            return BadRequest("PartitionKey or RowKey is missing.");
        }

        await _tableStorageService.DeleteClaimAsync(partitionKey, rowKey);
        return RedirectToAction("AllClaims");
    }

    [HttpGet]
    public async Task<IActionResult> AllClaims()
    {
        var claims = await _tableStorageService.GetAllClaimsAsync();
        return View(claims);
    }
}

