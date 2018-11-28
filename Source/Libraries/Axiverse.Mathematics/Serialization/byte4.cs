namespace Axiverse.Mathematics.Serialization
{
    /// <summary>
    /// 4d vector of bytes.
    /// </summary>
    public struct byte4
    {
        byte X;
        byte Y;
        byte Z;
        byte W;

        /// <summary>
        /// Constructs a byte vector.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <param name="W"></param>
        public byte4(byte X = 0, byte Y = 0, byte Z = 0, byte W = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }
    }
}
