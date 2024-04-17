using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using z5.ms.common.validation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using z5.ms.common.assets.common;
using z5.ms.common.validation.authproviders;
using Type = System.Type;

namespace z5.ms.userservice
{
    /// <summary>MS API Swagger registration helper</summary>
    public static class Swagger
    {
        /// <summary>Add API description and filters</summary>
        public static void SetApiSwaggerOptions(this SwaggerGenOptions o, string apiName, string version, IConfiguration configuration)
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);

            //add all XMLdoc files
            foreach (var xmlDocPath in Directory.EnumerateFiles(directory, "*.xml", SearchOption.TopDirectoryOnly))
            {
                if (File.Exists(xmlDocPath))
                    o.IncludeXmlComments(xmlDocPath);
            }

            o.MapType<Guid>(() => new Schema { Type = "string", Format = "uuid" });
            o.MapType<Guid?>(() => new Schema { Type = "string", Format = "uuid" });
            o.DescribeAllEnumsAsStrings();

            o.SwaggerDoc(version,
                new Info
                {
                    Title = apiName,
                    Version = version,
                    Description = apiName,
                    TermsOfService = "Proprietary API",
                    Contact = new Contact
                    {
                        Email = "info@axinom.com"
                    },
                    License = new License
                    {
                        Name = "Proprietary license",
                        Url = "/license"
                    }
                }
            );

            o.AddSecurityDefinition("bearer", new ApiKeyScheme
            {
                Type = "apiKey",
                Description = "User authentication via token. The following has to be present in the HTTP header: 'Authorization: Bearer [token-from-login-response]'",
                Name = "Authorization",
                In = "header"
            });

            o.AddSecurityDefinition("cms", new ApiKeyScheme
            {
                Type = "apiKey",
                Description = "CMS app authentication via API key. The following has to be present in the HTTP header: 'Authorization: Cms [shared-secret]'",
                Name = "Authorization",
                In = "header"
            });

            var crmScheme = configuration.GetSection("CrmApiSecret").Value.Split(' ')[0];
            o.AddSecurityDefinition("crm", new ApiKeyScheme
            {
                Type = "apiKey",
                Description = $"CRM authentication via API key and signature. The following has to be present in the HTTP header: 'Authorization: {crmScheme} [shared-secret]:[signature]'",
                Name = "Authorization",
                In = "header"
            });

            o.CustomSchemaIds(s => ToUnderscoreCase(s.Name));
            //Assign scope requirements to operations based on AuthAttribute
            o.OperationFilter<SecurityRequirementsOperationFilter>();
            var integerEnums = new Dictionary<string, System.Type>{ { "asset_type", typeof(AssetType) } };
            o.SchemaFilter<SingleEnumDefinition>(integerEnums);
            o.OperationFilter<SingleEnumDefinition>(integerEnums);
            o.SchemaFilter<CustomizeSchemas>();
            o.OperationFilter<FixOperations>();
            
        }

        /// <summary>Add Swagger middleware and Swagger UI to the API</summary>
        public static void AddApiSwaggerWithUI(this IApplicationBuilder app, string apiName, string version, string routeSecret)
        {
            //Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            //Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint($"/swagger/{version}/swagger.json", apiName);
                o.RoutePrefix = $"help{(string.IsNullOrWhiteSpace(routeSecret) ? "" : $"/{routeSecret}")}";
                o.DocExpansion(DocExpansion.None);
                //o.EnabledValidator();
            });
        }

        internal static string ToUnderscoreCase(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", "_$0", System.Text.RegularExpressions.RegexOptions.Compiled).ToLowerInvariant();
        }
    }

    /// <summary>Authorization filter from auto-generated swagger page</summary>
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var security = new List<IDictionary<string, IEnumerable<string>>>();
            var authAttr = GetAuthAtrAttribute(context);

            if (ContainsProvider<JwtAuthProvider>(authAttr) || ContainsProvider<OAuthJwtAuthProvider>(authAttr))
                security.Add(new Dictionary<string, IEnumerable<string>> { { "bearer", new List<string>() } });

            if (ContainsProvider<CmsInternalAuthProvider>(authAttr))
                security.Add(new Dictionary<string, IEnumerable<string>> { { "cms", new List<string>() } });

            if (ContainsProvider<CrmSignedAuthProvider>(authAttr))
                security.Add(new Dictionary<string, IEnumerable<string>> { { "crm", new List<string>() } });

            if (security.Any())
                operation.Security = security;
        }

        private static bool ContainsProvider<T>(CustomAttributeData authAttr) where T : IAuthProvider
        {
            return authAttr?.ConstructorArguments?
                .SelectMany(a => a.Value as IEnumerable<CustomAttributeTypedArgument>)
                .Any(c => (Type)c.Value == typeof(T)) ?? false;
        }

        private static CustomAttributeData GetAuthAtrAttribute(OperationFilterContext context)
        {
            var attributes = new List<CustomAttributeData>();
            attributes.AddRange(context?.MethodInfo?.CustomAttributes);
            attributes.AddRange(((ControllerActionDescriptor)context?.ApiDescription?.ActionDescriptor)?.ControllerTypeInfo?.CustomAttributes);

            return attributes.FirstOrDefault(a => a.AttributeType == typeof(AuthorizeAttribute));
        }
    }

    /// <summary>
    /// Extract enumerations as definitions and not have them inline in multiple places
    /// </summary>
    public class SingleEnumDefinition : ISchemaFilter, IOperationFilter
    {
        private readonly Dictionary<string, System.Type> _useTypesAsInteger;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="useTypesAsInteger">A dictionary of key is the swagger object name and type the C# class type for those types that should use integer values.</param>
        public SingleEnumDefinition(Dictionary<string, System.Type> useTypesAsInteger)
        {
            _useTypesAsInteger = useTypesAsInteger;
        }

        /// <summary>
        /// Extract enumerations as custom types within the given schema
        /// This is about objects having enum property types. The enumeration values
        /// should no be defined inline but extracted as custom definitions.
        /// </summary>
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (model.Properties == null)
                return;

            // search the schema for all enumeration properties and array enumeration properties
            var enumProperties = model.Properties.Where(p => p.Value.Enum != null)
                .Union(model.Properties.Where(p => p.Value.Items?.Enum != null)).ToList();

            // The found schema properties do not know the C# system type which we need to extract
            // the enumeration as a custom object.
            // So we have to find all the enumeration system types that are defined on the schema SystemType.
            var enums = context.SystemType.GetProperties()
                .Select(p => Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType.GetElementType() ??
                             p.PropertyType.GetGenericArguments().FirstOrDefault() ?? p.PropertyType)
                .Where(p => p.GetTypeInfo().IsEnum)
                .ToList();

            foreach (var enumProperty in enumProperties)
            {
                var enumPropertyValue = enumProperty.Value.Enum != null ? enumProperty.Value : enumProperty.Value.Items;

                System.Type enumType;                
                if (_useTypesAsInteger.ContainsKey(enumProperty.Key))
                {
                    // For pre-defined types we do not want to expose the string values but the integer values
                    enumType = _useTypesAsInteger[enumProperty.Key];

                    var enumeration = GetIntegerEnumAndDescription(enumType);

                    enumPropertyValue.Enum = enumeration.values;
                    enumPropertyValue.Type = "integer";
                    enumPropertyValue.Description += enumeration.description.TrimEnd(' ', ',');
                }
                else
                {
                    // for enumerations that we want to keep with string values we have to find the C# system type

                    // first we get all the enumeration values that are defined on the schema
                    var enumValues = enumPropertyValue.Enum.Select(e => $"{Swagger.ToUnderscoreCase(e.ToString())}").ToList();

                    // then we check if there is any C# enumeration defined that has the same enumeration values as the schema one
                    enumType = enums.FirstOrDefault(p =>
                    {
                        var enumNames = Enum.GetNames(p).Select(Swagger.ToUnderscoreCase).ToList();
                        if (enumNames.Except(enumValues, StringComparer.InvariantCultureIgnoreCase).Any())
                            return false;
                        if (enumValues.Except(enumNames, StringComparer.InvariantCultureIgnoreCase).Any())
                            return false;
                        return true;
                    });

                    // to fix the casing we apply the underscore cased values to the enum
                    enumPropertyValue.Enum = enumValues.Cast<object>().ToList();
                }

                if (enumType == null)
                    throw new Exception($"Property {enumProperty} not found in {context.SystemType.Name} Type.");

                var enumName = Swagger.ToUnderscoreCase(enumType.Name);

                // If it was not already done - add the definition now as a global definition
                if (context.SchemaRegistry.Definitions.ContainsKey(enumName) == false)
                    context.SchemaRegistry.Definitions.Add(enumName, enumPropertyValue);

                // adjust the inline definition to now contain only a reference and not the enum values anymore
                var schema = new Schema
                {
                    Ref = $"#/definitions/{enumName}"
                };
                if (enumProperty.Value.Enum != null)
                {
                    model.Properties[enumProperty.Key] = schema;
                }
                else if (enumProperty.Value.Items?.Enum != null)
                {
                    enumProperty.Value.Items = schema;
                }
            }
        }

        /// <summary>
        /// Create integer based enumeration output for e.g. AssetType in operation parameters
        /// Swagger does not allow "query" parameters to have $ref schemas so we have to keep them inline.
        /// </summary>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // for now we assume that enum type name == parameter name (this may not be correct in the future!)
            var enumerationParameters = operation.Parameters
                .Where(p => p is NonBodyParameter nonBody && nonBody.Enum?.Count > 0 && _useTypesAsInteger.ContainsKey(p.Name))
                .Select(p => p as NonBodyParameter);

            foreach(var enumerationParameter in enumerationParameters)
            {
                var enumeration = GetIntegerEnumAndDescription(_useTypesAsInteger[enumerationParameter.Name]);
                enumerationParameter.Enum = enumeration.values;
                enumerationParameter.Description += enumeration.description;
                enumerationParameter.Type = "integer";
            }
        }

        private static (List<object> values, string description) GetIntegerEnumAndDescription(Type enumType)
        {
            var integers = new List<object>();
            var description = string.Empty;
            foreach (var value in Enum.GetValues(enumType))
            {
                var index = Convert.ToInt32(Enum.Parse(enumType, value.ToString()));
                description += $" {index} = {value},";

                integers.Add(index);
            }

            return (integers, description);
        }
    }

    /// <summary>
    /// Special schema customizations
    /// </summary>
    public class CustomizeSchemas : ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            // Unique items is right now not correctly applied so we hide it
            schema.UniqueItems = null;

            AdjustAdditionalPropertiesObjects(schema, context);
        }

        private static void AdjustAdditionalPropertiesObjects(Schema schema, SchemaFilterContext context)
        {
            var properties = context.SystemType.GetProperties()
                .Where(prop => prop.PropertyType == typeof(Newtonsoft.Json.Linq.JRaw) || prop.PropertyType == typeof(Newtonsoft.Json.Linq.JObject));
            foreach (var property in properties)
            {
                var schemaProperty = schema.Properties[Swagger.ToUnderscoreCase(property.Name)];
                // For all JRaw/JObject we mark it as having additional properties
                schemaProperty.AdditionalProperties = new Schema
                {
                    Type = "string",
                };
            }
        }
    }

    /// <summary>
    /// Change array input parameters to CSV
    /// </summary>
    public class FixOperations : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            foreach(var parameter in operation.Parameters)
            {
                if (parameter is NonBodyParameter nonBodyParameter)
                {
                    if (nonBodyParameter.Type == "array")
                    {
                        nonBodyParameter.CollectionFormat = "csv";
                    }
                }
                else if (parameter.In == "body")
                {
                    parameter.Name = Swagger.ToUnderscoreCase(parameter.Name);
                }
            }
        }
    }
}