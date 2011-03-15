using System.Collections.Generic;
using MIProgram.Model;

namespace MIProgram.Core
{
    public class AlbumReviewBodyCleaner : ProductReviewBodyCleaner<Album>
    {
        private readonly IList<string> _textCleaningPatterns = new List<string> {
                                                                                    @"(<\s*a\s+[^>]+>([^<>]+)<\s*/a\s*>)",
                                                                                    @"(<\s*u\s*>([^<>]+)<\s*/u\s*>)",
                                                                                    @"(<\s*img\s*>([^<>]+)<\s*/img\s*>)",
                                                                                    @"(<\s*[^<>]\s*>\s*(discographie|tracklisting|distribution)[^<>]+<\s*/[^<>]\s*>)" };


        public AlbumReviewBodyCleaner(ICanShowReviewCleaningForm form)
            : base(form)
        { }

        protected override IList<string> TextCleaningPatterns
        {
            get { return _textCleaningPatterns; }
        }
    }
}