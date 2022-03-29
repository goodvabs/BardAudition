using BardAudition.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BardAudition;

public interface INoteController
{
    void NoteOn(int midiPitch);
    void NoteOff(int midiPitch);
    void NoteOffAll();
}

public class AudioController : IDisposable
{
    private NoteSynthesizer noteSampleController;
    private NAudio.Wave.WaveOut waveOut;

    public AudioController()
    {
        var soundFontPath = "Assets/TimGM6mb.sf2";
        noteSampleController = new NoteSynthesizer(soundFontPath);

        waveOut = new NAudio.Wave.WaveOut(NAudio.Wave.WaveCallbackInfo.FunctionCallback());
        waveOut.DesiredLatency = 50;
        waveOut.Init(noteSampleController, convertTo16Bit: true);
        waveOut.Play();
    }

    public INoteController GetNoteController()
    {
        return noteSampleController;
    }

    public void Dispose()
    {
        waveOut.Stop();
        waveOut.Dispose();
    }
}
