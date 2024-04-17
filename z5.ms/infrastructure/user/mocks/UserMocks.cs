using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using User = z5.ms.domain.user.viewmodels.User;

namespace z5.ms.infrastructure.user.mocks
{
    /// <summary>
    /// Mock data for testing User endpoints
    /// </summary>
    public class UserMocks
    {
        /// <summary>Mock user data</summary>
        public List<User> Users = new List<User>
        {
            new User
            {
                Id = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                System = "Internal",
                Email = "user1@gmail.com",
                Mobile = "918080096097",
                RegistrationCountry = "IN",
                FirstName = "Martin",
                LastName = "Smith",
                MacAddress = "B3-49-AC-A1-FB-A6",
                Birthday = new DateTime(1970,1,1),
                Gender = Gender.Male,
                ActivationDate = DateTime.Parse("2017-01-13T09:20:50.52Z"),
                Activated = true
            },
            new User
            {
                Id = Guid.Parse("e5b1a3b4-bb25-4796-b73e-415818223775"),
                System = "Internal",
                Email = "user2@gmail.com",
                Mobile = "918080096098",
                RegistrationCountry = "IN",
                FirstName = "John",
                LastName = "Doe",
                MacAddress = "DA-FD-AC-A1-BC-45",
                Birthday = new DateTime(1989,5,9),
                Gender = Gender.Male,
                ActivationDate = DateTime.Parse("2017-03-20T17:43:31.27Z"),
                Activated = true
            }
        };

        /// <summary>Mock user entity data</summary>
        public List<UserEntity> UserEntities = new List<UserEntity>
        {
            new UserEntity
            {
                Id = Guid.Parse("4ad1877e-3cb3-4dfb-b5d1-9faebff2bf29"),
                System = "Internal",
                Email = "user1@gmail.com",
                Mobile = "918080096097",
                PasswordHash = "SNaXQ+O093TSCFJa389ECA==", // pass: pass123
                RegistrationCountry = "IN",
                FirstName = "Martin",
                LastName = "Smith",
                MacAddress = "B3-49-AC-A1-FB-A6",
                Birthday = new DateTime(1970,1,1),
                Gender = Gender.Male,
                ActivationDate = DateTime.Parse("2017-01-13T09:20:50.52Z"),
                EmailConfirmationKey = "4HuMq1jaw8aSsrcXgWzz",
                MobileConfirmationKey = "4HuMq1jaw8aSsrcXgWzz",
                PasswordResetKey = "4HuMq1jaw8aSsrcXgWzz",
                State = UserState.Verified
            },
            new UserEntity
            {
                Id = Guid.Parse("e5b1a3b4-bb25-4796-b73e-415818223775"),
                System = "Internal",
                Email = "user2@gmail.com",
                Mobile = "918080096098",
                PasswordHash = "SNaXQ+O093TSCFJa389ECA==", // pass: pass123
                RegistrationCountry = "IN",
                FirstName = "John",
                LastName = "Doe",
                MacAddress = "DA-FD-AC-A1-BC-45",
                Birthday = new DateTime(1989,5,9),
                Gender = Gender.Male,
                ActivationDate = DateTime.Parse("2017-03-20T17:43:31.27Z"),
                EmailConfirmationKey = "4HuMq1jaw8aSsrcXgWzz",
                MobileConfirmationKey = "4HuMq1jaw8aSsrcXgWzz",
                PasswordResetKey = "4HuMq1jaw8aSsrcXgWzz",
                State = UserState.Verified
            }
        };

        /// <summary>Mock user auth tokens</summary>
        public List<ClaimsPrincipal> AuthTokens => new List<ClaimsPrincipal>
        {
            new ClaimsPrincipal(new ClaimsIdentity(
                new[]
                {
                    new Claim("sub", Guid.NewGuid().ToString()),
                    new Claim("activation_date", DateTime.UtcNow.AddDays(-10).ToLongDateString()),
                    new Claim("system", "Internal"),
                    new Claim("user_email", "user1@email.com"),
                    new Claim("user_mobile", "123456789")
                })),
            new ClaimsPrincipal(new ClaimsIdentity(
                new[]
                {
                    new Claim("sub", Guid.NewGuid().ToString()),
                    new Claim("activation_date", DateTime.UtcNow.AddDays(-10).ToLongDateString()),
                    new Claim("system", "Internal"),
                    new Claim("user_email", "user2@email.com"),
                    new Claim("user_mobile", "987654321")
                }))
        };

        /// <summary>Mock user JWT auth tokens</summary>
        public List<JObject> Tokens = new List<JObject>
        {
            JsonConvert.DeserializeObject<JObject>("{\"access_token\":\"eyJhbGciOiJSUzI1NiIsImtpZCI6ImY2ODEzNDY5NmM3MjJjOThmYWMzNTVhODI5MGM3MGVmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1Mzk2MTQ3ODYsImV4cCI6MTUzOTYxNjU4NiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MzY4IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTM2OC9yZXNvdXJjZXMiLCJzdWJzY3JpcHRpb25hcGkiLCJ1c2VyYXBpIl0sImNsaWVudF9pZCI6InJlZnJlc2hfdG9rZW5fY2xpZW50Iiwic3ViIjoiZWQzZGRiZTctNzVmZi00N2VhLWIzMmYtYWZiYTczMGY3ZGE3IiwiYXV0aF90aW1lIjoxNTM5NjE0NzgzLCJpZHAiOiJsb2NhbCIsInVzZXJfaWQiOiJlZDNkZGJlNy03NWZmLTQ3ZWEtYjMyZi1hZmJhNzMwZjdkYTciLCJ1c2VyX2VtYWlsIjoiZ3VyQGF4aW5vbS5jb20iLCJ1c2VyX21vYmlsZSI6IiIsInN5c3RlbSI6Ilo1IiwiYWN0aXZhdGlvbl9kYXRlIjoiMjAxOC0wOS0yNlQxNDoxNjowMiIsImNyZWF0ZWRfZGF0ZSI6IjIwMTgtMDktMjZUMTQ6MTQ6NTgiLCJyZWdpc3RyYXRpb25fY291bnRyeSI6IlpaIiwiY3VycmVudF9jb3VudHJ5IjoiWloiLCJzY29wZSI6WyJzdWJzY3JpcHRpb25hcGkiLCJ1c2VyYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImRlbGVnYXRpb24iXX0.gJvwUgAL_lpCKLWbSmWzhZKbC0TYW8KrJC9TC2KXT87tzHxydkPu2Gja2hEb8zF9wreGAr05BSTm03Tcf1Ofyyl18vpqoGASjDRub4uYCsudiLxDQ1TlDnpliRll6Ku8T1XXlVmYSYR5999mWq4HzGybtMUZvtxqCt_HBRSXgBfCxjC0PMX0IJxwqqv5kFcLbURqm6HLv1MneXCWwLfw03_dfT4CFuZFfXvhAx_VR_xYZUr_23KRCZiPO-ep8hqc_8Gx2BbZh9BWZtNSoyY6nycHEkI0UzBpCAu5RD94xYCcRohFvXiiWtn_CPBC9G2CKtXg5IXDlN1RLM0HvzVYdQ\",\"expires_in\":1800,\"token_type\":\"Bearer\",\"refresh_token\":\"b66430715863b8550cece92781219c52713ea4d7d82fc845d9f6d0afb4f4b864\"}")
        };
    }
}
