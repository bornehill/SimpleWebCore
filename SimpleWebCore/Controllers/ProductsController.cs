using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleWebCore.Data;
using SimpleWebCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebCore.Controllers
{
    [Route("api/[Controller]")]
    public class ProductsController: Controller
    {
        private readonly ISimpleWebCoreRepository _repository;
        private readonly ILogger<SimpleWebCoreRepository> _logger;

        public ProductsController(ISimpleWebCoreRepository repository, ILogger<SimpleWebCoreRepository> logger) {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get() {
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception ex) {
                _logger.LogError($"Failed to get products {ex.Message}");
                return BadRequest("Failed to get products");
            }
        }
    }
}
