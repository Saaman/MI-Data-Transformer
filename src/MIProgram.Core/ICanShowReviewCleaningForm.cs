using MIProgram.Core.Cleaners;

namespace MIProgram.Core
{
    public interface ICanShowReviewCleaningForm
    {
        ReviewCleaningFormResult ShowReviewCleaningForm(RemovalsPresenter presenter);
    }
}
