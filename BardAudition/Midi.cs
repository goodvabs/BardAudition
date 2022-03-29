using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BardAudition;

public static class Midi
{
    public static readonly Dictionary<string, int> MapMidiNoteStrToMidiValue = new Dictionary<string, int>()
    {
        {  "c3", 48+00 },
        { "c#3", 49+00 },
        {  "d3", 50+00 },
        { "d#3", 51+00 },
        {  "e3", 52+00 },

        {  "f3", 53+00 },
        { "f#3", 54+00 },
        {  "g3", 55+00 },
        { "g#3", 56+00 },
        {  "a3", 57+00 },
        { "a#3", 58+00 },
        {  "b3", 59+00 },


        {  "c4", 48+12 },
        { "c#4", 49+12 },
        {  "d4", 50+12 },
        { "d#4", 51+12 },
        {  "e4", 52+12 },

        {  "f4", 53+12 },
        { "f#4", 54+12 },
        {  "g4", 55+12 },
        { "g#4", 56+12 },
        {  "a4", 57+12 },
        { "a#4", 58+12 },
        {  "b4", 59+12 },


        {  "c5", 48+24 },
        { "c#5", 49+24 },
        {  "d5", 50+24 },
        { "d#5", 51+24 },
        {  "e5", 52+24 },

        {  "f5", 53+24 },
        { "f#5", 54+24 },
        {  "g5", 55+24 },
        { "g#5", 56+24 },
        {  "a5", 57+24 },
        { "a#5", 58+24 },
        {  "b5", 59+24 },


        {  "c6", 48+36 },
    };
}