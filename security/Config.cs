using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Security
{
    public class Config
    {
        public static List<Client> Clients
        {
            get
            {
                var result = new List<Client>();

                result.Add(new Client
                {
                    Enabled = true,
                    ClientId = "curl",
                    ClientName = "cURL",
                    ClientSecrets = { new Secret("password".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "series.read" }
                });

                return result;
            }
        }

        public static List<TestUser> Users
        {
            get
            {
                var result = new List<TestUser>();

                result.Add(new TestUser
                {
                    IsActive = true,
                    ProviderName = "local",
                    ProviderSubjectId = Guid.NewGuid().ToString(),
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "test",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Test")
                    }
                });

                return result;
            }
        }

        public static List<ApiScope> Scopes
        {
            get
            {
                var result = new List<ApiScope>();

                result.Add(new ApiScope
                {
                    Enabled = true,
                    Required = true,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    Name = "series.read",
                    Description = "series.read",
                    DisplayName = "series.read"
                });

                result.Add(new ApiScope
                {
                    Enabled = true,
                    Required = true,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    Name = "config.read",
                    Description = "config.read",
                    DisplayName = "config.read"
                });

                result.Add(new ApiScope
                {
                    Enabled = true,
                    Required = true,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    Name = "config.write",
                    Description = "config.write",
                    DisplayName = "config.write"
                });

                return result;
            }
        }

        public static List<ApiResource> ApiResources
        {
            get
            {
                var result = new List<ApiResource>();

                result.Add(new ApiResource
                {
                    Enabled = true,
                    ShowInDiscoveryDocument = true,
                    Name = "frontend",
                    Description = "frontend",
                    DisplayName = "frontend",
                    ApiSecrets = { new Secret("frontendpassword".Sha256()) },
                    Scopes = new[] { "series.read", "config.read", "config.write" }
                });

                result.Add(new ApiResource
                {
                    Enabled = true,
                    ShowInDiscoveryDocument = true,
                    Name = "backend",
                    Description = "backend",
                    DisplayName = "backend",
                    ApiSecrets = { new Secret("backendpassword".Sha256()) },
                    Scopes = new[] { "generate", "write", "sum.read", "count.read", "mean.read", "variance.read", "deviation.read", "reset" }
                });

                return result;
            }
        }

        public static List<IdentityResource> IdentityResources
        {
            get
            {
                var result = new List<IdentityResource>();

                result.Add(new IdentityResources.OpenId());

                result.Add(new IdentityResources.Profile());

                result.Add(new IdentityResource
                {
                    Enabled = true,
                    Required = true,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    Name = "debug",
                    Description = "debug",
                    DisplayName = "debug"
                });

                return result;
            }
        }
    }
}