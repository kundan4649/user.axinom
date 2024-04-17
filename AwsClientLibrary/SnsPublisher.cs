using AwsClientLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using z5.ms.common.abstractions;

namespace z5.ms.common.AWS
{
    public class SnsPublisher<T> : IPublisher<T>
    {
        private readonly IAWSPublisher<T> _awsPublisher;
        public SnsPublisher(IAWSPublisher<T> awsPublisher)
        {
            _awsPublisher = awsPublisher;
        }

        public async Task<Result<Success>> Publish(T message)
        {
            await _awsPublisher.Publish(message);
            return Result<Success>.FromValue(new Success { Code = 0, Message = $"{typeof(T).Name} published" });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
