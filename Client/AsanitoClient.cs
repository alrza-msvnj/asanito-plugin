using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Tesla.Plugin.Widgets.CRM.Asanito.Models;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO;
using Newtonsoft.Json.Linq;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Warehouse;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Client
{
    public class AsanitoClient : IAsanitoClient
    {
        #region Fields

        private const string baseUrl = "https://panelbak.asanito.com";
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        #region Ctor

        public AsanitoClient(IStoreContext storeContext, ISettingService settingService, IHttpClientFactory httpClientFactory)
        {
            _storeContext = storeContext;
            _settingService = settingService;
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Methods

        public async Task<CreateCustomerResponseDto> CreateCustomer(CreateCustomerRequestDto customer)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(customer);
            var createCustomerRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createCustomerResponse = await client.PostAsync("/api/asanito/person/AddLean", createCustomerRequest);
            if (createCustomerResponse.IsSuccessStatusCode)
            {
                var responseContent = await createCustomerResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateCustomerResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto customer)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(customer);
            var updateCustomerRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var updateCustomerResponse = await client.PutAsync("/api/asanito/Person/editLean", updateCustomerRequest);
            if (updateCustomerResponse.IsSuccessStatusCode)
            {
                var responseContent = await updateCustomerResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<UpdateCustomerResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<GetCustomersResponseDto> GetCustomers(GetCustomersRequestDto body)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(body);
            var getCustomersRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var getCustomersResponse = await client.PostAsync("/api/asanito/person/advancedsearch", getCustomersRequest);
            if (getCustomersResponse.IsSuccessStatusCode)
            {
                var responseContent = await getCustomersResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GetCustomersResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var deleteCustomerResponse = await client.DeleteAsync($"/api/asanito/Person/delete?ID={id}");
            if (deleteCustomerResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CreateCustomerAddressResponseDto> CreateCustomerAddress(CreateCustomerAddressRequestDto address)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(address);
            var createCustomerAddressRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createCustomerAddressResponse = await client.PostAsync("/api/asanito/PersonAddress", createCustomerAddressRequest);
            if (createCustomerAddressResponse.IsSuccessStatusCode)
            {
                var responseContent = await createCustomerAddressResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateCustomerAddressResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<GetProvinceResponseDto>> GetProvince(GetProvinceAndCityRequestDto body)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(body);
            var getProvinceRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var getProvinceResponse = await client.PostAsync("/api/asanito/Province/getDropListWith", getProvinceRequest);
            if (getProvinceResponse.IsSuccessStatusCode)
            {
                var responseContent = await getProvinceResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<List<GetProvinceResponseDto>>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<GetCityResponseDto>> GetCity(GetProvinceAndCityRequestDto body, int id)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(body);
            var getCityRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var getCityResponse = await client.PostAsync($"/api/asanito/City/getDropListWith?provinceID={id}", getCityRequest);
            if (getCityResponse.IsSuccessStatusCode)
            {
                var responseContent = await getCityResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<List<GetCityResponseDto>>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<CreateCategoryResponseDto> CreateCategory(CreateCategoryRequestDto category)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(category);
            var createCategoryRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createCategoryResponse = await client.PostAsync("/api/asanito/ProductCategory/addNew", createCategoryRequest);
            if (createCategoryResponse.IsSuccessStatusCode)
            {
                var responseContent = await createCategoryResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateCategoryResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<UpdateCategoryResponseDto> UpdateCategory(UpdateCategoryRequestDto category)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(category);
            var updateCategoryRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var updateCategoryResponse = await client.PutAsync("/api/asanito/ProductCategory/edit", updateCategoryRequest);
            if (updateCategoryResponse.IsSuccessStatusCode)
            {
                var responseContent = await updateCategoryResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<UpdateCategoryResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var deleteCategoryResponse = await client.DeleteAsync($"/api/asanito/ProductCategory/delete?ID={id}");
            if (deleteCategoryResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GetProductResponseDto> GetProducts(GetProductRequestDto body)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(body);
            var getProductsRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var getProductsResponse = await client.PostAsync("/api/asanito/Product/getList", getProductsRequest);
            if (getProductsResponse.IsSuccessStatusCode)
            {
                var responseContent = await getProductsResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GetProductResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto product)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(product);
            var createProductRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createProductResponse = await client.PostAsync("/api/asanito/Product/addNew", createProductRequest);
            if (createProductResponse.IsSuccessStatusCode)
            {
                var responseContent = await createProductResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateProductResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<UpdateProductResponseDto> UpdateProduct(UpdateProductRequestDto product)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(product);
            var updateProductRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var updateProductResponse = await client.PutAsync("/api/asanito/Product/edit", updateProductRequest);
            if (updateProductResponse.IsSuccessStatusCode)
            {
                var responseContent = await updateProductResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<UpdateProductResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var deleteProductResponse = await client.DeleteAsync($"/api/asanito/Product/delete?ID={id}");
            if (deleteProductResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GetWarehouseResponseDto> GetWarehouse()
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var getWarehouseResponse = await client.GetAsync("/api/asanito/Warehouse/getList");
            if (getWarehouseResponse.IsSuccessStatusCode)
            {
                var responseContent = await getWarehouseResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<List<GetWarehouseResponseDto>>(responseContent);
                return response[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<GetWarehouseStockResponseDto> GetWarehouseStock(int id)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var getWarehouseStockResponse = await client.GetAsync($"/api/asanito/Product/getWarehouseStockList?productID={id}");
            if (getWarehouseStockResponse.IsSuccessStatusCode)
            {
                var responseContent = await getWarehouseStockResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<List<GetWarehouseStockResponseDto>>(responseContent);
                return response[0];
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateWarehouse(List<UpdateWarehouseRequestDto> warehouse, int id)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(warehouse);
            var updateWarehouseRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var updateWarehouseResponse = await client.PutAsync($"/api/asanito/WarehouseStock/updateInitialStocks?productID={id}", updateWarehouseRequest);
            if (updateWarehouseResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CreateInvoiceResponseDto> CreateInvoice(CreateInvoiceRequestDto invoice)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(invoice);
            var createInvoiceRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createInvoiceResponse = await client.PostAsync("/api/asanito/Invoice/issue", createInvoiceRequest);
            if (createInvoiceResponse.IsSuccessStatusCode)
            {
                var responseContent = await createInvoiceResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateInvoiceResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<CreateSettledInvoiceResponseDto> CreateSettledInvoice(CreateSettledInvoiceRequestDto invoice)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(invoice);
            var createSettledInvoiceRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createSettledInvoiceResponse = await client.PutAsync("/api/asanito/Invoice/issueAndPay", createSettledInvoiceRequest);
            if (createSettledInvoiceResponse.IsSuccessStatusCode)
            {
                var responseContent = await createSettledInvoiceResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateSettledInvoiceResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<UpdateInvoiceResponseDto> UpdateInvoice(UpdateInvoiceRequestDto invoice)
        {
            // TODO 
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(invoice);
            var updateInvoiceRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var updateInvoiceResponse = await client.PutAsync($"/api/asanito/Invoice/groupUpdateStatus", updateInvoiceRequest);
            if (updateInvoiceResponse.IsSuccessStatusCode)
            {
                var responseContent = await updateInvoiceResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<UpdateInvoiceResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateInvoiceStatus(UpdateInvoiceStatusRequestDto invoiceStatus)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(invoiceStatus);
            var updateInvoiceStatusRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var updateInvoiceStatusResponse = await client.PutAsync("/api/asanito/Invoice/groupUpdateStatus", updateInvoiceStatusRequest);
            if (updateInvoiceStatusResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CreateOperationIncomeResponseDto> CreateOperationIncome(CreateOperationIncomeRequestDto income)
        {
            var token = await LoginAndGetToken();
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(income);
            var createOperationIncomeRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var createOperationIncomeResponse = await client.PostAsync("/api/asanito/OperatingIncome/addNew", createOperationIncomeRequest);
            if (createOperationIncomeResponse.IsSuccessStatusCode)
            {
                var responseContent = await createOperationIncomeResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CreateOperationIncomeResponseDto>(responseContent);
                return response;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Utilities

        private async Task<string> LoginAndGetToken()
        {
            var storeId = _storeContext.CurrentStore.Id;
            var settings = _settingService.LoadSetting<AsanitoSettings>(storeId);
            var model = settings.ToSettingsModel<AsanitoSettingsModel>();

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            var loginRequestModel = new LoginRequestDto()
            {
                Username = model.Username,
                Password = model.Password,
            };
            var json = JsonConvert.SerializeObject(loginRequestModel);
            var loginRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var loginResponse = await client.PostAsync("/api/auth/Account/Loginbyidentity", loginRequest);
            if (loginResponse.IsSuccessStatusCode)
            {
                var responseContent = await loginResponse.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(responseContent);
                var token = jsonObject["access_token"].ToString();
                return token;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}