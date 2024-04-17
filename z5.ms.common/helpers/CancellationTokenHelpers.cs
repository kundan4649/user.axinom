using System;
using System.Threading;

namespace z5.ms.common.helpers
{
    /// <summary>Helpers for creating / extending cancellation tokens</summary>
    public static class CancellationTokenHelpers
    {
        /// <summary>Create a cancellation token that times out at a set time</summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static CancellationToken Timeout(DateTime dateTime) => new CancellationTokenSource(dateTime - DateTime.UtcNow).Token;

        /// <summary>Extend an existing cancellation token with a timeout</summary>
        /// <param name="parent"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static CancellationToken WithTimeout(this CancellationToken parent, TimeSpan timeSpan)
        {
            var src = CancellationTokenSource.CreateLinkedTokenSource(parent);
            src.CancelAfter(timeSpan);
            return src.Token;
        }

        /// <summary>Extend an existing cancellation token with a timeout</summary>
        /// <param name="parent"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static CancellationToken WithTimeout(this CancellationToken parent, DateTime dateTime)
        {
            var src = CancellationTokenSource.CreateLinkedTokenSource(parent);
            src.CancelAfter(dateTime - DateTime.UtcNow);
            return src.Token;
        }
    }
}
