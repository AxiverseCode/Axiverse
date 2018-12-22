using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace Axiverse.Interface.Audio
{
    public class AudioDevice
    {
        private XAudio2 xaudio2;
        private MasteringVoice masteringVoice;

        public AudioDevice()
        {
            xaudio2 = new XAudio2();
            masteringVoice = new MasteringVoice(xaudio2);

        }

        public void Dispose()
        {
            masteringVoice.Dispose();
            xaudio2.Dispose();
        }

        /// <summary>
        /// Play a sound file. Supported format are Wav(pcm+adpcm) and XWMA
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="text">Text to display</param>
        /// <param name="fileName">Name of the file.</param>
        static void PLaySoundFile(XAudio2 device, string text, string fileName)
        {
            Console.WriteLine("{0} => {1} (Press esc to skip)", text, fileName);
            var stream = new SoundStream(File.OpenRead(fileName));
            var waveFormat = stream.Format;
            var buffer = new AudioBuffer
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int)stream.Length,
                Flags = BufferFlags.EndOfStream
            };
            stream.Close();

            var sourceVoice = new SourceVoice(device, waveFormat, true);
            // Adds a sample callback to check that they are working on source voices
            sourceVoice.BufferEnd += (context) => Console.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
            sourceVoice.Start();

            int count = 0;
            while (sourceVoice.State.BuffersQueued > 0 && !IsKeyPressed(ConsoleKey.Escape))
            {
                if (count == 50)
                {
                    Console.Write(".");
                    Console.Out.Flush();
                    count = 0;
                }
                Thread.Sleep(10);
                count++;
            }
            Console.WriteLine();

            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();
            buffer.Stream.Dispose();
        }
        /// <summary>
        /// Determines whether the specified key is pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is pressed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }
    }

}
