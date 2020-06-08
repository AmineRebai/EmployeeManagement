using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement2_2.Controllers
{
    public class ErrorControler : Controller
    {
        public ILogger<ErrorControler> logger { get; }
        public ErrorControler(ILogger<ErrorControler> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatustCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry the ressource you request could not be found";
                    logger.LogWarning($"404 error occured. Path = {statusCodeResult.OriginalPath}" 
                        + $"and QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogError($"The path {exceptionDetails.Path} throw an exception " + $"{exceptionDetails.Error}");
            return View("Error");
        }

    }
}
