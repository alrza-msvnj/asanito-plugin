using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tesla.Plugin.Widgets.CRM.Asanito.Domain;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Services
{
    public interface IAsanitoService
    {
        Task<AsanitoCustomerMapping> GetCustomerByCustomerIdAsync(int id);

        Task<AsanitoCustomerMapping> GetCustomerByAsanitoCustomerIdAsync(int id);

        IList<AsanitoCustomerMapping> GetAsanitoCustomersWithWrongGenderId(int limit);

        Task<GeneralResponseModel> InsertCustomerAsync(AsanitoCustomerMapping customer);

        Task<GeneralResponseModel> UpdateCustomerAsync(AsanitoCustomerMapping customer);

        Task<GeneralResponseModel> DeleteCustomerAsync(AsanitoCustomerMapping customer);

        Task<AsanitoCategoryMapping> GetAsanitoCategoryByCategoryIdAsync(int id);

        Task<int> GetCategoryIdByAsanitoCategoryIdAsync(int id);

        Task<GeneralResponseModel> InsertCategoryAsync(AsanitoCategoryMapping category);

        Task<GeneralResponseModel> UpdateCategoryAsync(AsanitoCategoryMapping category);

        Task<GeneralResponseModel> DeleteCategoryAsync(AsanitoCategoryMapping category);

        Task<AsanitoProductMapping> GetAsanitoProductByProductIdAsync(int id);

        Product GetProductByProductNameAsync(string name);

        Task<AsanitoProductMapping> GetProductByProductAttributeCombinationIdAsync(int id);

        Task<List<ProductAttributeCombination>> GetProductAttributeCombinationsByProductAttributeValueAsync(ProductAttributeValue productAttributeValue);

        IList<AsanitoProductMapping> GetAllProductsAsync();

        Task<GeneralResponseModel> InsertProductAsync(AsanitoProductMapping product);

        Task<GeneralResponseModel> UpdateProductAsync(AsanitoProductMapping product);

        Task<GeneralResponseModel> DeleteProductAsync(AsanitoProductMapping product);

        Task<AsanitoInvoiceMapping> GetInvoiceByOrderIdAsync(int id);

        Task<GeneralResponseModel> InsertInvoiceAsync(AsanitoInvoiceMapping invoice);

        IList<Customer> GetCustomersNotInAsanitoCustomerMapping(int limit);

        IList<Category> GetParentCategoriesNotInAsanitoCategoryMapping(int limit);

        IList<Category> GetChildCategoriesNotInAsanitoCategoryMapping(int limit);

        IList<Product> GetProductsNotInAsanitoProductMapping(int limit);

        Task<List<ProductAttributeCombination>> GetProductAttributeCombinationsNotInAsanitoProductAsync(int limit);
    }
}