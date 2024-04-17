using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace z5.ms.common.infrastructure.assetcache
{

    /// <summary>S3 storage access logic.</summary>
    /// <remarks>Specific to asset loading</remarks>
    public static class AWSStorage
    {      

        /// <summary>Build paths from path segments.</summary>
        /// <param name="paths">Path segments to join</param>
        /// <returns>Complete path</returns>
        public static string CombinePath(params string[] paths) 
            => paths.Aggregate(string.Empty, (cur, path) => cur + $"{path.Trim('/')}/").Trim('/');

        /// <summary>Download a file.</summary>
        /// <param name="options">AWS storage connection details</param>
        /// <param name="bucketName">Storage bucket name</param>
        /// <param name="path">file path</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Returns an open stream to the bucket object or null in case of failure</returns>
        public static async Task<Stream> GetFile(IAmazonS3 amazonS3Client, string bucketName, string path, ILogger logger = null)
        {
            try
            {
                GetObjectRequest objectRequest = new GetObjectRequest() { BucketName = bucketName, Key = path};
                var response = await amazonS3Client.GetObjectAsync(objectRequest);
                return response.ResponseStream;                
            }
            catch (Exception ex)
            {
                logger?.LogError($"Failed to load file from path : {path} Error: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>Return the basename of a path.</summary>
        private static string GetBasename(string path)
        {
            return path.Substring(path.LastIndexOf('/') + 1);
        }
        
        /// <summary>Return the dirname of a path.</summary>
        /// <remarks>Returns an empty string in case of a name with no path separators</remarks>
        private static string GetDirname(string path)
        {
            var i = path.LastIndexOf('/');
            return i > 0 ? path.Substring(0, i) : "";
        }
        
        /// <summary>Remove the container name from the relative URI.</summary>
        private static string StripContainerName(string path, string container) 
            => path.Substring(path.LastIndexOf($"{container}/", StringComparison.Ordinal) + $"{container}/".Length);        
      
    }

  
}