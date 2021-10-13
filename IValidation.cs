namespace HappyCRUD;
/// <summary>
/// Interface to apply validation rules to your model
/// </summary>
public interface IValidation
    {
    /// <summary>
    /// Track your item edit etate
    /// </summary>
    bool InEditState { get; set; }
    /// <summary>
    /// Check if item is valid
    /// </summary>
    /// <returns>True if valid and False if not</returns>
    bool IsValid();
    }