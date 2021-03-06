# Chosing a Methodology

Once your endpoints are configured, you need to chose a methodology to provide Firestorm with the description of your API.

## Stems

[Stems](../stems/stems-intro.md) were the original idea; a class that defines how your API consumers can interact with your objects. You derive from the `Stem` class, write your properties and methods as usual, then decorate them with `Attributes` to tell the Firestorm Engine how to use your class.

The pattern is based on the `Controller` class in ASP.<span/>NET MVC and the `Hub` class in SignalR. As with those, Stems are fully-featured with dependency injection and contain protected properties such as `IPrincipal User`.

## Fluent

Another pattern is available for simpler requirements. The [Fluent API](../fluent/fluent-intro.md) allows you to configure your whole API using a single `RestContext` class and a builder pattern.

This pattern is based on the Entity Framework Core Fluent API. In fact you can use it alongside, so your entire API, front to back end, is simply a set of POCO classes and two contexts: the `DbContext` from Entity Framework and the `ApiContext` from Firestorm.

This is not as feature-rich though. Everything is configured on startup in a static context, there is no dependency injection support and some advanced features are not included.