# API Request Shape Policy

This project uses a consistent pattern for Minimal API request models to avoid binding pitfalls and to keep endpoint delegates concise.

- Body endpoints (POST/PUT/PATCH):
  - Use a request record that wraps the JSON payload in a `Body` property, for example:
    - `CreateMovieRequest(Body: CreateMovieRequestBody)`
    - `UpdateMovieRequest(Id, Body: UpdateMovieRequestBody)`
  - Bind in endpoints with `[AsParameters]` to combine route + body cleanly.
  - Rationale:
    - Minimal APIs support only one body parameter — the Body wrapper prevents multiple-from-body binding issues and keeps delegates tidy.
    - Swagger (ExampleFilters) can target the wrapper type and show the exact JSON shape.
    - Mappers remain simple: `request.Body` provides all fields; route values (like `Id`) live alongside in the same request record.

- No-body endpoints (GET/DELETE):
  - Do not use a Body wrapper. Prefer a thin request record with route and/or query parameters, for example:
    - `GetMovieByIdRequest(Id)`
    - `GetMoviesRequest(Title, ReleaseYear, GenreId, PageNumber, PageSize)`
  - Bind in endpoints with `[AsParameters]` for a concise delegate signature.

## Code Examples

- Create (POST): `src/TheMovie.Api/Features/Movies/CreateMovie/CreateMovieRequest.cs`
- Update (PUT): `src/TheMovie.Api/Features/Movies/UpdateMovie/UpdateMovieRequest.cs`
- Get by id (GET): `src/TheMovie.Api/Features/Movies/GetMovieById/GetMovieByIdRequest.cs`
- Get list (GET): `src/TheMovie.Api/Features/Movies/GetMovies/GetMoviesRequest.cs`
- Delete (DELETE): `src/TheMovie.Api/Features/Movies/DeleteMovie/DeleteMovieRequest.cs`

