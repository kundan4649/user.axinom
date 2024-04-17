using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper.FastCrud;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using Microsoft.Extensions.Options;
using z5.ms.common.infrastructure.db;

namespace z5.ms.infrastructure.user.customer
{
    /// <summary>Handler for getting paged customers query</summary>
    public class GetCustomersQueryHandler : DbQueryHandler<GetCustomersQuery, Result<Customers>>
    {
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public GetCustomersQueryHandler(IMapper mapper, IOptions<DbConnectionOptions> options)
            : base(options.Value.ReplicaDatabaseConnection)
        {
            _mapper = mapper;
        }

        /// <inheritdoc />
        public override async Task<Result<Customers>> Handle(GetCustomersQuery message)
        {
            var (queryString, queryParameters) = ConstructDatabaseQuery(message);

            using (var connection = Connection)
            {
                var totalCount = message.PageSize;
                var result = await connection.FindAsync<UserEntity>(queryBuilder => queryBuilder
                    .Where($"{queryString}").WithParameters(queryParameters)
                    .OrderBy($"{Enum.GetName(typeof(CustomerSortByField), message.SortByField)} {Enum.GetName(typeof(SortOrder), message.SortOrder)}")
                    .Skip((message.Page - 1) * message.PageSize)
                    .Top(message.PageSize));

                try
                {
                    totalCount = await connection.CountAsync<UserEntity>(queryBuilder => queryBuilder
                        .Where($"{queryString}").WithParameters(queryParameters).WithTimeout(TimeSpan.FromSeconds(5)));
                }
                catch
                {
                    // ignored
                }

                var customers = new Customers
                {
                    Total = totalCount,
                    Page = message.Page,
                    PageSize = message.PageSize,
                    CustomerList = result.Select(user => _mapper.Map<UserEntity, Customer>(user)).ToList()
                };

                return Result<Customers>.FromValue(customers);
            }
        }

        private static (string, object) ConstructDatabaseQuery(GetCustomersQuery message) =>
            (GetQueryString(message), GetQueryParameters(message));

        private static object GetQueryParameters(GetCustomersQuery message)
        {
            var queryParameters = new
            {
                Id = message.Id.ToGuid(),
                Ids = message.Ids?.Select(id => id.ToGuid()),
                message.FirstName,
                message.LastName,
                message.Email,
                message.Mobile,
                message.RegistrationCountry,
                message.System
            };
            return queryParameters;
        }

        private static string GetQueryString(GetCustomersQuery message)
        {
            var queryString = $"State <> {(int)UserState.Deleted}";

            if (message.Ids != null && message.Ids.Count > 0)
                queryString += " AND Id IN @Ids";
            if (!string.IsNullOrWhiteSpace(message.Id))
                queryString += " AND Id = @Id";
            if (!string.IsNullOrWhiteSpace(message.FirstName))
                queryString += " AND FirstName = @FirstName";
            if (!string.IsNullOrWhiteSpace(message.LastName))
                queryString += " AND LastName = @LastName";
            if (!string.IsNullOrWhiteSpace(message.Email))
                queryString += " AND Email = @Email";
            if (!string.IsNullOrWhiteSpace(message.Mobile))
                queryString += " AND Mobile = @Mobile";
            if (!string.IsNullOrWhiteSpace(message.RegistrationCountry))
                queryString += " AND RegistrationCountry = @RegistrationCountry";
            if (message.System != null)
                queryString += " AND System = @System";

            return queryString;
        }
    }
}