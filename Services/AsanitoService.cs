using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.ResponseModel;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Events;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;


namespace Tesla.Plugin.Widgets.CRM.Asanito.Services
{
    public class AsanitoService : IAsanitoService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly IRepository<AsanitoCustomerMapping> _asanitoCustomerMappingRepository;
        private readonly IRepository<AsanitoCategoryMapping> _asanitoCategoryMappingRepository;
        private readonly IRepository<AsanitoProductMapping> _asanitoProductMappingRepository;
        private readonly IRepository<AsanitoInvoiceMapping> _asanitoInvoiceMappingRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductAttributeCombination> _productAttributeCombinationRepository;
        private readonly IRepository<ProductAttributeMapping> _productAttributeMappingRepository;
        private readonly IProductAttributeParser _productAttributeParser;


        #endregion

        #region Ctor

        public AsanitoService(IEventPublisher eventPublisher, IHttpClientFactory httpClientFactory, IStoreContext storeContext, ISettingService settingService, IRepository<AsanitoCustomerMapping> asanitoCustomerMappingRepository, IRepository<AsanitoCategoryMapping> asanitoCategoryMappingRepository, IRepository<AsanitoProductMapping> asanitoProductMappingRepository, IRepository<AsanitoInvoiceMapping> asanitoInvoiceMappingRepository, IRepository<Customer> customerRepository, IRepository<Category> categoryRepository, IRepository<Product> productRepository, IRepository<ProductAttributeCombination> productAttributeCombinationRepository, IRepository<ProductAttributeMapping> productAttributeMappingRepository, IProductAttributeParser productAttributeParser)
        {
            _eventPublisher = eventPublisher;
            _httpClientFactory = httpClientFactory;
            _storeContext = storeContext;
            _settingService = settingService;
            _asanitoCustomerMappingRepository = asanitoCustomerMappingRepository;
            _asanitoCategoryMappingRepository = asanitoCategoryMappingRepository;
            _asanitoProductMappingRepository = asanitoProductMappingRepository;
            _asanitoInvoiceMappingRepository = asanitoInvoiceMappingRepository;
            _customerRepository = customerRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productAttributeCombinationRepository = productAttributeCombinationRepository;
            _productAttributeMappingRepository = productAttributeMappingRepository;
            _productAttributeParser = productAttributeParser;
        }

        #endregion

        #region CRUD

        public async Task<AsanitoCustomerMapping> GetCustomerByCustomerIdAsync(int id)
        {
            var customer = await _asanitoCustomerMappingRepository.Table.FirstOrDefaultAsync(p => p.CustomerId == id);

            return customer;
        }

        public async Task<AsanitoCustomerMapping> GetCustomerByAsanitoCustomerIdAsync(int id)
        {
            var customer = await _asanitoCustomerMappingRepository.Table.FirstOrDefaultAsync(p => p.AsanitoCustomerId == id);

            return customer;
        }

        public IList<AsanitoCustomerMapping> GetAsanitoCustomersWithWrongGenderId(int limit)
        {
            var asanitoCustomers = _asanitoCustomerMappingRepository.Table
                .Where(c => c.GenderId != 3)
                .Take(limit)
                .ToList();

            return asanitoCustomers;
        }

        public async Task<GeneralResponseModel> InsertCustomerAsync(AsanitoCustomerMapping customer)
        {
            var asanitoCustomer = await GetCustomerByCustomerIdAsync(customer.CustomerId);
            if (asanitoCustomer != null)
            {
                return new GeneralResponseModel { Success = false, Error = "Duplicate" };
            }
            else
            {
                _asanitoCustomerMappingRepository.Insert(customer);

                return new GeneralResponseModel { Success = true };
            }
        }

        public async Task<GeneralResponseModel> UpdateCustomerAsync(AsanitoCustomerMapping customer)
        {
            _asanitoCustomerMappingRepository.Update(customer);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<GeneralResponseModel> DeleteCustomerAsync(AsanitoCustomerMapping customer)
        {
            _asanitoCustomerMappingRepository.Delete(customer);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<AsanitoCategoryMapping> GetAsanitoCategoryByCategoryIdAsync(int id)
        {
            var category = await _asanitoCategoryMappingRepository.Table.FirstOrDefaultAsync(c => c.CategoryId == id);

            return category;
        }

        public async Task<int> GetCategoryIdByAsanitoCategoryIdAsync(int id)
        {
            var category = await _asanitoCategoryMappingRepository.Table.FirstOrDefaultAsync(c => c.AsanitoCategoryId == id);

            return category.CategoryId;
        }

        public async Task<GeneralResponseModel> InsertCategoryAsync(AsanitoCategoryMapping category)
        {
            var asanitoCategory = await GetAsanitoCategoryByCategoryIdAsync(category.CategoryId);
            if (asanitoCategory != null)
            {
                return new GeneralResponseModel { Success = false, Error = "Duplicate" };
            }
            else
            {
                _asanitoCategoryMappingRepository.Insert(category);

                return new GeneralResponseModel { Success = true };
            }
        }

        public async Task<GeneralResponseModel> UpdateCategoryAsync(AsanitoCategoryMapping category)
        {
            _asanitoCategoryMappingRepository.Update(category);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<GeneralResponseModel> DeleteCategoryAsync(AsanitoCategoryMapping category)
        {
            _asanitoCategoryMappingRepository.Delete(category);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<AsanitoProductMapping> GetAsanitoProductByProductIdAsync(int id)
        {
            var product = await _asanitoProductMappingRepository.Table.FirstOrDefaultAsync(p => p.ProductId == id);

            return product;
        }

        public Product GetProductByProductNameAsync(string name)
        {
            var product = _productRepository.Table.FirstOrDefault(p => p.Name == name);

            return product;
        }

        public async Task<AsanitoProductMapping> GetProductByProductAttributeCombinationIdAsync(int id)
        {
            var product = await _asanitoProductMappingRepository.Table.FirstOrDefaultAsync(p => p.ProductAttributeCombinationId == id);

            return product;
        }

        public async Task<List<ProductAttributeCombination>> GetProductAttributeCombinationsByProductAttributeValueAsync(ProductAttributeValue productAttributeValue)
        {
            var productAttributeMapping = _productAttributeMappingRepository.GetById(productAttributeValue.ProductAttributeMappingId);
            var productAttributeCombinations = _productAttributeCombinationRepository.Table
                .Where(pac => pac.ProductId == productAttributeMapping.ProductId)
                .ToList();

            var relatedCombinations = new List<ProductAttributeCombination>();
            foreach (var combination in productAttributeCombinations)
            {
                var selectedAttributeValues = _productAttributeParser.ParseProductAttributeValues(combination.AttributesXml);
                if (selectedAttributeValues.Any(x => x.Id == productAttributeValue.Id))
                {
                    relatedCombinations.Add(combination);
                }
            }

            return relatedCombinations;
        }

        public IList<AsanitoProductMapping> GetAllProductsAsync()
        {
            var asanitoProducts = _asanitoProductMappingRepository.Table
                .ToList();

            return asanitoProducts;
        }

        public async Task<GeneralResponseModel> InsertProductAsync(AsanitoProductMapping product)
        {
            _asanitoProductMappingRepository.Insert(product);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<GeneralResponseModel> UpdateProductAsync(AsanitoProductMapping product)
        {
            _asanitoProductMappingRepository.Update(product);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<GeneralResponseModel> DeleteProductAsync(AsanitoProductMapping product)
        {
            _asanitoProductMappingRepository.Delete(product);

            return new GeneralResponseModel { Success = true };
        }

        public async Task<AsanitoInvoiceMapping> GetInvoiceByOrderIdAsync(int id)
        {
            var invoice = await _asanitoInvoiceMappingRepository.Table.FirstOrDefaultAsync(p => p.OrderId == id);

            return invoice;
        }

        public async Task<GeneralResponseModel> InsertInvoiceAsync(AsanitoInvoiceMapping invoice)
        {
            var asanitoInvoice = await GetInvoiceByOrderIdAsync(invoice.OrderId);
            if (asanitoInvoice != null)
            {
                return new GeneralResponseModel { Success = false, Error = "Duplicate" };
            }
            else
            {
                _asanitoInvoiceMappingRepository.Insert(invoice);

                return new GeneralResponseModel { Success = true };
            }
        }

        public IList<Customer> GetCustomersNotInAsanitoCustomerMapping(int limit)
        {
            var mappedCustomerIds = _asanitoCustomerMappingRepository.Table
                                    .Select(acm => acm.CustomerId)
                                    .ToList();

            var customersNotMapped = _customerRepository.Table
                                    .Where(c => c.Username != null && !c.Deleted && !c.IsSystemAccount && !mappedCustomerIds.Contains(c.Id))
                                    .Take(limit)
                                    .ToList();

            return customersNotMapped;
        }

        public IList<Category> GetParentCategoriesNotInAsanitoCategoryMapping(int limit)
        {
            var mappedCategoryIds = _asanitoCategoryMappingRepository.Table
                                    .Select(acm => acm.CategoryId)
                                    .ToList();

            var categoriesNotMapped = _categoryRepository.Table
                                    .Where(c => !mappedCategoryIds.Contains(c.Id) && c.ParentCategoryId == 0)
                                    .Take(limit)
                                    .ToList();

            return categoriesNotMapped;
        }

        public IList<Category> GetChildCategoriesNotInAsanitoCategoryMapping(int limit)
        {
            var mappedCategoryIds = _asanitoCategoryMappingRepository.Table
                                    .Select(acm => acm.CategoryId)
                                    .ToList();

            var categoriesNotMapped = _categoryRepository.Table
                                    .Where(c => !mappedCategoryIds.Contains(c.Id) && c.ParentCategoryId > 0)
                                    .Take(limit)
                                    .ToList();

            return categoriesNotMapped;
        }

        public IList<Product> GetProductsNotInAsanitoProductMapping(int limit)
        {
            var mappedProductIds = _asanitoProductMappingRepository.Table
                                    .Select(apm => apm.ProductId)
                                    .ToList();

            var productsNotMapped = _productRepository.Table
                                    .Where(p => !mappedProductIds.Contains(p.Id) && p.MainCategoryId > 0 && p.Published && !p.Deleted)
                                    .Take(limit)
                                    .ToList();

            return productsNotMapped;
        }

        public async Task<List<ProductAttributeCombination>> GetProductAttributeCombinationsNotInAsanitoProductAsync(int limit)
        {
            var productIds = _asanitoProductMappingRepository.Table.Select(p => p.ProductId).ToList();
            var asanitoProductAttributeCombinationIds = _asanitoProductMappingRepository.Table.Where(p => p.ProductAttributeCombinationId > 0).Select(p => p.ProductAttributeCombinationId).ToList();

            var productAttributeCombinations = _productAttributeCombinationRepository.Table
                .Where(pac => !asanitoProductAttributeCombinationIds.Contains(pac.Id) && productIds.Contains(pac.ProductId))
                .Take(limit)
                .ToList();

            return productAttributeCombinations;
        }

        #endregion
    }
}