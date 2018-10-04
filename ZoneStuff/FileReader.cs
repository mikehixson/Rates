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
    public class FileReader
    {
        public static async Task ReadAllLinesAsync(string filePath, Action<ReadOnlySequence<byte>> action)
        {
            //var path = @"C:\Users\Mike\Downloads\charts\format2.txt";

            var pipe = new Pipe();
            _ = FillPipeAsync(pipe.Writer, filePath);

            await ConsumePipeAsync(pipe.Reader, action);
        }

        public static async Task FillPipeAsync(PipeWriter writer, string filePath)
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

        public static async Task ConsumePipeAsync(PipeReader reader, Action<ReadOnlySequence<byte>> action)
        {
            while (true)
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
                    action(line);

                    // Skip past part of buffer already read. "buffer" now begins at the byte after the new line char
                    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                }

                reader.AdvanceTo(buffer.Start, buffer.End); //todo: i dont fully understand what the "examined" argument is
            }
        }        
    }
}
