using System.Text;
using MIProgram.Core.Extensions;
using MIProgram.Core.Helpers;

namespace MIProgram.Core.Model
{
    public class Review
    {
        public int Id { get; private set; }

        public string Body { get; private set; }
        public int Score { get; private set; }
        public int Hits { get; private set; }
        public Reviewer Reviewer { get; private set; }

        public Product Product { get; set; }

        private static readonly IDGenerator ReviewIdGenerator = new IDGenerator();
        
        public Review(string body, int reviewHits, int reviewScore, Reviewer reviewer)
        {
            Id = ReviewIdGenerator.NewID();
            Body = body;
            Score = reviewScore;
            Hits = reviewHits;
            Reviewer = reviewer;
        }

        public string ToYAMLInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("id: {0}", Id);
            sb.AppendLine();
            sb.AppendLine("model: review");
            sb.AppendLine("product_type: Album");
            sb.AppendFormat("product_id: {0}", Product.Id);
            sb.AppendLine();
            sb.AppendFormat("body: '{0}'", Body.GetSafeRails());
            sb.AppendLine();
            sb.AppendFormat("score: {0}", Score);
            sb.AppendLine();
            sb.AppendFormat("hits: {0}", Hits);
            sb.AppendLine();
            sb.AppendFormat("created_at: {0}",  Product.CreationDate);
            sb.AppendLine();
            sb.AppendFormat("updated_at: {0}", Product.CreationDate);
            sb.AppendLine();
            sb.AppendFormat("created_by: {0}", Reviewer.Id);
            sb.AppendLine();
            return sb.ToString();
        }
    }
}