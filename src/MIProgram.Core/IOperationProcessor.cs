using MIProgram.Core.ProductStores;
using MIProgram.Model;

namespace MIProgram.Core
{
    public interface IOperationProcessor<T> where T : Product
    {
        //void Process(BackgroundWorker worker, DoWorkEventArgs e, OperationsManager_Deprecated managerDeprecated);
        void Process(ProductRepository<T> productRepository);
        string ProcessDescription { get; }
    }
}