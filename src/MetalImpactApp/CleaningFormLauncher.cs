using System.Windows.Forms;
using MIProgram.Core;
using MIProgram.Core.BodyCleaning;

namespace MetalImpactApp
{
    public class CleaningFormLauncher : ICanShowReviewCleaningForm
    {
        public ReviewCleaningFormResult ShowReviewCleaningForm(RemovalsPresenter presenter)
        {
            var res = new ReviewCleaningFormResult(presenter.ReviewId);

            var form = new ReviewCleaningForm(ref res, presenter);
            var dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return null;
            }

            return res;
        }
    }
}