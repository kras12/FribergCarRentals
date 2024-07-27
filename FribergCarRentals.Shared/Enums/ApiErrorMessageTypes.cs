namespace FribergCarRentals.Shared
{
    /// <summary>
    /// Supported API error message types.
    /// </summary>
    public enum ApiErrorMessageTypes
    {
        AuthorizationError,
        EmailConfirmationFailed,
        GeneralError,
        IdentityError,
        IncompleteInputData,
        InputDataConflict,
        ResourceNotFound,
        ResourceOwnershipConflict,
        UserCreationFailed,
        UserExist,
        UserLoginFailed,
        UserNotFound,
        ValidationError
    }
}
