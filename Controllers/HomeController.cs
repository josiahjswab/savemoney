using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using savemoney.Models;
using Google.Apis;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace savemoney.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = ""
                });

                // YOUR CODE SHOULD BE HERE..
                // SAMPLE CODE:
                private static readonly string _applicationName = "MoneySpent";
                private static readonly string SpreadsheetId = "1ApbJqISKbOBGstjBaMLcXzZO83Hn_mBpgiMJv2gcs0Y";
                private static readonly string sheet = "Sheet1";
                private static readonly string range = $"{sheet}!A1:A3";

                var something = await Spreadsheets.Values.Get(SpreadsheetId, range).ExecuteAsync();
                ViewBag.Message = "Something: " + something;
                return Content(something);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
