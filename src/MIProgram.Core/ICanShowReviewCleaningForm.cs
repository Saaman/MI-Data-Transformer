using MIProgram.Core.BodyCleaning;

namespace MIProgram.Core
{
    public interface ICanShowReviewCleaningForm
    {
        ReviewCleaningFormResult ShowReviewCleaningForm(RemovalsPresenter presenter);
    }
}
