using System;

namespace MIProgram.Core.Model
{
    public abstract class Product
    {
        public int Id { get; protected set; }
        public DateTime CreationDate { get; set; }
        public Artist Artist { get; protected set; }
        public Reviewer Reviewer { get; protected set; }
        public string Title { get; protected set; }
    }
}