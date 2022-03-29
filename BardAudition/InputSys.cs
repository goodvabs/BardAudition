using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BardAudition;

public static class InputSys
{
    public static Dictionary<string, string> mapNoteToKey = new Dictionary<string, string>();
    public static Dictionary<string, string> mapKeyToNote = new Dictionary<string, string>();

    static InputSys()
    {
        // ==================================================
        // Map: Note Strings to Keyboard Keys and vice-versa.
        // ==================================================

        // Load the performance_keybinds.json file.
        var keybindsPath = "Assets/performance_keybinds.json";
        var keybindsJsonStr = System.IO.File.ReadAllText(keybindsPath);
        var jsonReader = new Newtonsoft.Json.JsonTextReader(new StringReader(keybindsJsonStr));
        Console.WriteLine("Reading keybinds json!");
        string? propName = null;
        string? strVal = null;
        while (jsonReader.Read())
        {
            var type = jsonReader.TokenType;
            var val = jsonReader.Value;
            if (val == null) continue;

            switch (type)
            {
                case Newtonsoft.Json.JsonToken.PropertyName:
                    propName = (string)val;
                    break;
                case Newtonsoft.Json.JsonToken.String:
                    strVal = (string)val;
                    break;
                default: throw new ArgumentException($"Unexpected {type}");
            }

            if (propName != null && strVal != null)
            {
                var validProp = true;

                // Ignore "comment" json key entries.
                if (propName.StartsWith("//")) { validProp = false; }

                if (validProp)
                {
                    var note = propName;
                    var key = strVal;
                    mapNoteToKey[note] = key;
                    mapKeyToNote[key] = note;
                    Console.Write($"Parsed: {propName}: {strVal}; ");
                }

                propName = strVal = null;
            }
        }
        Console.WriteLine("Done reading json. =========");
    }

    public struct KeyBinding
    {
        public string kbKey;
        public string formsKey;
        public int midiPitch;
    }

    public static List<KeyBinding> GetKeyBindings()
    {
        var keyBindings = new List<KeyBinding>();

        foreach (var noteKeyPair in mapNoteToKey)
        {
            var noteStr = noteKeyPair.Key;
            var kbKey = noteKeyPair.Value;

            var midiPitch = (int?)null;
            if (!Midi.MapMidiNoteStrToMidiValue.TryGetValue(noteStr, out var midiValue))
            {
                throw new Exception($"Unhandled note str {noteStr}, tried to get from MapMidiNoteStrToMidiValue");
            } else
            {
                midiPitch = midiValue;
            }

            var formsKey = (string?)null;
            if (!MapKeyStringToWinFormsKeyString.TryGetValue(kbKey, out var outFormsKey))
            {
                throw new Exception($"No Windows Forms key name known for key string {kbKey}");
            } else
            {
                formsKey = outFormsKey;
            }

            keyBindings.Add(new KeyBinding { kbKey = kbKey, formsKey = formsKey, midiPitch = midiPitch.Value });
        }

        return keyBindings;
    }

    public static readonly Dictionary<string, string> MapKeyStringToWinFormsKeyString = new Dictionary<string, string>()
    {
        {  "1", "D1" },
        {  "2", "D2" },
        {  "3", "D3" },
        {  "4", "D4" },
        {  "5", "D5" },
        {  "6", "D6" },
        {  "7", "D7" },
        {  "8", "D8" },
        {  "9", "D9" },
        {  "0", "D0" },
        {  "-", "OemMinus" },
        {  "=", "Oemplus" },

        {  "q", "Q" },
        {  "w", "W" },
        {  "e", "E" },
        {  "r", "R" },
        {  "t", "T" },
        {  "y", "Y" },
        {  "u", "U" },
        {  "i", "I" },
        {  "o", "O" },
        {  "p", "P" },
        {  "[", "OemOpenBrackets" },
        {  "]", "Oem6" },
        {  @"\", "Oem5" },

        {  "a", "A" },
        {  "s", "S" },
        {  "d", "D" },
        {  "f", "F" },
        {  "g", "G" },
        {  "h", "H" },
        {  "j", "J" },
        {  "k", "K" },
        {  "l", "L" },
        {  ";", "Oem1" },
        {  "'", "Oem7" },

        {  "z", "Z" },
        {  "x", "X" },
        {  "c", "C" },
        {  "v", "V" },
        {  "b", "B" },
        {  "n", "N" },
        {  "m", "M" },
        {  ",", "Oemcomma" },
        {  ".", "OemPeriod" },
        {  "/", "OemQuestion" }
    };
}
