﻿namespace MIProgram.Model
{
    public abstract class Product
    {
        public int Id { get; protected set; }
        public Artist Artist { get; protected set; }
    }
}