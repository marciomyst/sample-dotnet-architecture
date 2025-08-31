namespace TheMovie.Application.Resources;

/// <summary>
/// This is a marker class whose only purpose is to group validation resources
/// for the IStringLocalizer. It allows the dependency injection system
/// to correctly inject IStringLocalizer<Validations> which will then
/// look for a RESX file named "Validations.resx" in this location.
/// </summary>
public class Validations
{
    // This class is intentionally left empty.
}
