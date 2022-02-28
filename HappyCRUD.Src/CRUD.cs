namespace HappyCRUD;
/// <summary>
///  Consider Using static CRUD to call IsInEditState directly
/// </summary>
public static class CRUD
    {
        /// <summary>
       /// Get and Set IsInEditState to track Model State
       /// </summary>
        public static bool IsInEditState { get; set; }
    }