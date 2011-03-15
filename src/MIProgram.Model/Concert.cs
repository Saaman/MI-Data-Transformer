using System;
using System.Collections.Generic;

namespace MIProgram.Model
{
    public class Concert : Product
    {
        public DateTime Date { get; set; }
        public IList<Artist> SharedArtists { get; set; }
        public string Place { get; set; }
        public string Duration { get; set; }
    }
}
