using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace z5.ms.domain.user.datamodels
{
    [Table("AccessValidationByIP")]
    public class AccessValidationByIPItemEntity
    {
        /// <summary>Unique Id of the AccessValidationByIP entity</summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>Source ip address of the request</summary>
        public string IpAddress { get; set; }

        /// <summary>Requested end point</summary>
        public string RequestEndPoint { get; set; }

        /// <summary>Number of hits received from the Ip for the end point</summary>
        public int RequestCount { get; set; }

        /// <summary>Request logged date</summary>
        public DateTime DateCreated { get; set; }

        /// <summary>Date on which last request is received from the Ip for the end point</summary>
        public DateTime LastRequestReceivedDate { get; set; }

        /// <summary>Date on which the last request is blocked from the Ip for the end point</summary>
        public DateTime? RequestBlockedDate { get; set; }
    }
}