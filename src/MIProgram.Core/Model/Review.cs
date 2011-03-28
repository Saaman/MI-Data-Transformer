namespace MIProgram.Core.Model
{
    public class Review
    {
        public string ReviewText { get; set; }
        public int ReviewNote { get; set; }
        public int ReviewHits { get; set; }

        public Product ReviewProduct { get; set; } 

        //créer le reviewer et faire son mapping + le tagger par ID
    }
}