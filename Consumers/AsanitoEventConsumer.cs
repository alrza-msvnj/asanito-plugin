using DNTPersianUtils.Core;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Logging;
using System;
using System.Collections.Generic;
using Tesla.Plugin.Widgets.CRM.Asanito.Areas.Admin.Factories;
using Tesla.Plugin.Widgets.CRM.Asanito.Client;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Warehouse;
using Tesla.Plugin.Widgets.CRM.Asanito.Services;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Consumers
{
    public class AsanitoEventConsumer : IConsumer<CustomerRegisteredEvent>,
        IConsumer<EntityInsertedEvent<Category>>,
        IConsumer<EntityUpdatedEvent<Category>>,
        IConsumer<EntityDeletedEvent<Category>>,
        IConsumer<EntityInsertedEvent<Product>>,
        IConsumer<EntityUpdatedEvent<Product>>,
        IConsumer<EntityDeletedEvent<Product>>,
        IConsumer<OrderPaidEvent>,
        IConsumer<OrderPlacedEvent>,
        IConsumer<EntityUpdatedEvent<Order>>,
        IConsumer<EntityDeletedEvent<Customer>>,
        IConsumer<EntityInsertedEvent<GenericAttribute>>,
        IConsumer<EntityUpdatedEvent<GenericAttribute>>,
        IConsumer<EntityInsertedEvent<ProductAttributeCombination>>,
        IConsumer<EntityUpdatedEvent<ProductAttributeCombination>>,
        IConsumer<EntityUpdatedEvent<ProductAttributeValue>>,
        IConsumer<EntityDeletedEvent<ProductAttributeCombination>>,
        IConsumer<OrderCancelledEvent>


    {
        #region Fields

        private readonly IAsanitoModelFactory _asanitoModelFactory;
        private readonly IAsanitoService _asanitoService;
        private readonly IAsanitoClient _asanitoClient;
        private readonly ILogger _logger;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerService _customerService;
        private readonly IProductAttributeParser _productAttributeParser;
        

        #endregion

        #region Ctor

        public AsanitoEventConsumer(IAsanitoModelFactory asanitoModelFactory, IAsanitoService asanitoService, IAsanitoClient asanitoClient, ILogger logger, IGenericAttributeService genericAttributeService, ICustomerService customerService, IProductAttributeParser productAttributeParser)
        {
            _asanitoModelFactory = asanitoModelFactory;
            _asanitoService = asanitoService;
            _asanitoClient = asanitoClient;
            _logger = logger;
            _genericAttributeService = genericAttributeService;
            _customerService = customerService;
            _productAttributeParser = productAttributeParser;
        }

        #endregion

        #region Methods

        public void HandleEvent(CustomerRegisteredEvent eventMessage)
        {
            var customer = eventMessage.Customer;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            if (settings.CustomerCreationTypeId == (int)CustomerCreationType.Registration)
            {
                var asanitoCustomer = _asanitoService.GetCustomerByCustomerIdAsync(customer.Id).Result;
                if (asanitoCustomer == null)
                {
                    var aCustomer = new CreateCustomerRequestDto()
                    {
                        OwnerUserId = settings.OwnerUserId
                    };
                    aCustomer.Mobiles.Add(customer.Username);
                    var createCustomerResult = _asanitoClient.CreateCustomer(aCustomer).Result;
                    if (createCustomerResult == null)
                    {
                        _logger.Error($"Create customer in Asanito failed. CustomerId = {customer.Id}");
                    }
                    else
                    {
                        asanitoCustomer = new AsanitoCustomerMapping()
                        {
                            AsanitoCustomerId = createCustomerResult.Id,
                            CustomerId = customer.Id,
                            FirstName = createCustomerResult.Name,
                            LastName = createCustomerResult.LastName,
                            GenderId = createCustomerResult.GenderId,
                            RawResponse = JsonConvert.SerializeObject(createCustomerResult)
                        };
                        var result = _asanitoService.InsertCustomerAsync(asanitoCustomer).Result;
                    }
                }
            }
        }

        public void HandleEvent(EntityInsertedEvent<Category> eventMessage)
        {
            var category = eventMessage.Entity;
            var asanitoCategory = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.Id).Result;
            if (asanitoCategory == null)
            {
                var parent = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.ParentCategoryId).Result;
                var aCategory = new CreateCategoryRequestDto()
                {
                    Title = category.Name
                };
                if (parent != null)
                {
                    aCategory.ParentId = parent.AsanitoCategoryId;
                }

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
                }
            }
        }

        public void HandleEvent(EntityUpdatedEvent<Category> eventMessage)
        {
            var category = eventMessage.Entity;
            var asanitoCategory = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.Id).Result;
            if (asanitoCategory != null)
            {
                var parent = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.ParentCategoryId).Result;
                var aCategory = new UpdateCategoryRequestDto()
                {
                    Id = asanitoCategory.AsanitoCategoryId,
                    Title = category.Name,
                };
                if (parent != null)
                {
                    aCategory.ParentId = parent.AsanitoCategoryId;
                }

                var updateCategoryResult = _asanitoClient.UpdateCategory(aCategory).Result;
                if (updateCategoryResult == null)
                {
                    _logger.Error($"Update category in Asanito failed. CategoryId = {category.Id}");
                }
                else
                {
                    asanitoCategory.RawResponse = JsonConvert.SerializeObject(updateCategoryResult);
                    var result = _asanitoService.UpdateCategoryAsync(asanitoCategory).Result;
                }
            }
        }

        public void HandleEvent(EntityDeletedEvent<Category> eventMessage)
        {
            var category = eventMessage.Entity;
            var asanitoCategory = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(category.Id).Result;
            if (asanitoCategory != null)
            {
                var deleteCategoryResult = _asanitoClient.DeleteCategory(asanitoCategory.AsanitoCategoryId).Result;
                if (!deleteCategoryResult)
                {
                    _logger.Error($"Delete category in Asanito failed. CategoryId = {category.Id}");
                }
                else
                {
                    var result = _asanitoService.DeleteCategoryAsync(asanitoCategory).Result;
                }
            }
        }

        public void HandleEvent(EntityInsertedEvent<Product> eventMessage)
        {
            var product = eventMessage.Entity;
            var asanitoProduct = _asanitoService.GetAsanitoProductByProductIdAsync(product.Id).Result;
            if (asanitoProduct == null)
            {
                if (product.MainCategoryId > 0)
                {
                    var category = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(product.MainCategoryId).Result;
                    var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
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
                    }
                }
            }
        }

        public void HandleEvent(EntityUpdatedEvent<Product> eventMessage)
        {
            var product = eventMessage.Entity;
            var asanitoProduct = _asanitoService.GetAsanitoProductByProductIdAsync(product.Id).Result;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            if (asanitoProduct != null)
            {
                var aWarehouse = new List<UpdateWarehouseRequestDto>();
                aWarehouse.Add(new UpdateWarehouseRequestDto()
                {
                    Id = _asanitoClient.GetWarehouseStock(asanitoProduct.AsanitoProductId).Result.Id,
                    WarehouseId = settings.WarehouseId,
                    Inventory = product.StockQuantity
                });

                var updateWarehouseResult = _asanitoClient.UpdateWarehouse(aWarehouse, asanitoProduct.AsanitoProductId).Result;
                if (!updateWarehouseResult)
                {
                    _logger.Error($"Update product stock quantity in Asanito failed. ProductId = {product.Id}");
                }

                var category = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(product.MainCategoryId).Result;
                var aProduct = new UpdateProductRequestDto()
                {
                    Id = asanitoProduct.AsanitoProductId,
                    Title = product.Name,
                    CategoryId = category.AsanitoCategoryId,
                    Type = 1,
                    InitialBuyPrice = Convert.ToInt32(product.ProductCost * 10),
                    UnitId = settings.ProductUnitId,
                    SellPrice = Convert.ToInt32(product.Price * 10)
                };

                var updateProductResult = _asanitoClient.UpdateProduct(aProduct).Result;
                if (updateProductResult == null)
                {
                    _logger.Error($"Update product in Asanito failed. ProductId = {product.Id}");
                }
                else
                {
                    asanitoProduct.AsanitoCateogryId = updateProductResult.Category.Id;
                    asanitoProduct.CategoryId = product.MainCategoryId;
                    asanitoProduct.Name = updateProductResult.Title;
                    asanitoProduct.StockQuantity = aWarehouse[0].Inventory;
                    asanitoProduct.InitialBuyPrice = updateProductResult.InitialBuyPrice;
                    asanitoProduct.SellPrice = updateProductResult.SellPrice;
                    asanitoProduct.AsanitoProductTypeId = updateProductResult.ProductType.Id;
                    asanitoProduct.AsanitoUnitId = updateProductResult.UnitId;
                    asanitoProduct.RawResponse = JsonConvert.SerializeObject(updateProductResult);
                    var result = _asanitoService.UpdateProductAsync(asanitoProduct).Result;
                }
            }
            else
            {
                if (product.MainCategoryId > 0 && !product.Deleted)
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
                    }
                }
            }
        }

        public void HandleEvent(EntityDeletedEvent<Product> eventMessage)
        {
            var product = eventMessage.Entity;
            var asanitoProduct = _asanitoService.GetAsanitoProductByProductIdAsync(product.Id).Result;
            if (asanitoProduct != null)
            {
                var deleteProductResult = _asanitoClient.DeleteProduct(asanitoProduct.AsanitoProductId).Result;
                if (!deleteProductResult)
                {
                    _logger.Error($"Delete product in Asanito failed. ProductId = {product.Id}");
                }
                else
                {
                    var result = _asanitoService.DeleteProductAsync(asanitoProduct).Result;
                }
            }
        }

        public void HandleEvent(OrderPaidEvent eventMessage)
        {
            var order = eventMessage.Order;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            if (settings.InvoiceCreationTypeId == (int)InvoiceCreationType.OrderPaid)
            {
                var asanitoInvoice = _asanitoService.GetInvoiceByOrderIdAsync(order.Id).Result;
                if (asanitoInvoice == null)
                {
                    var currentDateTime = DateTime.Now;
                    var persianDate = currentDateTime.ToPersianYearMonthDay();
                    string formattedDate = $"{persianDate.Month:00}_{persianDate.Day:00}_{persianDate.Year} {currentDateTime.Hour:00}:{currentDateTime.Minute:00}";
                    var invoice = new CreateSettledInvoiceRequestDto()
                    {
                        Title = $"فاکتور شماره {order.Id}",
                        OrganizationId = settings.OrganizationId,
                        Date = formattedDate,
                        PersonId = _asanitoService.GetCustomerByCustomerIdAsync(order.Customer.Id).Result.AsanitoCustomerId,
                        BankAccountId = settings.BankAccountId,
                        OwnerUserId = settings.OwnerUserId,
                        SellDate = formattedDate
                    };
                    foreach (var item in order.OrderItems)
                    {
                        var discountPercent = 0;
                        var discountAmount = 0;
                        if (item.DiscountAmountExclTax > 0)
                        {
                            discountPercent = Convert.ToInt32(item.DiscountAmountExclTax * 100 / item.UnitPriceExclTax);
                            discountAmount = Convert.ToInt32(item.DiscountAmountExclTax);
                        }
                        invoice.Items.Add(new InvoiceItemDto()
                        {
                            ProductId = _asanitoService.GetAsanitoProductByProductIdAsync(item.ProductId).Result.AsanitoProductId,
                            Title = item.Product.Name,
                            Count = item.Quantity,
                            UnitPrice = Convert.ToInt32(item.UnitPriceExclTax * 10),
                            DiscountType = item.DiscountAmountExclTax > 0,
                            DiscountPercent = discountPercent,
                            DiscountAmount = discountAmount * 10,
                            TotalDiscountAmount = Convert.ToInt32(item.DiscountAmountExclTax * 10),
                            ProductUnitId = settings.ProductUnitId,
                            HostWarehouseId = settings.WarehouseId,
                            ProductType = 1
                        });
                    }
                    if (order.OrderShippingExclTax > 0)
                    {
                        invoice.AdditionDeductions.Add(new InvoiceAdditionDeductionDto
                        {
                            Title = "هزینه ارسال",
                            Type = 1,
                            CalcType = false,
                            Amount = Convert.ToInt32(order.OrderShippingExclTax * 10)
                        });
                    }
                    invoice.CashPayments.Add(new InvoiceCashPaymentDto
                    {
                        Date = formattedDate,
                        AccountId = settings.BankAccountId
                    });

                    var createSettledInvoiceResult = _asanitoClient.CreateSettledInvoice(invoice).Result;
                    if (createSettledInvoiceResult == null)
                    {
                        _logger.Error($"Create settled invoice in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        asanitoInvoice = new AsanitoInvoiceMapping()
                        {
                            AsanitoInvoiceId = createSettledInvoiceResult.Id,
                            OrderId = order.Id,
                            RawResponse = JsonConvert.SerializeObject(createSettledInvoiceResult)
                        };
                        var result = _asanitoService.InsertInvoiceAsync(asanitoInvoice).Result;
                    }
                }
            }
            else if (settings.InvoiceCreationTypeId == (int)InvoiceCreationType.OrderPlaced)
            {
                var asanitoInvoice = _asanitoService.GetInvoiceByOrderIdAsync(order.Id).Result;
                if (asanitoInvoice != null)
                {
                    var currentDateTime = DateTime.Now;
                    var persianDate = currentDateTime.ToPersianYearMonthDay();
                    string formattedDate = $"{persianDate.Month:00}_{persianDate.Day:00}_{persianDate.Year} {currentDateTime.Hour:00}:{currentDateTime.Minute:00}";
                    var invoiceStatus = new UpdateInvoiceStatusRequestDto()
                    {
                        Status = 3,
                        SellDate = formattedDate
                    };
                    invoiceStatus.InvoiceIds.Add(asanitoInvoice.AsanitoInvoiceId);

                    var updateInvoiceStatusResult = _asanitoClient.UpdateInvoiceStatus(invoiceStatus).Result;
                    if (!updateInvoiceStatusResult)
                    {
                        _logger.Error($"Update invoice status in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        var income = new CreateOperationIncomeRequestDto()
                        {
                            PersonId = _asanitoService.GetCustomerByCustomerIdAsync(order.Customer.Id).Result.AsanitoCustomerId
                        };
                        income.Invoices.Add(new InvoiceInvoiceDto
                        {
                            Amount = JsonConvert.DeserializeObject<CreateInvoiceResponseDto>(asanitoInvoice.RawResponse).RemainedAmount,
                            InvoiceId = asanitoInvoice.AsanitoInvoiceId
                        });
                        income.CashPayments.Add(new InvoiceCashPaymentDto
                        {
                            Date = formattedDate,
                            Amount = income.Invoices[0].Amount,
                            AccountId = settings.BankAccountId
                        });

                        var createOperationIncomeResult = _asanitoClient.CreateOperationIncome(income).Result;
                        if (createOperationIncomeResult == null)
                        {
                            _logger.Error($"Create operation income in Asanito failed. OrderId = {order.Id}");
                        }
                    }
                }
            }
        }

        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            if (settings.CustomerCreationTypeId == (int)CustomerCreationType.Registration)
            {
                var asanitoCustomer = _asanitoService.GetCustomerByCustomerIdAsync(order.Customer.Id).Result;
                if (asanitoCustomer != null)
                {
                    var aCustomer = new UpdateCustomerRequestDto()
                    {
                        Id = asanitoCustomer.AsanitoCustomerId,
                        Name = order.BillingAddress.FirstName,
                        LastName = order.BillingAddress.LastName,
                        OwnerUserId = settings.OwnerUserId
                    };
                    var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                    if (updateCustomerResult == null)
                    {
                        _logger.Error($"Update customer in Asanito failed. CustomerId = {order.Customer.Id}");
                    }
                    else
                    {
                        asanitoCustomer.FirstName = aCustomer.Name;
                        asanitoCustomer.LastName = aCustomer.LastName;
                        asanitoCustomer.GenderId = aCustomer.GenderId;
                        asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                        var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                    }

                    var asanitoCustomerAddress = new CreateCustomerAddressRequestDto
                    {
                        ContactId = asanitoCustomer.AsanitoCustomerId,
                        Address = order.BillingAddress.Address1
                    };

                    var province = order.BillingAddress.StateProvince.Name.Trim();
                    var asanitoProvince = new GetProvinceAndCityRequestDto
                    {
                        Value = province
                    };

                    var getProvinceResult = _asanitoClient.GetProvince(asanitoProvince).Result;
                    if (getProvinceResult == null)
                    {
                        _logger.Error($"Get province in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        var asanitoProvinceId = getProvinceResult[0].Id;
                        var city = order.BillingAddress.City.Trim();
                        var asanitoCity = new GetProvinceAndCityRequestDto
                        {
                            Value = city
                        };

                        var getCityResult = _asanitoClient.GetCity(asanitoCity, asanitoProvinceId).Result;
                        if (getCityResult == null)
                        {
                            _logger.Error($"Get city in Asanito failed. OrderId = {order.Id}");
                        }
                        else
                        {
                            try
                            {
                                asanitoCustomerAddress.CityId = getCityResult[0].Id;
                                var createCustomerAddressResult = _asanitoClient.CreateCustomerAddress(asanitoCustomerAddress).Result;
                                if (createCustomerAddressResult == null)
                                {
                                    _logger.Error($"Create customer address in Asanito failed. OrderId = {order.Id}");
                                }
                            }
                            catch (Exception)
                            {
                                asanitoCustomerAddress.CityId = 82;
                                asanitoCustomerAddress.Address = $"{order.BillingAddress.StateProvince.Name.Trim()} {order.BillingAddress.City.Trim()} {order.BillingAddress.Address1}";
                                var createCustomerAddressResult = _asanitoClient.CreateCustomerAddress(asanitoCustomerAddress).Result;
                                if (createCustomerAddressResult == null)
                                {
                                    _logger.Error($"Create customer address in Asanito failed. OrderId = {order.Id}");
                                }
                            }
                        }
                    }
                }
                else if (asanitoCustomer == null)
                {
                    var aCustomer = new CreateCustomerRequestDto()
                    {
                        Name = order.BillingAddress.FirstName,
                        LastName = order.BillingAddress.LastName
                    };
                    aCustomer.Mobiles.Add(order.Customer.Username);

                    var province = order.BillingAddress.StateProvince.Name.Trim();
                    var asanitoProvince = new GetProvinceAndCityRequestDto
                    {
                        Value = province
                    };

                    var getProvinceResult = _asanitoClient.GetProvince(asanitoProvince).Result;
                    if (getProvinceResult == null)
                    {
                        _logger.Error($"Get province in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        var asanitoProvinceId = getProvinceResult[0].Id;
                        var city = order.BillingAddress.City.Trim();
                        var asanitoCity = new GetProvinceAndCityRequestDto
                        {
                            Value = city
                        };

                        var getCityResult = _asanitoClient.GetCity(asanitoCity, asanitoProvinceId).Result;
                        if (getCityResult == null)
                        {
                            _logger.Error($"Get city in Asanito failed. OrderId = {order.Id}");
                        }
                        else
                        {
                            var asanitoCustomerAddress = new CustomerAddressDto();

                            try
                            {
                                asanitoCustomerAddress.CityId = getCityResult[0].Id;
                                asanitoCustomerAddress.Address = order.BillingAddress.Address1;
                            }
                            catch (Exception)
                            {
                                asanitoCustomerAddress.CityId = 82;
                                asanitoCustomerAddress.Address = $"{order.BillingAddress.StateProvince.Name.Trim()} {order.BillingAddress.City.Trim()} {order.BillingAddress.Address1}";
                            }
                            
                            aCustomer.Addresses.Add(asanitoCustomerAddress);
                            var createCustomerResult = _asanitoClient.CreateCustomer(aCustomer).Result;
                            if (createCustomerResult == null)
                            {
                                _logger.Error($"Create customer in Asanito failed. CustomerId = {order.Customer.Id}");
                            }
                            else
                            {
                                asanitoCustomer = new AsanitoCustomerMapping()
                                {
                                    AsanitoCustomerId = createCustomerResult.Id,
                                    CustomerId = order.Customer.Id,
                                    FirstName = createCustomerResult.Name,
                                    LastName = createCustomerResult.LastName,
                                    GenderId = createCustomerResult.GenderId,
                                    RawResponse = JsonConvert.SerializeObject(createCustomerResult)
                                };
                                var result = _asanitoService.InsertCustomerAsync(asanitoCustomer).Result;
                            }
                        }
                    }
                }
            }
            else if (settings.CustomerCreationTypeId == (int)CustomerCreationType.PlaceOrder)
            {
                var asanitoCustomer = _asanitoService.GetCustomerByCustomerIdAsync(order.Customer.Id).Result;
                if (asanitoCustomer == null)
                {
                    var aCustomer = new CreateCustomerRequestDto()
                    {
                        Name = order.BillingAddress.FirstName,
                        LastName = order.BillingAddress.LastName,
                        OwnerUserId = settings.OwnerUserId
                    };
                    aCustomer.Mobiles.Add(order.Customer.Username);

                    var province = order.BillingAddress.StateProvince.Name.Trim();
                    var asanitoProvince = new GetProvinceAndCityRequestDto
                    {
                        Value = province
                    };

                    var getProvinceResult = _asanitoClient.GetProvince(asanitoProvince).Result;
                    if (getProvinceResult == null)
                    {
                        _logger.Error($"Get province in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        var asanitoProvinceId = getProvinceResult[0].Id;
                        var city = order.BillingAddress.City.Trim();
                        var asanitoCity = new GetProvinceAndCityRequestDto
                        {
                            Value = city
                        };

                        var getCityResult = _asanitoClient.GetCity(asanitoCity, asanitoProvinceId).Result;
                        if (getCityResult == null)
                        {
                            _logger.Error($"Get city in Asanito failed. OrderId = {order.Id}");
                        }
                        else
                        {
                            var asanitoCustomerAddress = new CustomerAddressDto();

                            try
                            {
                                asanitoCustomerAddress.CityId = getCityResult[0].Id;
                                asanitoCustomerAddress.Address = order.BillingAddress.Address1;
                            }
                            catch (Exception)
                            {
                                asanitoCustomerAddress.CityId = 82;
                                asanitoCustomerAddress.Address = $"{order.BillingAddress.StateProvince.Name.Trim()} {order.BillingAddress.City.Trim()} {order.BillingAddress.Address1}";
                            }

                            aCustomer.Addresses.Add(asanitoCustomerAddress);
                            var createCustomerResult = _asanitoClient.CreateCustomer(aCustomer).Result;
                            if (createCustomerResult == null)
                            {
                                _logger.Error($"Create customer in Asanito failed. CustomerId = {order.Customer.Id}");
                            }
                            else
                            {
                                asanitoCustomer = new AsanitoCustomerMapping()
                                {
                                    AsanitoCustomerId = createCustomerResult.Id,
                                    CustomerId = order.Customer.Id,
                                    FirstName = createCustomerResult.Name,
                                    LastName = createCustomerResult.LastName,
                                    GenderId = createCustomerResult.GenderId,
                                    RawResponse = JsonConvert.SerializeObject(createCustomerResult)
                                };
                                var result = _asanitoService.InsertCustomerAsync(asanitoCustomer).Result;
                            }
                        }
                    }
                }
                else
                {
                    var aCustomer = new UpdateCustomerRequestDto()
                    {
                        Id = asanitoCustomer.AsanitoCustomerId,
                        Name = order.BillingAddress.FirstName,
                        LastName = order.BillingAddress.LastName,
                        OwnerUserId = settings.OwnerUserId
                    };
                    var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                    if (updateCustomerResult == null)
                    {
                        _logger.Error($"Update customer in Asanito failed. CustomerId = {order.Customer.Id}");
                    }
                    else
                    {
                        asanitoCustomer.FirstName = aCustomer.Name;
                        asanitoCustomer.LastName = aCustomer.LastName;
                        asanitoCustomer.GenderId = aCustomer.GenderId;
                        asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                        var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                    }

                    var asanitoCustomerAddress = new CreateCustomerAddressRequestDto
                    {
                        ContactId = asanitoCustomer.AsanitoCustomerId,
                        Address = order.BillingAddress.Address1
                    };

                    var province = order.BillingAddress.StateProvince.Name.Trim();
                    var asanitoProvince = new GetProvinceAndCityRequestDto
                    {
                        Value = province
                    };

                    var getProvinceResult = _asanitoClient.GetProvince(asanitoProvince).Result;
                    if (getProvinceResult == null)
                    {
                        _logger.Error($"Get province in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        var asanitoProvinceId = getProvinceResult[0].Id;
                        var city = order.BillingAddress.City.Trim();
                        var asanitoCity = new GetProvinceAndCityRequestDto
                        {
                            Value = city
                        };

                        var getCityResult = _asanitoClient.GetCity(asanitoCity, asanitoProvinceId).Result;
                        if (getCityResult == null)
                        {
                            _logger.Error($"Get city in Asanito failed. OrderId = {order.Id}");
                        }
                        else
                        {
                            try
                            {
                                asanitoCustomerAddress.CityId = getCityResult[0].Id;
                                var createCustomerAddressResult = _asanitoClient.CreateCustomerAddress(asanitoCustomerAddress).Result;
                                if (createCustomerAddressResult == null)
                                {
                                    _logger.Error($"Create customer address in Asanito failed. OrderId = {order.Id}");
                                }
                            }
                            catch (Exception)
                            {
                                asanitoCustomerAddress.CityId = 82;
                                asanitoCustomerAddress.Address = $"{order.BillingAddress.StateProvince.Name.Trim()} {order.BillingAddress.City.Trim()} {order.BillingAddress.Address1}";
                                var createCustomerAddressResult = _asanitoClient.CreateCustomerAddress(asanitoCustomerAddress).Result;
                                if (createCustomerAddressResult == null)
                                {
                                    _logger.Error($"Create customer address in Asanito failed. OrderId = {order.Id}");
                                }
                            }
                        }
                    }
                }
            }

            if (settings.InvoiceCreationTypeId == (int)InvoiceCreationType.OrderPlaced)
            {
                var asanitoInvoice = _asanitoService.GetInvoiceByOrderIdAsync(order.Id).Result;
                if (asanitoInvoice == null)
                {
                    var currentDateTime = DateTime.Now;
                    var persianDate = currentDateTime.ToPersianYearMonthDay();
                    string formattedDate = $"{persianDate.Month:00}_{persianDate.Day:00}_{persianDate.Year} {currentDateTime.Hour:00}:{currentDateTime.Minute:00}";
                    var customer = _asanitoService.GetCustomerByCustomerIdAsync(order.Customer.Id).Result;
                    var invoice = new CreateInvoiceRequestDto()
                    {
                        Title = $"فاکتور شماره {order.Id}",
                        OrganizationId = settings.OrganizationId,
                        Date = formattedDate,
                        PersonId = customer.AsanitoCustomerId,
                        BankAccountId = settings.BankAccountId,
                        OwnerUserId = settings.OwnerUserId
                    };
                    foreach (var item in order.OrderItems)
                    {
                        var discountPercent = 0;
                        var discountAmount = 0;
                        if (item.DiscountAmountExclTax > 0)
                        {
                            discountPercent = Convert.ToInt32(item.DiscountAmountExclTax * 100 / item.UnitPriceExclTax);
                            discountAmount = Convert.ToInt32(item.DiscountAmountExclTax);
                        }
                        invoice.Items.Add(new InvoiceItemDto()
                        {
                            ProductId = _asanitoService.GetAsanitoProductByProductIdAsync(item.ProductId).Result.AsanitoProductId,
                            Title = item.Product.Name,
                            Count = item.Quantity,
                            UnitPrice = Convert.ToInt32(item.UnitPriceExclTax * 10),
                            DiscountType = item.DiscountAmountExclTax > 0,
                            DiscountPercent = discountPercent,
                            DiscountAmount = discountAmount * 10,
                            TotalDiscountAmount = Convert.ToInt32(item.DiscountAmountExclTax * 10),
                            ProductUnitId = settings.ProductUnitId,
                            HostWarehouseId = settings.WarehouseId,
                            ProductType = 1
                        });
                    }
                    if (order.OrderShippingExclTax > 0)
                    {
                        invoice.AdditionDeductions.Add(new InvoiceAdditionDeductionDto
                        {
                            Title = "هزینه ارسال",
                            Type = 1,
                            CalcType = false,
                            Amount = Convert.ToInt32(order.OrderShippingExclTax * 10)
                        });
                    }

                    var createInvoiceResult = _asanitoClient.CreateInvoice(invoice).Result;
                    if (createInvoiceResult == null)
                    {
                        _logger.Error($"Create invoice in Asanito failed. OrderId = {order.Id}");
                    }
                    else
                    {
                        asanitoInvoice = new AsanitoInvoiceMapping()
                        {
                            AsanitoInvoiceId = createInvoiceResult.Id,
                            OrderId = order.Id,
                            RawResponse = JsonConvert.SerializeObject(createInvoiceResult)
                        };
                        var result = _asanitoService.InsertInvoiceAsync(asanitoInvoice).Result;
                    }
                }
            }
        }

        public void HandleEvent(OrderCancelledEvent eventMessage)
        {
            var order = eventMessage.Order;
            var asanitoInvoice = _asanitoService.GetInvoiceByOrderIdAsync(order.Id).Result;
            if (asanitoInvoice != null)
            {
                var invoiceStatus = new UpdateInvoiceStatusRequestDto()
                {
                    Status = 6
                };
                invoiceStatus.InvoiceIds.Add(asanitoInvoice.AsanitoInvoiceId);

                var updateInvoiceStatusResult = _asanitoClient.UpdateInvoiceStatus(invoiceStatus).Result;
                if (!updateInvoiceStatusResult)
                {
                    _logger.Error($"Update invoice status in Asanito failed. OrderId = {order.Id}");
                }
            }
        }

        public void HandleEvent(EntityUpdatedEvent<Order> eventMessage)
        {
            var order = eventMessage.Entity;
            var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
            var asanitoInvoice = _asanitoService.GetInvoiceByOrderIdAsync(order.Id).Result;
            if (asanitoInvoice != null)
            {
                var updateInvoice = new UpdateInvoiceRequestDto()
                {

                };

                var updateInvoiceResult = _asanitoClient.UpdateInvoice(updateInvoice).Result;
                if (updateInvoiceResult == null)
                {
                    _logger.Error($"Update invoice in Asanito failed. OrderId = {order.Id}");
                }
            }
        }

        public void HandleEvent(EntityDeletedEvent<Customer> eventMessage)
        {
            var customer = eventMessage.Entity;
            var asanitoCustomer = _asanitoService.GetCustomerByCustomerIdAsync(customer.Id).Result;
            if (asanitoCustomer != null)
            {
                var deleteCustomerResult = _asanitoClient.DeleteCustomer(asanitoCustomer.AsanitoCustomerId).Result;
                if (!deleteCustomerResult)
                {
                    _logger.Error($"Delete customer in Asanito failed. CustomerId = {customer.Id}");
                }
                else
                {
                    var result = _asanitoService.DeleteCustomerAsync(asanitoCustomer).Result;
                }
            }
        }

        public void HandleEvent(EntityInsertedEvent<GenericAttribute> eventMessage)
        {
            var genericAttribute = eventMessage.Entity;
            if (genericAttribute.KeyGroup == "Customer")
            {
                var customer = _customerService.GetCustomerById(genericAttribute.EntityId);
                var asanitoCustomer = _asanitoService.GetCustomerByCustomerIdAsync(customer.Id).Result;
                var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
                if (asanitoCustomer != null)
                {
                    var aCustomer = new UpdateCustomerRequestDto()
                    {
                        Id = asanitoCustomer.AsanitoCustomerId,
                        Name = asanitoCustomer.FirstName,
                        LastName = asanitoCustomer.LastName,
                        OwnerUserId = settings.OwnerUserId
                    };

                    if (genericAttribute.Key == NopCustomerDefaults.FirstNameAttribute)
                    {
                        var firstName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.FirstNameAttribute);
                        if (!string.IsNullOrEmpty(firstName))
                        {
                            aCustomer.Name = firstName;
                        }

                        var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                        if (updateCustomerResult == null)
                        {
                            _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomer.FirstName = aCustomer.Name;
                            asanitoCustomer.LastName = aCustomer.LastName;
                            asanitoCustomer.GenderId = aCustomer.GenderId;
                            asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                            var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                        }
                    }
                    else if (genericAttribute.Key == NopCustomerDefaults.LastNameAttribute)
                    {
                        var lastName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.LastNameAttribute);
                        if (!string.IsNullOrEmpty(lastName))
                        {
                            aCustomer.LastName = lastName;
                        }

                        var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                        if (updateCustomerResult == null)
                        {
                            _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomer.FirstName = aCustomer.Name;
                            asanitoCustomer.LastName = aCustomer.LastName;
                            asanitoCustomer.GenderId = aCustomer.GenderId;
                            asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                            var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                        }
                    }
                    else if (genericAttribute.Key == NopCustomerDefaults.GenderAttribute)
                    {
                        var gender = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.GenderAttribute);
                        if (!string.IsNullOrEmpty(gender))
                        {
                            switch (gender)
                            {
                                case "M":
                                    aCustomer.GenderId = 1;
                                    break;
                                case "F":
                                    aCustomer.GenderId = 2;
                                    break;
                            }
                        }

                        var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                        if (updateCustomerResult == null)
                        {
                            _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomer.FirstName = aCustomer.Name;
                            asanitoCustomer.LastName = aCustomer.LastName;
                            asanitoCustomer.GenderId = aCustomer.GenderId;
                            asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                            var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                        }
                    }
                }
            }
        }

        public void HandleEvent(EntityUpdatedEvent<GenericAttribute> eventMessage)
        {
            var genericAttribute = eventMessage.Entity;
            if (genericAttribute.KeyGroup == "Customer")
            {
                var customer = _customerService.GetCustomerById(genericAttribute.EntityId);
                var asanitoCustomer = _asanitoService.GetCustomerByCustomerIdAsync(customer.Id).Result;
                var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
                if (asanitoCustomer != null)
                {
                    var aCustomer = new UpdateCustomerRequestDto()
                    {
                        Id = asanitoCustomer.AsanitoCustomerId,
                        Name = asanitoCustomer.FirstName,
                        LastName = asanitoCustomer.LastName,
                        OwnerUserId = settings.OwnerUserId
                    };

                    if (genericAttribute.Key == NopCustomerDefaults.FirstNameAttribute)
                    {
                        var firstName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.FirstNameAttribute);
                        if (!string.IsNullOrEmpty(firstName))
                        {
                            aCustomer.Name = firstName;
                        }

                        var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                        if (updateCustomerResult == null)
                        {
                            _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomer.FirstName = aCustomer.Name;
                            asanitoCustomer.LastName = aCustomer.LastName;
                            asanitoCustomer.GenderId = aCustomer.GenderId;
                            asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                            var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                        }
                    }
                    else if (genericAttribute.Key == NopCustomerDefaults.LastNameAttribute)
                    {
                        var lastName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.LastNameAttribute);
                        if (!string.IsNullOrEmpty(lastName))
                        {
                            aCustomer.LastName = lastName;
                        }

                        var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                        if (updateCustomerResult == null)
                        {
                            _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomer.FirstName = aCustomer.Name;
                            asanitoCustomer.LastName = aCustomer.LastName;
                            asanitoCustomer.GenderId = aCustomer.GenderId;
                            asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                            var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                        }
                    }
                    else if (genericAttribute.Key == NopCustomerDefaults.GenderAttribute)
                    {
                        var gender = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.GenderAttribute);
                        if (!string.IsNullOrEmpty(gender))
                        {
                            switch (gender)
                            {
                                case "M":
                                    aCustomer.GenderId = 1;
                                    break;
                                case "F":
                                    aCustomer.GenderId = 2;
                                    break;
                            }
                        }

                        var updateCustomerResult = _asanitoClient.UpdateCustomer(aCustomer).Result;
                        if (updateCustomerResult == null)
                        {
                            _logger.Error($"Update customer in Asanito failed. CustomerId = {customer.Id}");
                        }
                        else
                        {
                            asanitoCustomer.FirstName = aCustomer.Name;
                            asanitoCustomer.LastName = aCustomer.LastName;
                            asanitoCustomer.GenderId = aCustomer.GenderId;
                            asanitoCustomer.RawResponse = JsonConvert.SerializeObject(updateCustomerResult);
                            var result = _asanitoService.UpdateCustomerAsync(asanitoCustomer).Result;
                        }
                    }
                }
            }
        }

        public void HandleEvent(EntityInsertedEvent<ProductAttributeCombination> eventMessage)
        {
            var productAttributeCombination = eventMessage.Entity;
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
                    var result = _asanitoService.InsertProductAsync(asanitoProduct).Result;
                }
            }
        }

        public void HandleEvent(EntityUpdatedEvent<ProductAttributeCombination> eventMessage)
        {
            var productAttributeCombination = eventMessage.Entity;
            var product = productAttributeCombination.Product;
            var asanitoProduct = _asanitoService.GetProductByProductAttributeCombinationIdAsync(productAttributeCombination.Id).Result;
            if (asanitoProduct != null)
            {
                var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
                var aWarehouse = new List<UpdateWarehouseRequestDto>();
                aWarehouse.Add(new UpdateWarehouseRequestDto()
                {
                    Id = _asanitoClient.GetWarehouseStock(asanitoProduct.AsanitoProductId).Result.Id,
                    WarehouseId = settings.WarehouseId,
                    Inventory = productAttributeCombination.StockQuantity
                });

                var updateWarehouseResult = _asanitoClient.UpdateWarehouse(aWarehouse, asanitoProduct.AsanitoProductId).Result;
                if (!updateWarehouseResult)
                {
                    _logger.Error($"Update product stock quantity in Asanito failed. ProductCombinationId = {productAttributeCombination.Id}");
                }
                else
                {
                    asanitoProduct.StockQuantity = aWarehouse[0].Inventory;
                    var result = _asanitoService.UpdateProductAsync(asanitoProduct).Result;
                }
            }
        }

        public void HandleEvent(EntityUpdatedEvent<ProductAttributeValue> eventMessage)
        {
            var productAttributeValue = eventMessage.Entity;
            var productAttributeCombinations = _asanitoService.GetProductAttributeCombinationsByProductAttributeValueAsync(productAttributeValue).Result;
            foreach (var productAttributeCombination in productAttributeCombinations)
            {
                var product = productAttributeCombination.Product;
                var asanitoProduct = _asanitoService.GetProductByProductAttributeCombinationIdAsync(productAttributeCombination.Id).Result;
                if (asanitoProduct != null)
                {
                    var settings = _asanitoModelFactory.PrepareAsanitoSettingsModel();
                    var productAttributeValues = _productAttributeParser.ParseProductAttributeValues(productAttributeCombination.AttributesXml);
                    var title = $"{product.Name}";
                    decimal sellPriceDifference = 0;
                    foreach (var attributeValue in productAttributeValues)
                    {
                        title += $" - {attributeValue.Name}";
                        sellPriceDifference += attributeValue.PriceAdjustment;
                    }
                    var category = _asanitoService.GetAsanitoCategoryByCategoryIdAsync(product.MainCategoryId).Result;
                    var aProduct = new UpdateProductRequestDto()
                    {
                        Id = asanitoProduct.AsanitoProductId,
                        Title = title,
                        CategoryId = category.AsanitoCategoryId,
                        Type = 1,
                        InitialBuyPrice = Convert.ToInt32(product.ProductCost * 10),
                        UnitId = settings.ProductUnitId,
                        SellPrice = Convert.ToInt32((product.Price + sellPriceDifference) * 10)
                    };

                    var updateProductResult = _asanitoClient.UpdateProduct(aProduct).Result;
                    if (updateProductResult == null)
                    {
                        _logger.Error($"Update product variant in Asanito failed. ProductCombinationId = {productAttributeCombination.Id}");
                    }
                    else
                    {
                        asanitoProduct.AsanitoCateogryId = updateProductResult.Category.Id;
                        asanitoProduct.CategoryId = product.MainCategoryId;
                        asanitoProduct.Name = updateProductResult.Title;
                        asanitoProduct.InitialBuyPrice = updateProductResult.InitialBuyPrice;
                        asanitoProduct.SellPrice = updateProductResult.SellPrice;
                        asanitoProduct.AsanitoProductTypeId = updateProductResult.ProductType.Id;
                        asanitoProduct.AsanitoUnitId = updateProductResult.UnitId;
                        asanitoProduct.RawResponse = JsonConvert.SerializeObject(updateProductResult);
                        var result = _asanitoService.UpdateProductAsync(asanitoProduct).Result;
                    }
                }
            }
        }

        public void HandleEvent(EntityDeletedEvent<ProductAttributeCombination> eventMessage)
        {
            var productAttributeCombination = eventMessage.Entity;
            var asanitoProduct = _asanitoService.GetProductByProductAttributeCombinationIdAsync(productAttributeCombination.Id).Result;
            if (asanitoProduct != null)
            {
                var deleteProductResult = _asanitoClient.DeleteProduct(asanitoProduct.AsanitoProductId).Result;
                if (!deleteProductResult)
                {
                    _logger.Error($"Delete product variant in Asanito failed. ProductAttributeCombinationId = {productAttributeCombination.Id}");
                }
                else
                {
                    var result = _asanitoService.DeleteProductAsync(asanitoProduct).Result;
                }
            }
        }

        #endregion
    }
}