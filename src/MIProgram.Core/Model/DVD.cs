﻿using System;

namespace MIProgram.Core.Model
{
    public class DVD : Product
    {
        public DateTime ReleaseDate { get; set; }
        public Artist SharedArtist { get; set; }
        public string Title { get; set; }
    }
}