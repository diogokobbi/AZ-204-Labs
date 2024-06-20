using BlobUploader.AzureServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlobUploader.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet=true)]
        public string[]? uploadedBlobNames { get; set; } = new string[0];
        private readonly IBlobStorageService _blobStorageService;

        public IndexModel(ILogger<IndexModel> logger, IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> OnGet()
        {
            var blobNames = await _blobStorageService.GetBlobsNamesAsync();
            uploadedBlobNames = blobNames?.ToArray() ?? null;
            return new PageResult();
        }
    }
}