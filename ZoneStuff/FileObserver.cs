using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneStuff
{ 
    // This came about because there is no way to do async yield return. So if I want to process a file asynchronously, but not accumulate all the lines in memory
    // then this is one way to do it.

    public class FileObserver
    {
        public static IObservable<T> AllLines<T>(string filePath, Func<ReadOnlySequence<byte>, T> f)
        {
            var pipe = new Pipe();
            _ = FillPipeAsync(pipe.Writer, filePath);

            return ConsumePipeAsync(pipe.Reader, f);
        }

        private static async Task FillPipeAsync(PipeWriter writer, string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1, true))
            {
                while (true)
                {
                    var memory = writer.GetMemory();

                    var read = await file.ReadAsync(memory);

                    if (read == 0)
                        break;

                    writer.Advance(read);

                    await writer.FlushAsync();
                }
            }

            writer.Complete();
        }


        private static IObservable<T> ConsumePipeAsync<T>(PipeReader reader, Func<ReadOnlySequence<byte>, T> f)
        {   
            

            return Observable.Create<T>(async (observer, cancelToken) =>
            {
                try
                {
                    while (true && !cancelToken.IsCancellationRequested)
                    {
                        ReadResult result = await reader.ReadAsync();
                        var buffer = result.Buffer;

                        if (buffer.IsEmpty && result.IsCompleted)
                            break; 

                        SequencePosition? position = null;

                        while ((position = buffer.PositionOf((byte)'\n')) != null)
                        {
                            var line = buffer.Slice(0, position.Value);
                            line = line.Slice(0, line.Length - 1);

                            // Process the line
                            observer.OnNext(f(line));

                            // Skip past part of buffer already read. "buffer" now begins at the byte after the new line char
                            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                        }

                        reader.AdvanceTo(buffer.Start, buffer.End); //todo: i dont fully understand what the "examined" argument is
                    }

                    observer.OnCompleted();
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }
                finally
                {
                    // Mark the PipeReader as complete
                    //TODO: reader.Complete();
                }
            });
        }
    }
}
