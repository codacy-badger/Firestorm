GetAttribute
------------

The `Get` attribute marks a member as a field that an API client can read.

#### Static Expressions

Static properties that return `Expression<Func<Artist, int>>` are the most efficient field getters. Expressions are combined into parts of a query.

Getters can be given a Display.Nested argument to display the field even when nested in a collection.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static Expression<Func<Artist, int>> ID
    {
        get { return a => a.ID; }
    }
    
    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }
    
    [Get]
    public static Expression<Func<Artist, DateTime>> StartDate
    {
        get { return a => a.StartDate; }
    }
}
```

``` json
GET /artists
200 OK
[        
    { "id": 122 },
    { "id": 123 },
    { "id": 124 }
]
```

``` json
GET /artists/123
200 OK
{
    "id": 123,
    "name": "Periphery",
    "start_date": "2005-01-01"
}
```

``` json
GET /artists/123/name
200 OK
"Periphery"
```


#### Sorting and filtering

The Firestorm Engine provides querying functionality.

``` json
GET /artists?fields=id,name
200 OK
[                        
    {
        "id": 122,
        "name": "Noisia"
    },                                        
    {
        "id": 123,
        "name": "Periphery"
    },                                        
    {
        "id": 124,
        "name": "Infected Mushroom"
    }
]
```

``` json
GET /artists?fields=id,name&sort=start_date
200 OK
[
    {
        "id": 124,
        "name": "Infected Mushroom"
    },
    {
        "id": 123,
        "name": "Periphery"
    },
    {
        "id": 122,
        "name": "Noisia"
    },
]
```

``` json
GET /artists?fields=name&where=id>122
200 OK                
[       
    {
        "name": "Periphery"
    },
    {                
        "name": "Infected Mushroom"
    }
]
```

SetAttribute
------------

The `Set` attribute marks a member as a field that an API client can write to.

#### Static Expressions

Simple static properties that return `Expression<Func<Artist, int>>` for a single property can also be used as setters.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static Expression<Func<Artist, int>> ID
    {
        get { return a => a.ID; }
    }
    
    [Get]
    [Set]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }
    
    [Get]
    public static Expression<Func<Artist, DateTime>> StartDate
    {
        get { return a => a.StartDate; }
    }
}
```

``` json
POST /artists
{
    "name": "Fred"
}
201 Created
```

``` json
PUT /artists/123                
{
    "name": "Fred"
}
200 OK
```

``` json
PUT /artists/123/name             
"Fred"
200 OK
```

#### Static Setter Methods

Void methods that take two parameters, one for the item and one for the field value, can also be used as setters for more complex scenarios. The attribute should be placed on a different method to the getter.

``` csharp
public class ArtistsStem : Stem<Artist>
{
    [Get(Display.Nested)]
    public static Expression<Func<Artist, int>> ID
    {
        get { return a => a.ID; }
    }
    
    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }
    
    [Get]
    public static Expression<Func<Artist, DateTime>> StartDate
    {
        get { return a => a.StartDate; }
    }
}
```

``` json
POST /artists
{
    "name": "Fred"
}
201 Created
```

``` json
PUT /artists/123                
{
    "name": "Fred"
}
200 OK
```

#### Instance Setter Methods

Non-static methods of the same signature can be used too. These will use a single Stem instance per API request. This is very handy when combined with [Constructor Inection](http://localhost:63119/Stems/NOWHERE).

``` csharp
public class ArtistsStem : Stem<Artist>
{
    private readonly IArtistsService _service;

    public ArtistsStem(IArtistsService service)
    {
        _service = service;
    }

    [Get]
    public static Expression<Func<Artist, string>> Name
    {
        get { return a => a.Name; }
    }

    [Set]
    public void SetName(Artist artist, string name)
    {
        if(_service.ArtistExistsWithName(name))
            throw new ArgumentException("An artist already exists with this name.");
        artist.OldName = artist.Name;
        artist.Name = name;
        artist.NameChangedDate = DateTime.Now;
    }
}
```

``` json
POST /artists
{
    "name": "The Beatles"
}
400 Bad Request
{
    "error": "argument",
    "message": "An artist already exists with this name."
}
```