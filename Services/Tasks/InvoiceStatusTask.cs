using Nop.Core.Data;
using Nop.Core.Domain.Orders;
using Nop.Services.Logging;
using Nop.Services.Tasks;
using System;
using System.Linq;
using Tesla.Plugin.Widgets.CRM.Asanito.Client;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Services.Tasks
{
    public class InvoiceStatusTask : IScheduleTask
    {
        #region Fields

        private readonly IAsanitoService _asanitoService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IAsanitoClient _asanitoClient;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public InvoiceStatusTask(IAsanitoService asanitoService, IRepository<Order> orderRepository, IAsanitoClient asanitoClient)
        {
            _asanitoService = asanitoService;
            _orderRepository = orderRepository;
            _asanitoClient = asanitoClient;
        }

        #endregion

        public void Execute()
        {
            var twoDaysAgo = DateTime.UtcNow.AddDays(-2);
            var oneDayAgo = DateTime.UtcNow.AddDays(-1);

            var orders = _orderRepository.Table
                .Where(o => o.CreatedOnUtc > twoDaysAgo && o.CreatedOnUtc <= oneDayAgo)
                .ToList();

            foreach (var order in orders)
            {
                var asanitoInvoice = _asanitoService.GetInvoiceByOrderIdAsync(order.Id).Result;
                if (asanitoInvoice != null)
                {
                    var invoiceStatus = new UpdateInvoiceStatusRequestDto()
                    {
                        Status = 6,
                    };
                    invoiceStatus.InvoiceIds.Add(asanitoInvoice.AsanitoInvoiceId);

                    var updateInvoiceStatusResult = _asanitoClient.UpdateInvoiceStatus(invoiceStatus).Result;
                    if (!updateInvoiceStatusResult)
                    {
                        _logger.Error($"Update invoice status in Asanito failed. OrderId = {order.Id}");
                    }
                }
            }
        }
    }
}