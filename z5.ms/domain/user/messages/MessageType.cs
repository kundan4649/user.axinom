namespace z5.ms.domain.user.messages
{
    /// <summary>Queue message type for async repositories</summary>
    public enum MessageType
    {
        /// <summary>Add a new record</summary>
        Add,
        
        /// <summary>Update existing record</summary>
        Update,
        
        /// <summary>Delete a record</summary>
        Delete
    }
}
