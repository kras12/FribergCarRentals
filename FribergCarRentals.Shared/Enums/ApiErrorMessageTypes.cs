namespace FribergCarRentals.Shared
{
    /// <summary>
    /// Supported API error message types.
    /// </summary>
    public enum ApiErrorMessageTypes
    {
        GeneralError,
        AuthorizationError,
        IdentityError,
        IncompleteInputData,
        InputDataConflict,
        ResourceOwnershipConflict,
        ResourceNotFound,
        ValidationError,
    }
}
