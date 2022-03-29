using MeltySynth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BardAudition;

public class NoteSynthesizer : NAudio.Wave.ISampleProvider, INoteController
{
    private static int SampleRate = 44100;
    private static int ChannelCount = 2;

    private MeltySynth.Synthesizer synth;

    public System.Diagnostics.Stopwatch? dbgWatch = null;
    public bool dbgWaitingForNoteOn = false;

    public NoteSynthesizer(string soundFontPath)
    {
        this.synth = new MeltySynth.Synthesizer(soundFontPath, SampleRate);
    }
    // =====================================
    #region Interface NAudio.Wave.ISampleProvider
    // =====================================

    public void NoteOn(int midiPitch)
    {
        synth.NoteOn(0, midiPitch, 100);
    }
    public void NoteOff(int midiPitch)
    {
        synth.NoteOff(0, midiPitch);
    }

    public void NoteOffAll()
    {
        synth.NoteOffAll(immediate: false);
    }

    #endregion
    // =====================================

    // =====================================
    #region Interface NAudio.Wave.ISampleProvider
    // =====================================

    public NAudio.Wave.WaveFormat WaveFormat => NAudio.Wave.WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, ChannelCount);

    public int Read(float[] buffer, int offset, int count)
    {
        // Debug latency measurement.
        if (dbgWaitingForNoteOn && dbgWatch != null)
        {
            dbgWatch.Stop();
            dbgWaitingForNoteOn = false;
            Console.Write($"Execution time (ms): {dbgWatch.ElapsedMilliseconds};");
            dbgWatch = null;
        }

        // RenderInterleaved accessible via MeltySynth (extension method)
        synth.RenderInterleaved(buffer.AsSpan(offset, count));
        return count;
    }

    #endregion
    // =====================================

}
