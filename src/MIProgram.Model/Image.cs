namespace MIProgram.Model
{
    public class Image
    {
        string ImagePath { get; set; }

        public Image(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
}