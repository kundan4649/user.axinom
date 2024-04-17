using System;
using System.Globalization;
using System.IO;

namespace z5.ms.common.helpers
{
    /// <summary>Enables creating a unique component Id that persists system restarts</summary>
    public static class ComponentIdHelper
    {
        private const int MaxLength = 14;

        private static string GetMachineName()
        {
            var name = Environment.MachineName.ToLower(CultureInfo.InvariantCulture);
            if (name.Length > MaxLength)
                name = name.Substring(0, MaxLength); 
            return name;
        }

        private static string GenerateComponentId() =>
            $"{GetMachineName()}_{Guid.NewGuid():N}";

        private const string ComponentIdFile = "UniqueComponentId";
        
        /// <summary>Fetch this component's UID, if it doesn't exist, create a new one and store it. </summary>
        public static string FetchOrCreateUniqueComponentId ()
        {
            if (!File.Exists(ComponentIdFile))
            {
                var componentId = GenerateComponentId();
                
                File.AppendAllText(ComponentIdFile, componentId);
            }

            return File.ReadAllText(ComponentIdFile);
        }
    }
}