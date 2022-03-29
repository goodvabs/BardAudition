using System;
using MeltySynth;
using NAudio.Wave;
//using MeltySynth;

namespace BardAudition.Audio;
public class MidiSampleProvider : ISampleProvider
{
    private static WaveFormat format = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    private MeltySynth.Synthesizer synthesizer;
    private MeltySynth.MidiFileSequencer sequencer;

    private object mutex;

    public MidiSampleProvider(string soundFontPath)
    {
        synthesizer = new MeltySynth.Synthesizer(soundFontPath, format.SampleRate);
        sequencer = new MeltySynth.MidiFileSequencer(synthesizer);

        mutex = new object();
    }

    public void Play(MeltySynth.MidiFile midiFile, bool loop)
    {
        lock (mutex)
        {
            sequencer.Play(midiFile, loop);
        }
    }

    public void Stop()
    {
        lock (mutex)
        {
            sequencer.Stop();
        }
    }

    public int Read(float[] buffer, int offset, int count)
    {
        lock (mutex)
        {
            // RenderInterleaved accessible via MeltySynth (extension method)
            sequencer.RenderInterleaved(buffer.AsSpan(offset, count));
        }

        return count;
    }

    public WaveFormat WaveFormat => format;
}