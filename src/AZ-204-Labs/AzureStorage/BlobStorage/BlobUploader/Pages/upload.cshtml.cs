using BlobUploader.AzureServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace BlobUploader.Pages
{
    public class uploadModel : PageModel
    {
        private readonly IBlobStorageService _blobStorageService;
        [BindProperty]
        public IFormFile? FormFile { get; set; }
        [BindProperty]
        public bool OverwriteIfExists { get; set; }
        public uploadModel(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (FormFile != null && !String.IsNullOrEmpty(FormFile.FileName))
            {
                var blobExists = await _blobStorageService.AnyBlobAsync(FormFile.FileName);
                if ((blobExists.HasValue && !blobExists.Value) || OverwriteIfExists) 
                {                     
                    using (var fileStream = new FileStream(FormFile.FileName, FileMode.Create))
                    {
                        var success = await _blobStorageService.PostBlobAsync(fileStream, FormFile.FileName, OverwriteIfExists);
                        if (success)
                        {
                            return new RedirectToPageResult("Index");
                        }
                    }
                }
            }
            return new PageResult();
        }
    }
}
