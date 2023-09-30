using Microsoft.EntityFrameworkCore;
using StageTest.Models;
namespace ProjectB.Controllers;

public static class ContainersVariablesTypeEndpoints
{
    public static void MapContainersVariablesTypeEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/ContainersVariablesType", async (ApplicationDbContext db) =>
        {
            return await db.ContainersVariablesTypes.ToListAsync();
        })
        .WithName("GetAllContainersVariablesTypes")
        .Produces<List<ContainersVariablesType>>(StatusCodes.Status200OK);

        routes.MapGet("/api/ContainersVariablesType/{id}", async (Guid IdVariableType, ApplicationDbContext db) =>
        {
            return await db.ContainersVariablesTypes.FindAsync(IdVariableType)
                is ContainersVariablesType model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetContainersVariablesTypeById")
        .Produces<ContainersVariablesType>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/ContainersVariablesType/{id}", async (Guid IdVariableType, ContainersVariablesType containersVariablesType, ApplicationDbContext db) =>
        {
            var foundModel = await db.ContainersVariablesTypes.FindAsync(IdVariableType);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(containersVariablesType);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateContainersVariablesType")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/ContainersVariablesType/", async (ContainersVariablesType containersVariablesType, ApplicationDbContext db) =>
        {
            db.ContainersVariablesTypes.Add(containersVariablesType);
            await db.SaveChangesAsync();
            return Results.Created($"/ContainersVariablesTypes/{containersVariablesType.IdVariableType}", containersVariablesType);
        })
        .WithName("CreateContainersVariablesType")
        .Produces<ContainersVariablesType>(StatusCodes.Status201Created);

        routes.MapDelete("/api/ContainersVariablesType/{id}", async (Guid IdVariableType, ApplicationDbContext db) =>
        {
            if (await db.ContainersVariablesTypes.FindAsync(IdVariableType) is ContainersVariablesType containersVariablesType)
            {
                db.ContainersVariablesTypes.Remove(containersVariablesType);
                await db.SaveChangesAsync();
                return Results.Ok(containersVariablesType);
            }

            return Results.NotFound();
        })
        .WithName("DeleteContainersVariablesType")
        .Produces<ContainersVariablesType>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
