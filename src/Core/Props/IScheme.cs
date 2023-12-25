namespace Core.Props;

/// <summary>
/// Interface of all schemes.
/// </summary>
/// <remarks>
/// Schema is the metadata of the base entity of the system.
/// The content of each schema implementation must be in a separate directory in the schema directory.
/// </remarks>
public interface IScheme
{
    /// <summary>
    /// Symbolic schema identifier.
    /// </summary>
    /// <remarks>
    /// The symbolic schema identifier is used to refer to the schema.
    /// For example, in the database or from the content of other schemas.
    /// </remarks>
    string Sid { get; }
}