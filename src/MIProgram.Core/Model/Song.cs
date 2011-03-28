using System;

namespace MIProgram.Core.Model
{
    public class Song
    {
        public int SongNumber { get; set; }
        public string SongTitle { get; set; }
        public TimeSpan SongDuration { get; set; }
    }
}