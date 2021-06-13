using System;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class ArrayLocation
    {
        public Offset Offset { get; set; }
        public int Count { get; set; }

        public ArrayLocation(
            Offset offset,
            int count)
        {
            Offset = offset;
            Count = count;
        }

        public int GetIndex(int dataLength)
        {
            switch(Offset)
            {
                case Offset.Absolute:
                {
                    return Count;
                }
                case Offset.FromEnd:
                {
                    return dataLength - Count;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}