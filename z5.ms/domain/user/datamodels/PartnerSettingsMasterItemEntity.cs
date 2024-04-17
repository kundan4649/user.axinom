using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace z5.ms.domain.user.datamodels
{
    [Table("PartnerSettingsMaster")]
    public class PartnerSettingsMasterItemEntity
    {
        /// <summary>Unique Id of the PartnerSettingsMaster entity</summary>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>Id of the partner</summary>
        public string PartnerId { get; set; }

        /// <summary>Name of the partner</summary>
        public string PartnerName { get; set; }

        /// <summary>Json string of the partner</summary>
        public string Json { get; set; }

        /// <summary>Entry date</summary>
        public DateTime DateCreated { get; set; }

        /// <summary>Is the json string considered for validation / retrieval</summary>
        public bool IsActive { get; set; }
    }
}