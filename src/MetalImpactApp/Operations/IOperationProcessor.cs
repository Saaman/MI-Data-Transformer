using System.ComponentModel;

namespace MetalImpactApp.Operations
{
    public interface IOperationProcessor
    {
        void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated);
    }
}
