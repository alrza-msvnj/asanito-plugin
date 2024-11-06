using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Tesla.Plugin.Widgets.CRM.Asanito.Models;
using Nop.Services.Configuration;
using Tesla.Plugin.Widgets.CRM.Asanito.Areas.Admin.Key;
using Tesla.Plugin.Widgets.CRM.Asanito.Services;
using Tesla.Plugin.Widgets.CRM.Asanito.Areas.Admin.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;
using Tesla.Plugin.Widgets.CRM.Asanito.Client;
using Nop.Services.Logging;
using Newtonsoft.Json;
using System;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product;
using Nop.Services.Customers;
using Nop.Services.Catalog;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;


namespace Tesla.Plugin.Widgets.CRM.Asanito.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class AsanitoController : BasePluginController
    {

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IAsanitoService _asanitoService;
        private readonly IAsanitoModelFactory _asanitoModelFactory;
        private readonly IAsanitoClient _asanitoClient;
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IRepository<AsanitoCustomerMapping> _asanitoCustomerMappingRepository;
        private readonly IRepository<AsanitoProductMapping> _asanitoProductMappingRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IAddressService _addressService;

        #endregion

        #region Ctor

        public AsanitoController(IWorkContext workContext, ISettingService settingService, IStoreContext storeContext, IAsanitoService asanitoService, IAsanitoModelFactory asanitoModelFactory, IAsanitoClient asanitoClient, ILogger logger, ICustomerService customerService, IProductService productService, IProductAttributeParser productAttributeParser, IRepository<AsanitoCustomerMapping> asanitoCustomerMappingRepository, IRepository<Customer> customerRepository, IAddressService addressService, IRepository<AsanitoProductMapping> asanitoProductMappingRepository)
        {
            _workContext = workContext;
            _settingService = settingService;
            _storeContext = storeContext;
            _asanitoService = asanitoService;
            _asanitoModelFactory = asanitoModelFactory;
            _asanitoClient = asanitoClient;
            _logger = logger;
            _customerService = customerService;
            _productService = productService;
            _productAttributeParser = productAttributeParser;
            _asanitoCustomerMappingRepository = asanitoCustomerMappingRepository;
            _customerRepository = customerRepository;
            _addressService = addressService;
            _asanitoProductMappingRepository = asanitoProductMappingRepository;
        }

        #endregion

        #region Methods

        public IActionResult Configure()
        {
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();

            return View(ViewPathKeys.Configure, settings);
        }

        [HttpPost]
        public IActionResult Configure(AsanitoSettingsModel model)
        {
            var storeId = _storeContext.CurrentStore.Id;
            var settings = _settingService.LoadSetting<AsanitoSettings>(storeId);
            settings = model.ToSettings(settings);
            _settingService.SaveSetting(settings);

            return RedirectToAction(nameof(Configure));
        }

        public IActionResult UpdateCustomer(int limit)
        {
            var count = 0;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            var asanitoCustomerMappingCustomerIds = _asanitoCustomerMappingRepository.Table
                .Select(acm => acm.CustomerId)
                .ToList();
            var customers = _customerRepository.Table
                .Where(c => c.Username != null && !c.Deleted && !c.IsSystemAccount)
                .Select(a => new { a.Id, a.Username, a.BillingAddressId })
                .ToList();

            foreach (var customer in customers)
            {
                if (!asanitoCustomerMappingCustomerIds.Contains(customer.Id))
                {
                    var asanitoCustomerMapping = _asanitoService.GetCustomerByCustomerIdAsync(customer.Id).Result;
                    if (asanitoCustomerMapping == null)
                    {
                        var asanitoCreateCustomerRequest = new CreateCustomerRequestDto
                        {
                            LastName = "ناشناس",
                            OwnerUserId = settings.OwnerUserId
                        };

                        if (customer.BillingAddressId.HasValue)
                        {
                            var billingAddress = _addressService.GetAddressById(customer.BillingAddressId.Value);
                            asanitoCreateCustomerRequest.Name = billingAddress.FirstName;
                            asanitoCreateCustomerRequest.LastName = billingAddress.LastName;
                        }

                        asanitoCreateCustomerRequest.Mobiles.Add(customer.Username);
                        var createCustomerResult = _asanitoClient.CreateCustomer(asanitoCreateCustomerRequest).Result;
                        if (createCustomerResult == null)
                        {
                            _logger.Error($"Create customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomerMapping = new AsanitoCustomerMapping
                            {
                                AsanitoCustomerId = createCustomerResult.Id,
                                CustomerId = customer.Id,
                                FirstName = createCustomerResult.Name,
                                LastName = createCustomerResult.LastName,
                                GenderId = createCustomerResult.GenderId,
                                RawResponse = JsonConvert.SerializeObject(createCustomerResult)
                            };

                            _asanitoService.InsertCustomerAsync(asanitoCustomerMapping).Wait();
                            count++;
                        }
                    }
                }
            }

            return Ok($"{count} customers added");
        }

        public IActionResult UpdateCustomerGenderId(int limit)
        {
            var asanitoCustomers = _asanitoService.GetAsanitoCustomersWithWrongGenderId(limit);
            var count = 0;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            foreach (var customer in asanitoCustomers)
            {
                var aCustomer = new UpdateCustomerRequestDto()
                {
                    Id = customer.AsanitoCustomerId,
                    Name = customer.FirstName,
                    LastName = customer.LastName,
                    OwnerUserId = settings.OwnerUserId
                };
                var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                if (updateCustomerResult == null)
                {
                    _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.CustomerId}");
                }
                else
                {
                    customer.GenderId = 3;
                    var result = _asanitoService.UpdateCustomerAsync(customer).Result;
                    count++;
                }
            }

            return Ok($"{count} customers updated");
        }

        public IActionResult InsertCustomers(int limit)
        {
            var count = 0;
            var body = new GetCustomersRequestDto
            {
                Take = limit
            };
            var getCustomersResult = _asanitoClient.GetCustomers(body).Result;
            if (getCustomersResult == null)
            {
                _logger.Error("Get customers in Asanito failed.");
            }
            else
            {
                foreach (var aCustomer in getCustomersResult.ResultList)
                {
                    var asanitoCustomer = _asanitoService.GetCustomerByAsanitoCustomerIdAsync(aCustomer.Id).Result;
                    if (asanitoCustomer == null)
                    {
                        var customer = _customerService.GetCustomerByUsername(aCustomer.PhoneNumbers[0].Mobile);
                        if (customer != null)
                        {
                            asanitoCustomer = new AsanitoCustomerMapping
                            {
                                AsanitoCustomerId = aCustomer.Id,
                                CustomerId = customer.Id,
                                FirstName = aCustomer.Name,
                                LastName = aCustomer.LastName,
                                GenderId = aCustomer.GenderId,
                            };
                            var result = _asanitoService.InsertCustomerAsync(asanitoCustomer).Result;
                            count++;
                        }
                    }
                }
            }

            return Ok($"{count} customers inserted");
        }

        public IActionResult UpdateParentCategory(int limit)
        {
            var categories = _asanitoService.GetParentCategoriesNotInAsanitoCategoryMapping(limit);
            var count = 0;
            foreach (var category in categories)
            {
                var asanitoCategory = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.Id).Result;
                if (asanitoCategory == null)
                {
                    var aCategory = new CreateCategoryRequestDto()
                    {
                        Title = category.Name
                    };

                    var createCategoryResult = _asanitoClient.CreateCategory(aCategory).Result;
                    if (createCategoryResult == null)
                    {
                        _logger.Error($"Create category in Asanito failed. CategoryId = {category.Id}");
                    }
                    else
                    {
                        asanitoCategory = new AsanitoCategoryMapping()
                        {
                            AsanitoCategoryId = createCategoryResult.Id,
                            CategoryId = category.Id,
                            RawResponse = JsonConvert.SerializeObject(createCategoryResult)
                        };
                        var result = _asanitoService.InsertCategoryAsync(asanitoCategory).Result;
                        count++;
                    }
                }
            }

            return Ok($"{count} categories added");
        }

        public IActionResult UpdateChildCategory(int limit)
        {
            var categories = _asanitoService.GetChildCategoriesNotInAsanitoCategoryMapping(limit);
            var count = 0;
            foreach (var category in categories)
            {
                var asanitoCategory = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.Id).Result;
                if (asanitoCategory == null)
                {
                    var aCategory = new CreateCategoryRequestDto()
                    {
                        Title = category.Name
                    };

                    var createCategoryResult = _asanitoClient.CreateCategory(aCategory).Result;
                    if (createCategoryResult == null)
                    {
                        _logger.Error($"Create category in Asanito failed. CategoryId = {category.Id}");
                    }
                    else
                    {
                        asanitoCategory = new AsanitoCategoryMapping()
                        {
                            AsanitoCategoryId = createCategoryResult.Id,
                            CategoryId = category.Id,
                            RawResponse = JsonConvert.SerializeObject(createCategoryResult)
                        };
                        var result = _asanitoService.InsertCategoryAsync(asanitoCategory).Result;
                        count++;
                    }
                }
            }

            return Ok($"{count} categories added");
        }

        public IActionResult UpdateProduct(int limit)
        {
            var products = _asanitoService.GetProductsNotInAsanitoProductMapping(limit);
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            var count = 0;
            foreach (var product in products)
            {
                var asanitoProduct = _asanitoService.GetAsanitoProductByProductIdAsync(product.Id).Result;
                if (asanitoProduct == null)
                {
                    var category = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(product.MainCategoryId).Result;
                    var aProduct = new CreateProductRequestDto()
                    {
                        Title = product.Name,
                        CategoryId = category.AsanitoCategoryId,
                        InitialBuyPrice = Convert.ToInt32(product.ProductCost * 10),
                        UnitId = settings.ProductUnitId,
                        SellPrice = Convert.ToInt32(product.Price * 10)
                    };
                    aProduct.WarehouseStock.Add(new ProductWarehouseDto()
                    {
                        WarehouseId = settings.WarehouseId,
                        Inventory = product.StockQuantity
                    });

                    var createProductResult = _asanitoClient.CreateProduct(aProduct).Result;
                    if (createProductResult == null)
                    {
                        _logger.Error($"Create product in Asanito failed. ProductId = {product.Id}");
                    }
                    else
                    {
                        asanitoProduct = new AsanitoProductMapping()
                        {
                            AsanitoProductId = createProductResult.Id,
                            ProductId = product.Id,
                            AsanitoCateogryId = createProductResult.Category.Id,
                            CategoryId = product.MainCategoryId,
                            Name = createProductResult.Title,
                            StockQuantity = aProduct.WarehouseStock[0].Inventory,
                            InitialBuyPrice = createProductResult.InitialBuyPrice,
                            SellPrice = createProductResult.SellPrice,
                            AsanitoProductTypeId = createProductResult.ProductType.Id,
                            AsanitoUnitId = createProductResult.UnitId,
                            RawResponse = JsonConvert.SerializeObject(createProductResult)
                        };
                        var result = _asanitoService.InsertProductAsync(asanitoProduct).Result;
                        count++;
                    }
                }
            }

            return Ok($"{count} products added");
        }

        public IActionResult UpdateProductInitialBuyPrice()
        {
            var products = _asanitoService.GetAllProductsAsync();
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            var count = 0;
            foreach (var product in products)
            {
                var productDetails = JsonConvert.DeserializeObject<CreateProductResponseDto>(product.RawResponse);
                var initialBuyPrice = _productService.GetProductById(product.ProductId).ProductCost;
                var aProduct = new UpdateProductRequestDto()
                {
                    Id = product.AsanitoProductId,
                    Title = productDetails.Title,
                    CategoryId = productDetails.Category.Id,
                    Type = productDetails.ProductType.Id,
                    InitialBuyPrice = Convert.ToInt32(initialBuyPrice * 10),
                    UnitId = settings.ProductUnitId,
                    SellPrice = Convert.ToInt32(productDetails.SellPrice)
                };

                var updateProductResult = _asanitoClient.UpdateProduct(aProduct).Result;
                if (updateProductResult == null)
                {
                    _logger.Error($"Update product in Asanito failed. ProductId = {product.ProductId}");
                }
                else
                {
                    product.RawResponse = JsonConvert.SerializeObject(updateProductResult);
                    _asanitoService.UpdateProductAsync(product).Wait();
                    count++;
                }

            }

            return Ok($"{count} products added");
        }

        public IActionResult UpdateProductVariants(int limit)
        {
            var productAttributeCombinations = _asanitoService.GetProductAttributeCombinationsNotInAsanitoProductAsync(limit).Result;
            var count = 0;
            foreach (var productAttributeCombination in productAttributeCombinations)
            {
                var product = productAttributeCombination.Product;
                var asanitoProduct = _asanitoService.GetAsanitoProductByProductIdAsync(productAttributeCombination.ProductId).Result;
                if (asanitoProduct != null)
                {
                    if (asanitoProduct.ProductAttributeCombinationId == 0)
                    {
                        var deleteProductResult = _asanitoClient.DeleteProduct(asanitoProduct.AsanitoProductId).Result;
                        if (!deleteProductResult)
                        {
                            _logger.Error($"Delete product in Asanito failed. ProductId = {productAttributeCombination.ProductId}");
                        }
                        else
                        {
                            var result = _asanitoService.DeleteProductAsync(asanitoProduct).Result;
                        }
                    }

                    var productAttributeValues = _productAttributeParser.ParseProductAttributeValues(productAttributeCombination.AttributesXml);
                    var title = $"{product.Name}";
                    decimal sellPriceDifference = 0;
                    foreach (var attributeValue in productAttributeValues)
                    {
                        title += $" - {attributeValue.Name}";
                        sellPriceDifference += attributeValue.PriceAdjustment;
                    }

                    var category = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(product.MainCategoryId).Result;
                    var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
                    var aProduct = new CreateProductRequestDto()
                    {
                        Title = title,
                        CategoryId = category.AsanitoCategoryId,
                        InitialBuyPrice = Convert.ToInt32(product.ProductCost * 10),
                        UnitId = settings.ProductUnitId,
                        SellPrice = Convert.ToInt32((product.Price + sellPriceDifference) * 10)
                    };

                    aProduct.WarehouseStock.Add(new ProductWarehouseDto()
                    {
                        WarehouseId = settings.WarehouseId,
                        Inventory = productAttributeCombination.StockQuantity
                    });

                    var createProductResult = _asanitoClient.CreateProduct(aProduct).Result;
                    if (createProductResult == null)
                    {
                        _logger.Error($"Create product variant in Asanito failed. ProductCombinationId = {productAttributeCombination.Id}");
                    }
                    else
                    {
                        asanitoProduct = new AsanitoProductMapping()
                        {
                            AsanitoProductId = createProductResult.Id,
                            ProductId = product.Id,
                            ProductAttributeCombinationId = productAttributeCombination.Id,
                            AsanitoCateogryId = createProductResult.Category.Id,
                            CategoryId = product.MainCategoryId,
                            Name = createProductResult.Title,
                            StockQuantity = aProduct.WarehouseStock[0].Inventory,
                            InitialBuyPrice = createProductResult.InitialBuyPrice,
                            SellPrice = createProductResult.SellPrice,
                            AsanitoProductTypeId = createProductResult.ProductType.Id,
                            AsanitoUnitId = createProductResult.UnitId,
                            RawResponse = JsonConvert.SerializeObject(createProductResult)
                        };

                        _asanitoService.InsertProductAsync(asanitoProduct).Wait();
                        count++;
                    }
                }
            }

            return Ok($"{count} product variants added");
        }

        public IActionResult InsertProduct(int limit)
        {
            var count = 0;
            var body = new GetProductRequestDto
            {
                Take = limit
            };
            var getProductsResult = _asanitoClient.GetProducts(body).Result;
            if (getProductsResult == null)
            {
                _logger.Error("Get products in Asanito failed.");
            }
            else
            {
                var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
                foreach (var asanitoProduct in getProductsResult.ResultList)
                {
                    var product = _asanitoService.GetProductByProductNameAsync(asanitoProduct.Title);
                    if (product != null)
                    {
                        var categoryId = _asanitoService.GetCategoryIdByAsanitoCategoryIdAsync(asanitoProduct.Category.Id).Result;
                        var aProduct = new AsanitoProductMapping()
                        {
                            AsanitoProductId = asanitoProduct.Id,
                            ProductId = product.Id,
                            AsanitoCateogryId = asanitoProduct.Category.Id,
                            CategoryId = categoryId,
                            Name = asanitoProduct.Title,
                            StockQuantity = Convert.ToInt32(asanitoProduct.Stock),
                            InitialBuyPrice = asanitoProduct.InitialBuyPrice,
                            SellPrice = asanitoProduct.SellPrice,
                            AsanitoProductTypeId = asanitoProduct.ProductType.Id,
                            AsanitoUnitId = asanitoProduct.UnitId,
                            RawResponse = JsonConvert.SerializeObject(asanitoProduct)
                        };
                        var result = _asanitoService.InsertProductAsync(aProduct).Result;
                        count++;
                    }
                }
            }

            return Ok($"{count} products added");
        }

        public IActionResult UpdateProductInAsanitoPanel(int limit)
        {
            var count = 0;
            var body = new GetProductRequestDto
            {
                Take = limit
            };
            var getProductsResult = _asanitoClient.GetProducts(body).Result;
            if (getProductsResult == null)
            {
                _logger.Error("Get products in Asanito failed.");
            }
            else
            {
                var nameToObject = new Dictionary<string, CreateProductResponseDto>();
                foreach (var asanitoProduct in getProductsResult.ResultList)
                {
                    if (!nameToObject.ContainsKey(asanitoProduct.Title))
                    {
                        nameToObject[asanitoProduct.Title] = asanitoProduct;
                    }
                }

                var uniqueProducts = new List<CreateProductResponseDto>(nameToObject.Values);

                foreach (var asanitoProduct in getProductsResult.ResultList)
                {
                    if (!uniqueProducts.Contains(asanitoProduct))
                    {
                        var deleteProductResult = _asanitoClient.DeleteProduct(asanitoProduct.Id).Result;
                        if (!deleteProductResult)
                        {
                            _logger.Error($"Delete product in Asanito failed. AsanitoProductId = {asanitoProduct.Id}");
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
            }

            return Ok($"{count} products deleted");
        }

        public IActionResult DeleteDuplicateProducts()
        {
            var count = 0;
            var duplicates = _asanitoProductMappingRepository.Table
                .Where(p => p.ProductAttributeCombinationId == 0)
                .GroupBy(p => p.ProductId)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicates)
            {
                // Find the record with the highest AsanitoProductId in the group
                var minAsanitoProductId = group.Min(p => p.AsanitoProductId);

                // Get the records that should be deleted (those with a lower AsanitoProductId)
                var recordsToDelete = group.Where(p => p.AsanitoProductId > minAsanitoProductId).ToList();

                // Delete the records
                foreach (var asanitoProduct in recordsToDelete)
                {
                    var deleteProductResult = _asanitoClient.DeleteProduct(asanitoProduct.AsanitoProductId).Result;
                    if (!deleteProductResult)
                    {
                        _logger.Error($"Delete product in Asanito failed. ProductId = {asanitoProduct.ProductId}");
                    }
                    else
                    {
                        _asanitoService.DeleteProductAsync(asanitoProduct).Wait();
                        count++;
                    }
                }
            }

            return Ok($"{count} products deleted");
        }

    }

    #endregion
}
