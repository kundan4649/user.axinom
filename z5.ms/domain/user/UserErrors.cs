using z5.ms.common.abstractions;

namespace z5.ms.domain.user
{
    //TODO define actual error codes instead of '1'
    //TODO consider unifying some error code objects
    /// <summary>Errors used by User Module</summary>
    public class UserErrors
    {
        /// <summary>Item couldn't be found </summary>
        public Error ItemNotFound { get; } = new Error {Code = 2, Message = "Item couldn't be found"};

        /// <summary>The user could not be found. </summary>
        public Error UserNotFound { get; } = new Error {Code = 2, Message = "The user could not be found."};

        #region Validation

        /// <summary>Invalid email </summary>
        public Error InvalidEmail { get; } = new Error {Code = 3, Message = "Invalid email"};

        /// <summary>Invalid phone number </summary>
        public Error InvalidPhone { get; } = new Error {Code = 3, Message = "Invalid phone number"};
        
        /// <summary>Invalid password </summary>
        public Error InvalidPassword { get; } = new Error {Code = 3, Message = "Invalid password"};
        
        /// <summary>Invalid country </summary>
        public Error InvalidCountry { get; } = new Error {Code = 3, Message = "Invalid country"};

        /// <summary>Page is invalid. Must be 1 or greater</summary>
        public Error InvalidPage { get; } = new Error {Code = 3, Message = "Page size must be equal of greater than 1"};

        /// <summary>How many items should be returned per page</summary>
        public Error InvalidPageSize { get; } = new Error
            {Code = 3, Message = "Page size must be an integer from 1 to 100 inclusively"};

        /// <summary>Asset id is invalid format</summary>
        public Error InvalidAssetId { get; } = new Error {Code = 3, Message = "Please specify a valid asset ID"};
        
        /// <summary>Asset Type doesn't match with asset id</summary>
        public Error InvalidAssetType { get; } = new Error {Code = 3, Message = "Asset ID and asset type do not match"};
        
        #endregion        
        
        #region User

        /// <summary>The old password was not correct. </summary>
        public Error IncorrectOldPassword { get; } = new Error {Code = 2103, Message = "The old password was not correct."};

        /// <summary>Password couldn't be updated (DB update failure) </summary>
        public Error PasswordUpdate { get; } = new Error {Code = 2104, Message = "Password couldn't be updated (DB update failure)"};

        /// <summary>The email address cannot be used. This email address is used already by some other user. </summary>
        public Error EmailAlreadyUsed { get; } = new Error
        {
            Code = 2010,
            Message = "The email address cannot be used. This email address is used already by some other user."
        };

        /// <summary>Registration failed (DB insert failure). Returned when inserting user by email </summary>
        public Error RegisterByEmailFailed { get; } = new Error {Code = 2013, Message = "Registration failed (DB insert failure)"};
        
        /// <summary>Recreate password failed (DB update failure) </summary>
        public Error PasswordRecreate { get; } = new Error {Code = 2018, Message = "Recreate password failed (DB update failure)"};

        /// <summary>The phone number cannot be used. This phone number is used already by some other user. </summary>
        public Error PhoneNumberUsed { get; } = new Error
        {
            Code = 2020,
            Message = "The phone number cannot be used. This phone number is used already by some other user."
        };

        /// <summary>Registration failed (DB insert failure). Returned when creating user using mobile </summary>
        public Error UsingMobileRegisterFailed { get; } = new Error {Code = 2023, Message = "Registration failed (DB insert failure)"};

        /// <summary>Cannot activate user, it has already been activated </summary>
        public Error UserAlreadyActivated { get; } = new Error {Code = 2027, Message = "User is already activated"};
        
        /// <summary>Registration failed (DB update failure). Returned when mobile confirmation key generation fails </summary>
        public Error MobileConfirmationRegisterFailed { get; } = new Error {Code = 2028, Message = "Registration failed (DB update failure)"};

        /// <summary>Password reset failed (DB update failure) </summary>
        public Error PasswordResetFailed { get; } = new Error {Code = 2028, Message = "Password reset failed (DB update failure)"};

        /// <summary>This facebook account is already registered. </summary>
        public Error FacebookUserRegistered { get; } = new Error {Code = 2031, Message = "This facebook account is already registered."};

        /// <summary>Registration failed (DB update failure). Returned when creating user using facebook. </summary>
        public Error FacebookUpdateFailed { get; } = new Error {Code = 2032, Message = "Registration failed (DB update failure)"};

        /// <summary>Registration failed (DB insert failure). Returned when creating user using facebook </summary>
        public Error FacebookInsertFailed { get; } = new Error {Code = 2033, Message = "Registration failed (DB insert failure)"};

        /// <summary>This google account is already registered. </summary>
        public Error GoogleAccountRegistered { get; } = new Error {Code = 2041, Message = "This google account is already registered."};

        /// <summary>Registration failed (DB update failure). Returned when creating user using google </summary>
        public Error GoogleUpdateFailed { get; } = new Error {Code = 2042, Message = "Registration failed (DB update failure)"};

        /// <summary>Registration failed (DB insert failure). Returned when creating user using google </summary>
        public Error GoogleInsertFailed { get; } = new Error {Code = 2032, Message = "Registration failed (DB insert failure)"};
        
        /// <summary>This twitter account is already registered. </summary>
        public Error TwitterAccountRegistered { get; } = new Error {Code = 2051, Message = "This twitter account is already registered."};

        /// <summary>Registration failed (DB update failure). Returned when creating user using twitter </summary>
        public Error TwitterUpdateFailed { get; } = new Error {Code = 2052, Message = "Registration failed (DB update failure)"};

        /// <summary>Registration failed (DB insert failure). Returned when creating user using twitter </summary>
        public Error TwitterInsertFailed { get; } = new Error {Code = 2052, Message = "Registration failed (DB insert failure)"};

        /// <summary>This Amazon account is already registered. </summary>
        public Error AmazonAccountRegistered { get; } = new Error { Code = 2053, Message = "This amazon account is already registered." };

        /// <summary>Registration failed (DB update failure). Returned when creating user using Amazon </summary>
        public Error AmazonUpdateFailed { get; } = new Error { Code = 2054, Message = "Registration failed (DB update failure)" };

        /// <summary>Registration failed (DB insert failure). Returned when creating user using Amazon </summary>
        public Error AmazonInsertFailed { get; } = new Error { Code = 2055, Message = "Registration failed (DB insert failure)" };

        /// <summary>The confirmation code was not found in our system. Used when confirming email</summary>
        public Error EmailConfirmationNotFound { get; } = new Error
        {
            Code = 2060,
            Message = "The confirmation code was not found in our system."
        };

        /// <summary>Confirmation code expired. Returned when confirming email </summary>
        public Error EmailConfirmationExpired { get; } = new Error {Code = 2061, Message = "Confirmation code expired."};

        /// <summary>Confirmation failed (DB update failure). Returned when confirming email </summary>
        public Error EmailConfirmationUpdateFailed { get; } = new Error {Code = 2062, Message = "Confirmation failed (DB update failure)"};
        
        /// <summary>The confirmation code was not found in our system. Used when confirming email change</summary>
        public Error ConfirmationNotFound { get; } = new Error {Code = 2063, Message = "The confirmation code was not found in our system."};

        /// <summary>Confirmation code expired. Used when confirming email change </summary>
        public Error ConfirmationExpired { get; } = new Error {Code = 2064, Message = "Confirmation code expired."};

        /// <summary>Confirmation failed (DB update failure) </summary>
        public Error ConfirmationUpdateFailed { get; } = new Error {Code = 2065, Message = "Confirmation failed (DB update failure)"};
        
        /// <summary>User is not yet activated </summary>
        public Error NotActivated { get; } = new Error {Code = 2066, Message = "User is not yet activated"};
        
        /// <summary>This email address is already in use </summary>
        public Error EmailOrMobileAlreadyUsed { get; } = new Error {Code = 2067, Message = "This email or mobile is already in use"};

        /// <summary>Confirmation code change failed (DB update failure) </summary>
        public Error ConfirmationCodeUpdateFailed { get; } = new Error {Code = 2068, Message = "Confirmation code change failed (DB update failure)"};

        /// <summary>The confirmation code was not found in our system. Returned when confirming mobile user </summary>
        public Error MobileConfirmNotFound { get; } = new Error {Code = 2070, Message = "The confirmation code was not found in our system."};

        /// <summary>Confirmation code expired. Returned when confirming mobile user </summary>
        public Error MobileConfirmExpired { get; } = new Error {Code = 2071, Message = "Confirmation code expired."};

        /// <summary>Confirmation failed (DB update failure). Returned when confirming mobile user </summary>
        public Error MobileConfirmUpdateFailed { get; } = new Error {Code = 2072, Message = "Confirmation failed (DB update failure)"};
        
        /// <summary>Delete user failed (DB update failure) </summary>
        public Error DeleteUserFailed { get; } = new Error {Code = 2111, Message = "Delete user failed (DB update failure)"};

        /// <summary>The email address and password combination was wrong during login. Used in login </summary>
        public Error WrongEmailPassword { get; } = new Error
        {
            Code = 2120,
            Message = "The email address and password combination was wrong during login."
        };

        /// <summary>The email address of the user is not confirmed. </summary>
        public Error EmailUnconfirmed { get; } = new Error {Code = 2121, Message = "The email address of the user is not confirmed."};
        
        /// <summary>User must login with account. Contains third party authentication service i.e. 'facebook' </summary>
        public Error LoginAccount { get; } = new Error {Code = 2122, Message = "This Email ID has been registered with us via Google/Facebook/Twitter. Please log in using the original mode of registration." };

        /// <summary>The email address and password combination was wrong during login. Used in mobile login </summary>
        public Error WrongMobilePassword { get; } = new Error {Code = 2130, Message = "The mobile number and password combination was wrong during login." };

        /// <summary>The phone number of the user is not confirmed. Used in mobile login </summary>
        public Error MobileUnconfirmed { get; } = new Error {Code = 2131, Message = "The phone number of the user is not confirmed."};
        
        /// <summary>User must login with account. Contains third party authentication service i.e. 'facebook' </summary>
        public Error MobileLoginAccount { get; } = new Error {Code = 2132, Message = "This Email ID has been registered with us via Google/Facebook/Twitter. Please log in using the original mode of registration." };
              
        /// <summary>The confirmation code was not found in our system. </summary>
        public Error NoConfirmationCode { get; } = new Error {Code = 2171, Message = "The confirmation code was not found in our system."};

        /// <summary>Confirmation code expired. </summary>
        public Error ExpiredConfirmationCode { get; } = new Error {Code = 2172, Message = "Confirmation code expired."};

        /// <summary>Update user failed (DB update failure) </summary>
        public Error UpdateUserFailed { get; } = new Error {Code = 2192, Message = "Update user failed (DB update failure)"};

        /// <summary>Customer Id is missing</summary>
        public Error MissingUserId { get; } = new Error {Code = 3, Message = "Please provide a user id"};

        /// <summary>Provided Facebook access token is invalid</summary>
        public Error FacebookAccessTokenInvalid { get; } = new Error {Code = 1, Message = "Facebook access token is invalid"};
        
        /// <summary>Provided Google access token is invalid</summary>
        public Error GoogleAccessTokenInvalid { get; } = new Error {Code = 1, Message = "Google login response code is invalid"};

        /// <summary>Provided code to change password is invalid</summary>
        public Error RecreatePasswordCodeInvalid { get; } = new Error {Code = 3, Message = "Code to recreate password is invalid"};

        /// <summary>Confirmation code is missing in request. </summary>
        public Error ConfirmationMissing { get; } = new Error {Code = 2064, Message = "No confirmation code in request"};
        
        #endregion
        
        #region Favourites
        
        /// <summary>Favourite item already exists </summary>
        public Error FavouriteExists { get; } = new Error {Code = 2211, Message = "Item already exists"};

        /// <summary>Favourite item couldn't be added (DB insert failure) </summary>
        public Error FavouriteAddFailed { get; } =
            new Error {Code = 2212, Message = "Item couldn't be added (DB insert failure)"};

        /// <summary>Favourite item couldn't be updated (DB update failure) </summary>
        public Error FavouriteUpdateFailed { get; } =
            new Error {Code = 2221, Message = "Item couldn't be updated (DB update failure)"};
        
        /// <summary>Favourite item couldn't be deleted (DB delete failure) </summary>
        public Error FavouriteDeleteFailed { get; } =
            new Error {Code = 2230, Message = "Item couldn't be deleted (DB delete failure)"};

        /// <summary>Favourite item is missing from query or command</summary>
        public Error FavoriteItemMissing { get; } = new Error {Code = 3, Message = "Please provide a favorite item"};
        
        #endregion

        #region Watch History

        /// <summary>Item already exists. Used in watch history </summary>
        public Error WatchHistoryItemExists { get; } = new Error {Code = 2311, Message = "Item already exists"};

        /// <summary>Item couldn't be added (DB insert failure). Used in watch history </summary>
        public Error WatchHistoryInsertFailed { get; } = new Error {Code = 2312, Message = "Item couldn't be added (DB insert failure)"};

        /// <summary>Item couldn't be updated (DB update failure). Used in watch history </summary>
        public Error WatchHistoryUpdateFailed { get; } = new Error {Code = 2321, Message = "Item couldn't be updated (DB update failure)"};
        
        /// <summary>Item couldn't be deleted (DB delete failure). Used in watch history </summary>
        public Error WatchHistoryDeleteFailed { get; } = new Error {Code = 2330, Message = "Item couldn't be deleted (DB delete failure)"};
        
        #endregion

        #region Watch list

        /// <summary>Item already exists. Used in watch list </summary>
        public Error WatchListItemExists { get; } = new Error {Code = 2411, Message = "Item already exists"};

        /// <summary>Item couldn't be added (DB insert failure). Used in watch list  </summary>
        public Error WatchListInsertFailed { get; } = new Error {Code = 2412, Message = "Item couldn't be added (DB insert failure)"};

        /// <summary>Item couldn't be updated (DB update failure). Used in watch list </summary>
        public Error WatchListUpdateFailed { get; } = new Error {Code = 2421, Message = "Item couldn't be updated (DB update failure)"};
        
        /// <summary>Item couldn't be deleted (DB delete failure). Used in watch list </summary>
        public Error WatchListDeleteFailed { get; } = new Error {Code = 2430, Message = "Item couldn't be deleted (DB delete failure)"};
        
        #endregion
        
        #region Settings

        /// <summary>Settings item is missing in request</summary>
        public Error SettingsItemMissing { get; } = new Error {Code = 3, Message = "No item in request"};
        
        /// <summary>Settings item already exists </summary>
        public Error SettingsExists { get; } = new Error {Code = 2511, Message = "Item already exists"};

        /// <summary>Settings item couldn't be added (DB insert failure) </summary>
        public Error SettingsAddFailed { get; } =
            new Error {Code = 2512, Message = "Item couldn't be added (DB insert failure)"};

        /// <summary>Item couldn't be updated (DB update failure) </summary>
        public Error SettingsUpdateFailed { get; } =
            new Error {Code = 2521, Message = "Item couldn't be updated (DB update failure)"};
        
        /// <summary>Item couldn't be deleted (DB delete failure) </summary>
        public Error SettingsDeleteFailed { get; } =
            new Error {Code = 2530, Message = "Item couldn't be deleted (DB delete failure)"};
        
        #endregion
        
        #region Reminders
        
        /// <summary>Reminder item already exists </summary>
        public Error ReminderExists { get; } = new Error {Code = 2611, Message = "Item already exists"};

        /// <summary>Reminder item couldn't be added </summary>
        public Error ReminderAddFailed { get; } = new Error {Code = 2612, Message = "Item couldn't be added"};

        /// <summary>Reminder item couldn't be deleted (DB delete failure) </summary>
        public Error ReminderDeleteFailed { get; } = new Error {Code = 2630, Message = "Item couldn't be deleted (DB delete failure)"};

        /// <summary>Reminder item couldn't be updated (DB update failure) </summary>
        public Error ReminderUpdateFailed { get; } = new Error {Code = 2621, Message = "Item couldn't be updated (DB update failure)"};
        
        #endregion
        
        #region Customer

        /// <summary>The customer could not be found. </summary>
        public Error CustomerNotFound { get; } = new Error {Code = 2, Message = "The customer could not be found."};
        
        /// <summary>The customer already exists. </summary>
        public Error CustomerExists { get; } = new Error {Code = 2900, Message = "The customer already exists."};

        /// <summary>The customer could not be created (DB insert failure) </summary>
        public Error CustomerCreateFailed { get; } = new Error
        {
            Code = 2901,
            Message = "The customer could not be created (DB insert failure)"
        };

        /// <summary>The customer is already deleted. </summary>
        public Error CustomerDeleted { get; } = new Error {Code = 2902, Message = "The customer is already deleted."};

        /// <summary>The customer could not be deleted (DB delete failure) </summary>
        public Error CustomerDeleteFailed { get; } = new Error
        {
            Code = 2903,
            Message = "The customer could not be deleted (DB delete failure)"
        };

        /// <summary>
        /// The customer could not be updated. This email address is used already by some other user.
        /// </summary>
        public Error CustomerEmailUsed { get; } = new Error
        {
            Code = 2904,
            Message = "The customer could not be updated. This email address is used already by some other user."
        };

        /// <summary>The customer could not be updated (DB update failure) </summary>
        public Error CustomerUpdateFailed { get; } = new Error
        {
            Code = 2905,
            Message = "The customer could not be updated (DB update failure)"
        };

        /// <summary>
        /// Customer's password could not be updated. Email and mobile are blank
        /// </summary>
        public Error CustomerEmailAndMobileBlank { get; } = new Error
        {
            Code = 2906,
            Message = "Account has blank email and mobile."
        };

        /// <summary>
        /// Customer's password could not be updated. Account is not active
        /// </summary>
        public Error CustomerAccountIsNotActive { get; } = new Error
        {
            Code = 2907,
            Message = "Account is not activated."
        };
        
        /// <summary>Customer Id is missing</summary>
        public Error MissingCustomerId { get; } = new Error {Code = 3, Message = "Please provide a customer id"};

        /// <summary>Customer data is invalid</summary>
        public Error InvalidCustomerData = new Error {Code = 3, Message = "Invalid customer data"};

        #endregion

        #region AccessValidationByIP

        /// <summary>Item already exists.</summary>
        public Error RequestURLEntryExistsWithThisIpAddress { get; } = new Error { Code = 2908, Message = "Item already exists" };

        /// <summary>AccessValidationByIP item couldn't be added (DB insert failure) </summary>
        public Error AccessValidationByIPAddFailed { get; } = new Error { Code = 2909, Message = "Item couldn't be added (DB insert failure)" };

        /// <summary>AccessValidationByIP item couldn't be deleted (DB delete failure) </summary>
        public Error AccessValidationByIPDeleteFailed { get; } = new Error { Code = 2909, Message = "Item couldn't be deleted (DB delete failure)" };

        /// <summary>AccessValidationByIP item couldn't be updated (DB update failure) </summary>
        public Error AccessValidationByIPUpdateFailed { get; } = new Error { Code = 2910, Message = "Item couldn't be updated (DB update failure)" };
        #endregion

        #region UserProfileUpdateHistory

        /// <summary>UserProfileUpdateHistory item couldn't be added (DB insert failure) </summary>
        public Error UserProfileUpdateHistoryAddFailed { get; } = new Error { Code = 2909, Message = "Item couldn't be added (DB insert failure)" };

        #endregion
    }
}