using AutoMapper;
using ForParts.DTOs.Invoice;
using ForParts.Exceptions.Invoice;
using ForParts.IRepository.Invoice;
using ForParts.IService.Invoice;
using ForParts.Models.Customers;
using ForParts.Models.Enums;
using ForParts.Models.Invoice;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;
using ProductAlias = ForParts.Models.Product.Product;
using CustomerAlias = ForParts.Models.Customers.Customer;
using ForParts.IRepository.Customer;

namespace ForParts.Service.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public InvoiceService(IInvoiceRepository invoiceRepo,  IMapper mapper, ICustomerRepository customerRepository)
        {
            _invoiceRepo = invoiceRepo;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task<InvoiceDto> CrearFacturaAsync(InvoiceCreateDto dto)
        {
            //Validar al cliente?
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);

            if (customer == null)
                throw new InvoiceException("Cliente no encontrado.");

            var invoice = new InvoiceAlias
            {
                InvoiceDateCreate = dto.InvoiceDateCreate,
                InvoiceCompany = dto.InvoiceCompany,
                InvoiceBranchOfCompanyId = dto.InvoiceBranchOfCompanyId,
                InvoiceCurrencyCode = dto.InvoiceCurrencyCode,
                TipoCambio = dto.TipoCambio,
                InvoiceDescription = dto.InvoiceDescription,
                InvoiceExpirationDate = dto.InvoiceExpirationDate,
                CustomerId = dto.CustomerId,
                Customer = customer,
                InvoiceState = InvoiceState.Pendiente,
                Items = dto.Items.Select(i => new InvoiceItem
                {
                    ProductCode = i.ProductCode,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Product = new ProductAlias { codeProduct = i.ProductCode }
                }).ToList()
            };

            await _invoiceRepo.AddAsync(invoice);
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<InvoiceDto?> ObtenerFacturaPorIdAsync(int invoiceId)
        {
            var invoice = await _invoiceRepo.GetByIdWithItemsAsync(invoiceId);
            return invoice is null ? null : _mapper.Map<InvoiceDto>(invoice);
        }
    }


}
