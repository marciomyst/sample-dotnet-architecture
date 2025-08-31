namespace TheMovie.Application;

/// <summary>
/// Marker type used to reference the <c>TheMovie.Application</c> assembly at runtime.
/// </summary>
/// <remarks>
/// <para>
/// This empty type allows consumers to obtain a reference to the application assembly using
/// <c>typeof(AssemblyReference).Assembly</c> without relying on hard-coded strings. It is
/// commonly used for assembly scanning in dependency injection, MediatR handler registration,
/// validation discovery, mapping profiles, and similar reflection-based features.
/// </para>
/// <para>
/// Example usage:
/// <code>
/// // e.g., when registering MediatR handlers or validators from this assembly
/// services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
/// </code>
/// </para>
/// </remarks>
public class AssemblyReference
{
}
