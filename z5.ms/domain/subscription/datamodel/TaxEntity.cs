using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace z5.ms.domain.subscription.datamodel
{
    /// <summary>
    /// DB DTO for tax data structure.
    /// </summary>
    [Table("Taxes")]
    public class TaxEntity
    {
        /// <summary>Unique ID of the tax</summary>
        public Guid Id { get; set; }
        
        /// <summary>The name or abbreviation of the tax</summary>
        public string Name { get; set; }
        
        /// <summary>Country code where the tax is applicable, in "ISO 3166-1 alpha-2" format</summary>
        public string CountryCode { get; set; }
        
        /// <summary>The percentage of the tax</summary>
        public double Percentage { get; set; }
    }
}
