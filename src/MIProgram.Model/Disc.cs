using System.Collections.Generic;

namespace MIProgram.Model
{
    public class Disc
    {
        public int DiscNumber { get; set; }
        public IList<Song> DiscSongs { get; set; }
    }
}