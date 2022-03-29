
//////using HookKey = HookKeys.HookKey;
//////HookKey.Main();


// ==================================================

//////using NAudio.Wave;
////////using MeltySynth;

// ==================================================

//////var player = new BardAudition.Audio.MidiSampleProvider("Assets/TimGM6mb.sf2");

//////using (var waveOut = new NAudio.Wave.WaveOut(NAudio.Wave.WaveCallbackInfo.FunctionCallback()))
//////{
//////    waveOut.Init(player);
//////    waveOut.Play();

//////    // Load the MIDI file.
//////    var midiFile = new MeltySynth.MidiFile(@"C:\Windows\Media\flourish.mid");

//////    // Play the MIDI file.
//////    player.Play(midiFile, true);

//////    // Wait until any key is pressed.
//////    Console.ReadKey();
//////}

// ==================================================

using MeltySynth;
using NAudio.Wave;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BardAudition;

public class Program
{

    // ============================================
    #region Main
    // ============================================

    // Add this if using AsioOut instead of WaveOut in NAudio: [STAThread]
    public static void Main()
    {
        //KeyHookSys.Init();
        //InputSys.Init();
        //AudioSys.Init();
        var audioCtrl = new AudioController();
        var noteCtrl = audioCtrl.GetNoteController();
        var activeNote = -1;
        
        foreach (var keyBinding in InputSys.GetKeyBindings()) {
            KeyHookSys.RegisterKeyDown(keyBinding.formsKey, _ => {
                activeNote = keyBinding.midiPitch;
                noteCtrl.NoteOn(keyBinding.midiPitch);
            });
            KeyHookSys.RegisterKeyUp(keyBinding.formsKey, _ => {
                if (activeNote == keyBinding.midiPitch) {
                    noteCtrl.NoteOffAll();
                }
            });
        }

        // Start a standard application method loop
        Application.Run();

        audioCtrl.Dispose();
    }

    #endregion

}

#region archive

// =============================================
// ASIO Method
// =============================================

//////var asioDriverNames = NAudio.Wave.AsioOut.GetDriverNames();
//////foreach (var driverName in asioDriverNames)
//////{
//////    Console.WriteLine(driverName);
//////}

//////var asio4AllDriverIdx = asioDriverNames.ToList().FindIndex(driverName => driverName.Contains("ASIO4ALL", StringComparison.CurrentCultureIgnoreCase));
//////if (asio4AllDriverIdx == -1)
//////{
//////    Console.WriteLine("Couldn't find Asio4All driver");
//////    return;
//////}
//////else
//////{
//////    Console.WriteLine($"Found Asio4All driver at index {asio4AllDriverIdx}");
//////}

//////var asioDriverName = asioDriverNames[asio4AllDriverIdx];
//////var asioOut = new NAudio.Wave.AsioOut(asioDriverName);
//////if (asioOut == null)
//////{
//////    Console.WriteLine($"Failed to init NAudio.Wave.AsioOut for driver named {asioDriverName}");
//////    return;
//////}
//////else
//////{
//////    Console.WriteLine($"Got ASIO driver named {asioDriverName}.");
//////}

//////var outputChannels = asioOut.DriverOutputChannelCount;
//////// optionally, change the starting channel for outputting audio:
////////asioOut.OutputChannelOffset = 2;
//////var sampleProvider = new MySampleProvider();
//////asioOut.Init(sampleProvider);

//////asioOut.Play(); // start playing
//////Console.ReadLine();
//////asioOut.Stop();

//////asioOut.Dispose();
//////sampleProvider.Dispose();

// =============================================

#endregion
