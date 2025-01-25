using FluentValidation;
using FluentValidation.Results;
using ProductsService.BusinessLogicLayer.DTOs;
using ProductsService.BusinessLogicLayer.ServiceContracts;

namespace ProductsService.API.EndPoints
{
    public static class ProductsEndpoints
    {
        public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/products", async (IProductsService productsService) =>
            {
                var products = await productsService.GetAll();
                return Results.Ok(products);
            });

            app.MapGet("/api/products/search/{searchString}", async (IProductsService productsService, string searchString) =>
            {
                var products = await productsService.GetAllByCondition(m => m.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                    || (!string.IsNullOrWhiteSpace(m.Category) && m.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase)));

                return Results.Ok(products);
            });

            app.MapGet("/api/products/search/product-id/{productId:guid}", async (IProductsService productsService, Guid productId) =>
            {
                var product = await productsService.GetByCondition(m => m.Id == productId);
                if(product is null)
                    return Results.NotFound();

                return Results.Ok(product);
            });

            app.MapPost("/api/products", async (IProductsService productsService, IValidator<AddProductDTO> validator, AddProductDTO addProductDTO) =>
            {
                // Validate
                ValidationResult result = await validator.ValidateAsync(addProductDTO);
                if (!result.IsValid)
                {
                    Dictionary<string, string[]> errors = result.Errors
                        .GroupBy(m => m.PropertyName)
                        .ToDictionary(grp => grp.Key, grp => grp.Select(m => m.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var response = await productsService.Add(addProductDTO);
                return response is not null ? 
                    Results.Created($"/api/products/search/product-id/{response.ProductId}", response) :
                    Results.Problem("Error in adding product");
            });

            app.MapPut("/api/products", async (IProductsService productsService, UpdateProductDTO updateProductDTO, IValidator<UpdateProductDTO> validator) =>
            {
                // Validate
                ValidationResult result = await validator.ValidateAsync(updateProductDTO);
                if (!result.IsValid)
                {
                    Dictionary<string, string[]> errors = result.Errors
                        .GroupBy(m => m.PropertyName)
                        .ToDictionary(grp => grp.Key, grp => grp.Select(m => m.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var response = await productsService.Update(updateProductDTO);
                return response is not null ?
                    Results.Ok(response) :
                    Results.Problem("Error in updating product");
            });

            app.MapDelete("/api/products/{id:guid}", async (IProductsService productsService, Guid id) =>
            {
                var isDeleted = await productsService.Delete(id);
                return isDeleted ? Results.Ok(isDeleted) : Results.Problem("Error in deleting product");
            });

            return app;
        }
    }
}
