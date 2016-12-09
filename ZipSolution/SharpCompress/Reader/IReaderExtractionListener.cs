using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoMain.Common.SharpCompress.Archive;
using SoMain.Common.SharpCompress.Common;

namespace SoMain.Common.SharpCompress.Reader
{
    internal  interface IReaderExtractionListener:IExtractionListener
    {
//        void EnsureEntriesLoaded();
        void FireEntryExtractionBegin(Entry entry);
        void FireEntryExtractionEnd(Entry entry);
    }
}
