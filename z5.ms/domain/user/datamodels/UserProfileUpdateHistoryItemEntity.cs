using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace z5.ms.domain.user.datamodels
{
    [Table("UserProfileUpdateHistory")]
    public class UserProfileUpdateHistoryItemEntity
    {
        /// <summary>Unique Id of the UserProfileUpdateHistory entity</summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>User id of the user profile</summary>
        public Guid UserId { get; set; }

        /// <summary>Email id of the user profile just before the update</summary>
        public string EmailId { get; set; }

        /// <summary>Mobile number of the user profile just before the update</summary>
        public string MobileNumber { get; set; }

        /// <summary>User profile update request payload</summary>
        public string RequestPayload { get; set; }

        /// <summary>Source ip address from which the update profile request is received</summary>
        public string IpAddress { get; set; }

        /// <summary>Country code from which the update profile request is received</summary>
        public string CountryCode { get; set; }

        /// <summary>Update profile request received date</summary>
        public DateTime DateCreated { get; set; }

        /// <summary>Password updated for the user</summary>
        public bool PasswordUpdated { get; set; }
    }
}