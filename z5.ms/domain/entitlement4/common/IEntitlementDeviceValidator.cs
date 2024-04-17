using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary>Common service to validate devices for all entitlement providers</summary>
    public interface IEntitlementDeviceValidator
    {
        /// <summary>
        /// Validate if the device is registered
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <param name="maxDeviceCount"></param>
        /// <returns></returns>
        Task<Result<Success>> ValidateDevice(Guid userId, string deviceId, int maxDeviceCount);
    }
}
