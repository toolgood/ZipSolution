using System;

namespace SoMain.Common.SharpCompress.Common
{
    public class ReaderExtractionEventArgs<T> : EventArgs
    {
        internal ReaderExtractionEventArgs(T entry)
        {
            Item = entry;
        }
        public T Item { get; private set; }
    }
}
