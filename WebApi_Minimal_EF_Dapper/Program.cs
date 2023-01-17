using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json;
using WebApi_Minimal_EF_Dapper.Business;
using WebApi_Minimal_EF_Dapper.Domain.Database;
using WebApi_Minimal_EF_Dapper.Endpoints.Categories;
using WebApi_Minimal_EF_Dapper.Endpoints.Orders;
using WebApi_Minimal_EF_Dapper.Endpoints.Products;

var builder = WebApplication.CreateBuilder(args);

//==================================================================================================
//Serviços
//==================================================================================================

//Adicionando o serviço do Swagger
builder.Services.AddSwaggerGen();

//--------------------------------------------------------------------------------------------------
//Log de aplicaçõa com SeriLog
//--------------------------------------------------------------------------------------------------

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.MSSqlServer(
            context.Configuration["Database:SQlServer"],
            sinkOptions: new MSSqlServerSinkOptions()
            {
                AutoCreateSqlTable = true,
                TableName = "ApplicationLog"
            });
});

//--------------------------------------------------------------------------------------------------
//DBContext
//--------------------------------------------------------------------------------------------------
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SQlServer"]);
builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();

//--------------------------------------------------------------------------------------------------
//Meus servicos personalizados da aplicacao
//--------------------------------------------------------------------------------------------------
builder.Services.AddScoped<ServiceAllProductsSold>();

//==================================================================================================
//Application
//==================================================================================================

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//==================================================================================================
// END POINTS
//==================================================================================================

// Product Endpoints -------------------------------------------------------------------------------

app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handle);

app.MapMethods(ProductGet.Template, ProductGet.Methods, ProductGet.Handle);

app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handle);

app.MapMethods(ProductPut.Template, ProductPut.Methods, ProductPut.Handle);

app.MapMethods(ProductDelete.Template, ProductDelete.Methods, ProductDelete.Handle);

// Categorie Endpoints-------------------------------------------------------------------------------

app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);

app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);

app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handle);

// Order Endpoints------------------------------------------------------------------------------------

app.MapMethods(OrderPost.Template, OrderPost.Methods, OrderPost.Handle);

app.MapMethods(OrderGet.Template, OrderGet.Methods, OrderGet.Handle);

//-----------------------------------------------------------------
//filtro de erros
//-----------------------------------------------------------------

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) =>
{
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

    if (error != null)
    {
        if (error is SqlException)
            return Results.Problem(title: "DataBase Out!!!", statusCode: 500);
        else if (error is FormatException)
            return Results.Problem(title: "Error to convert data to other type format", statusCode: 500);
        else if (error is JsonException)
            return Results.Problem(title: "Format error on current Json", statusCode: 500);
    }

    return Results.Problem(title: "An error ocurred", statusCode: 500);
});


app.Run();