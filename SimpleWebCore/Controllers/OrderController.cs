using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleWebCore.Data;
using SimpleWebCore.Data.Entities;
using SimpleWebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebCore.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController: Controller
    {
        private readonly ISimpleWebCoreRepository _repository;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;

        public OrderController(ISimpleWebCoreRepository repository, ILogger<OrderController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(_repository.GetAllOrders()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders {ex.Message}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);

                if (order != null)
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                else
                    return NotFound($"Order Id : {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order by Id {ex.Message}");
                return BadRequest("Failed to get order by Id");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel model) {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = new Order
                    {
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                    };

                    if (order.OrderDate == DateTime.MinValue)
                        order.OrderDate = DateTime.Now;

                    _repository.AddEntity(order);
                    if (_repository.SaveAll()) {
                        model.OrderId = order.Id;
                        return Created($"/api/order/{model.OrderId}", model);
                    }
                }
                else {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex) {
                _logger.LogError($"Failed to save a new order : {ex.Message}");
            }
            return BadRequest($"Failed to save a new order");
        }
    }
}
