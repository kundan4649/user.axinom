using System;

namespace z5.ms.common.helpers
{
    /// <summary>Bit stream representation of a byte array</summary>
    /// <remarks>Allows to specify number of bytes to be used for each added byte</remarks>
    public class BitStream
    {
        /// <summary>Current position in the stream</summary>
        public int BitPosition { get; set; }

        private readonly byte[] _buffer;

        /// <summary>Public constructor</summary>
        /// <param name="buffer">Buffer for reading or writing data</param>
        public BitStream(byte[] buffer)
        {
            _buffer = buffer;
        }

        /// <summary>Write single byte to current stream with specified number of bits</summary>
        /// <param name="value">A byte value to write</param>
        /// <param name="bitLength">Number of bits to be written</param>
        public void Write(byte value, byte bitLength)
        {
            var left = BitPosition / 8;

            if (left >= _buffer.Length)
                return;

            var leftVal = 0;
            var remainder = 8 - BitPosition + left * 8;
            if (remainder >= bitLength)
                leftVal = value << (remainder - bitLength);

            if (remainder < bitLength)
                leftVal = value >> (bitLength - remainder);

            _buffer[left] = (byte) (_buffer[left] | leftVal);
            if (remainder < bitLength)
                _buffer[left + 1] = (byte) (value << (8 - (bitLength - remainder)));

            BitPosition = BitPosition + bitLength;
        }

        /// <summary>Read single byte</summary>
        /// <param name="bitLength">Number of bits to be read</param>
        /// <returns>A byte containing value read from the bit stream. Returns 0 when the end of buffer is reached</returns>
        /// <remarks>Increases bit position as new bits are read</remarks>
        public byte Read(byte bitLength)
        {
            var left = BitPosition / 8;

            if (left >= _buffer.Length)
                return 0;

            var leftVal = 0;
            var remainder = 8 - BitPosition + (left * 8);
            if (remainder >= bitLength)
            {
                leftVal = _buffer[left] >> (remainder - bitLength);
                leftVal = leftVal & (0xFF >> (8 - bitLength));
            }

            if (remainder < bitLength)
            {
                leftVal = (_buffer[left] << (bitLength - remainder)) & (0xFF >> (8 - bitLength));
                var rightVal = (_buffer[left + 1] >> (8 - bitLength + remainder)) &
                               (0xFF >> (8 - bitLength + remainder));
                leftVal = leftVal | rightVal;
            }

            BitPosition = BitPosition + bitLength;
            return (byte) leftVal;
        }

        /// <summary>Write array of bytes</summary>
        /// <param name="value">source byte array</param>
        /// <param name="byteLength">Number of bytes to write</param>
        /// <param name="bitLength">Number of bits to be written from each byte in the inbound array</param>
        public void WriteArray(byte[] value, byte byteLength, byte bitLength)
        {
            //TODO: check for write attempt into a filled buffer

            //actual value byte array can be shorter, it will be placed towards the start of the buffer, and the rest bytes will be 0
            var buff = new byte[byteLength];
            Buffer.BlockCopy(value, 0, buff, 0, Math.Min(value.Length, byteLength));

            for (var i = 0; i < byteLength; i++)
                Write(buff[i], bitLength);
        }

        /// <summary>Read array of bytes</summary>
        /// <param name="byteLength">Number of bytes to read</param>
        /// <param name="bitLength">Number of bits to be read into each byte</param>
        /// <returns>A byte array containing values read from the bit stream. Returns 0 when the end of buffer is reached</returns>
        /// <remarks>Increases bit position as new bytes are read</remarks>
        public byte[] ReadArray(byte byteLength, byte bitLength)
        {
            var bytes = new byte[byteLength];
            var totalBitCount = _buffer.Length * bitLength;
            for (var i = 0; i < byteLength; i++)
            {
                if (BitPosition >= totalBitCount)
                    break;

                bytes[i] = Read(bitLength);
            }

            return bytes;
        }
    }
}