using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace ZoneStuff
{
    // Trying to handle side-effects better
    //public class FileObserver2
    //{
    //    public static IObservable<ReadOnlySequence<byte>> Create(string filePath)
    //    {
    //        var pipe = new Pipe();
    //        _ = FillPipeAsync(pipe.Writer, filePath);

    //        return ConsumePipeAsync(pipe.Reader, f);
    //    }
    //}


}
