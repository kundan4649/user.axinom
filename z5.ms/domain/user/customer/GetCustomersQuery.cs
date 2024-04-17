using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.attributes;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.customer
{ 
    /// <summary>Get sorted and paginated list of customers</summary>
    public class GetCustomersQuery : PagingQuery<Customers, CustomerSortByField>
    {
        /// <summary>Filter by ID</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "id")]
        public string Id { get; set; }

        /// <summary>Filter the items by a list of IDs</summary>
        /// <remarks>Show only those items, whose ID is part of the parameter list</remarks>
        [FromQueryCsv(Name = "ids")]
        public List<string> Ids { get; set; }

        /// <summary>The first name of the customer</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "first_name")]
        public string FirstName { get; set; }

        /// <summary>The first name of the customer</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "last_name")]
        public string LastName { get; set; }

        /// <summary>The email address of the customer</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "email")]
        public string Email { get; set; }

        /// <summary>The mobile phone number of the customer</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "mobile")]
        public string Mobile { get; set; }

        /// <summary>Country in “ISO 3166-1 alpha-2” format from where the user initially registered.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "country")]
        [StringLength(2)]
        public string RegistrationCountry { get; set; }

        /// <summary>The system from where the customers came from (see system enum)</summary>
        [JsonProperty("system", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [FromQuery(Name = "system")]
        public string System { get; set; }
    }

    /// <summary>Validator for get customers query</summary>
    public class GetCustomersQueryValidator : PagingQueryValidator<GetCustomersQuery, CustomerSortByField, Customers>
    {
        /// <inheritdoc />
        public GetCustomersQueryValidator()
        {
            RuleFor(q => q.Id).Must(BeValidGuid).WithMessage("Please specify a valid customer ID");
            RuleFor(q => q.Ids).Must(BeValidGuids).WithMessage("Please specify valid customer IDs");
            RuleFor(c => c.RegistrationCountry).Must(BeValidCountryCode).WithMessage("Please specify a valid two-letter country code");
        }
        
        private static bool BeValidGuid(string guid)
            => guid == null || Guid.TryParse(guid, out _);

        private static bool BeValidGuids(IEnumerable<string> guids)
            => guids == null || guids.All(BeValidGuid);

        private static bool BeValidCountryCode(string language)
            => string.IsNullOrWhiteSpace(language) || language.Length == 2;
    }
}