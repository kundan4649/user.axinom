using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Dapper.FastCrud;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.customer;

namespace customer
{
    /// <summary>
    /// Unit tests for verifying that the <see cref="GetCustomersQuery"/> is handled correctly by the <see cref="GetCustomerQueryHandler"/>
    /// </summary>
    public class GetCustomersQueryHandlerTests
    {
        private readonly IDbConnection _connection;
        private readonly IMapper _mapper;

        private readonly List<UserEntity> _userEntities = new List<UserEntity>
        {
            new UserEntity
            {
                Email = "test@test.test",
                FirstName = "John",
                LastName = "Smith",
                Id = Guid.Parse("4da309bb-f852-4bb1-b2b8-bff3cfc2bc46"),
                Mobile = "123456789",
                RegistrationCountry = "EE",
                PasswordHash = "pw",
                System = "internal"
            },
            new UserEntity
            {
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe",
                Id = Guid.Parse("c7cada2b-2610-48bc-861f-d2830c30fca7"),
                Mobile = "123456789",
                RegistrationCountry = "EE",
                PasswordHash = "pw2"
            }
        };

        private readonly GetCustomersQueryHandler _handler;

        /// <inheritdoc />
        public GetCustomersQueryHandlerTests()
        {
            SqlLiteHelpers.ConfigureDapper();
            _connection = SqlLiteHelpers.CreateTestDatabase();
            _connection.Execute(@"
            CREATE TABLE [Users] (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                    Email NVARCHAR NULL,
                                                    Mobile NVARCHAR NULL,
                                                    FirstName NVARCHAR NOT NULL,
                                                    LastName NVARCHAR NOT NULL,
                                                    PasswordHash NVARCHAR NOT NULL,
                                                    EmailConfirmationKey NVARCHAR NULL,
                                                    MobileConfirmationKey NVARCHAR NULL,
                                                    EmailConfirmationExpiration DATETIME NULL,
                                                    MobileConfirmationExpiration DATETIME NULL,
                                                    IsEmailConfirmed BIT NULL,
                                                    IsMobileConfirmed BIT NULL,
                                                    PasswordResetKey NVARCHAR(48),
                                                    PasswordResetExpiration DATETIME NULL,
                                                    LastLogin DATETIME NULL,
                                                    System INT NULL,
                                                    RegistrationCountry NVARCHAR NULL,
                                                    MacAddress NVARCHAR NULL,
                                                    Birthday DATETIME NULL,
                                                    Gender INT NULL,
                                                    ActivationDate DATETIME NULL,
                                                    CreationDate DATETIME NULL,
                                                    State INT NULL,
                                                    RegistrationRegion NVARCHAR NULL,
                                                    FacebookUserId NVARCHAR NULL,
                                                    GoogleUserId NVARCHAR NULL,
                                                    TwitterUserId NVARCHAR NULL,
                                                    AmazonUserId NVARCHAR NULL,
                                                    B2BUserId NVARCHAR NULL,
                                                    ProviderName NVARCHAR NULL,
                                                    ProviderSubjectId NVARCHAR NULL,
                                                    NewEmail NVARCHAR NULL,
                                                    NewEmailConfirmationKey NVARCHAR NULL,
                                                    NewEmailConfirmationExpiration NVARCHAR NULL,
                                                    IpAddress NVARCHAR NULL,
                                                    Json NVARCHAR NULL
                                                    )"
            );

            AddEntitiesToDb(_userEntities);
            _mapper = new MapperConfiguration(m => m.AddProfile<CustomerProfile>()).CreateMapper();

            var options = Options.Create(new DbConnectionOptions
            {
                ReplicaDatabaseConnection = _connection.ConnectionString
            });
            _handler = new GetCustomersQueryHandler(_mapper, options);
        }

        /// <summary>Ensure that handler returns customers with valid query</summary>
        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var command = new GetCustomersQuery
            {
                Email = "test@test.test",
                FirstName = "John",
                LastName = "Smith",
                Id = "4da309bb-f852-4bb1-b2b8-bff3cfc2bc46",
                Mobile = "123456789",
                RegistrationCountry = "EE",
                System = "internal"
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            result.Success.Should().BeTrue();
            result.Value.CustomerList.Should().HaveCount(1);
        }

        /// <summary>Verify that the correct <see cref="Customer"/> data is returned by the handle method.</summary>
        [Fact]
        public async void Handle_HasCorrectFields()
        {
            // Arrange
            var correctData = _userEntities.First();
            var expectedCustomer = _mapper.Map<UserEntity, Customer>(correctData);
            var command = new GetCustomersQuery
            {
                Id = correctData.Id.ToString()
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            result.Success.Should().BeTrue();
            result.Value.CustomerList.Should().NotBeNullOrEmpty();
            result.Value.CustomerList.First().Should().BeEquivalentTo(expectedCustomer);
        }

        /// <summary>Verify that Handle returns multiple customers matching the query</summary>
        [Fact]
        public async void Handle_Success_MultipleCustomers()
        {
            // Arrange
            var command = new GetCustomersQuery
            {
                FirstName = "John"
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            result.Success.Should().BeTrue();
            result.Value.CustomerList.Count.Should().Be(2);
        }


        /// <summary>Verify that querying customers by a list of IDs works as expected.</summary>
        [Fact]
        public async void Handle_Success_QueryByIDs()
        {
            var userIds = _userEntities
                .Where(entity => entity.FirstName == "John")
                .Select(entity => entity.Id.ToString())
                .ToList();
            // Arrange
            var command = new GetCustomersQuery
            {
                Ids = userIds
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            result.Success.Should().BeTrue();
            result.Value.CustomerList.Count.Should().Be(userIds.Count);
        }

        private void AddEntitiesToDb(List<UserEntity> entities)
        {
            entities.ForEach(entity => _connection.Insert(entity));
        }
    }
}