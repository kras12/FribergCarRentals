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
        InputDataConflict,
        InvalidInputData,
        ResourceNotFound,
        ResourceNotModified,
        ResourceOwnershipConflict,
        UserCreationFailed,
        UserExist,
        UserLoginFailed,
        UserNotFound,
        ValidationError
    }
}
