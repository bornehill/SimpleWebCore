using AutoMapper;
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
    [Route("api/order/{orderId}/items")]
    public class OrderItemsController: Controller
    {
        private readonly ISimpleWebCoreRepository _repository;
        private readonly ILogger<OrderItemsController> _logger ;
        private readonly IMapper _mapper;

        public OrderItemsController(ISimpleWebCoreRepository repository, ILogger<OrderItemsController> logger,IMapper mapper) {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId) {
            var order = _repository.GetOrderById(orderId);
            if (order != null) return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));

            return NotFound($"Failed get Items in order {orderId}");
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int orderId, int Id)
        {
            var order = _repository.GetOrderById(orderId);
            if (order != null) {

                var item = order.Items.Where(i => i.Id == Id).FirstOrDefault();

                if(item!=null)
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
            }
            return NotFound($"Failed get Items in order {orderId}");
        }
    }
}
