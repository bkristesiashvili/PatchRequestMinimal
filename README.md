
# Patch Request for Minimal API

helps to process patch request from the minimalAPI

## Installation

Install my-project with dotnet-cli

```bash
  dotnet add package PatchRequestMinimal --version 7.0.17
```
    
## Features

- Replace
- Remove


## Usage/Examples

```csharp
using PatchRequest;

//================================================================
// Request DTO

public sealed record NestedModel(Guid Id, string Name, int? Value);

public sealed record RequestModel(string Title, 
    int Number,
    NestedModel NestedObject);

//================================================================
// Destination Entity 

public sealed record NestedEntity(Guid Id, string Name, int? Value);
public sealed record SourceEntity(string Title, 
    int Number,
    NestedEntity NestedObject);

//================================================================
// Patch endpoint into Program.cs

app.MapPatch("patch-test/{id}", (Guid id, 
    [FromBody] PatchRequest<RequestModel> request) =>{
    SourceEntity entity = dbContext.Set<SourceEntity>()
        .First(x => x.Id == id);

    RequestResult result = request.Apply(entity);

    if(result.Succeeded){
        dbContext.Update(entity);
        dbContext.SaveChanges();

        return Results.Ok(new { Message = "Some property values changed." })
    }

    var errors = result.Messages.Select(x => x.Description);

    return Results.BadRequest(errors);
});

//================================================================
// PatchRequest<T> example requests
/*
    {
        "operations":[
            {
                "op": "replace",
                "prop": "title",
                "value": "new value"
            },
            {
                "op": "replace",
                "prop": "nestedObject.name",
                "value": "nested object name new value"
            },
            {
                "op": "remove",
                "prop": "nestedObject.value",
                "value": null
            }
        ]
    }
*/
```

