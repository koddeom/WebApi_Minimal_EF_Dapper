﻿using Microsoft.AspNetCore.Mvc;
using WebApi_Minimal_EF_Dapper.Domain.Database;

namespace WebApi_Minimal_EF_Dapper.Endpoints.Categories
{
    public class CategoryDelete
    {
        public static string Template => "/categorys/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
        public static Delegate Handle => Action;

        //-----------------------------------------------------------------------

        public static IResult Action([FromRoute] Guid id, ApplicationDbContext dbContext)
        {
            //Recupero o produto do banco
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return Results.NotFound();
            }

            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();

            return Results.Ok();
        }
    }
}