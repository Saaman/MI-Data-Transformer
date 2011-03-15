using System.Collections.Generic;
using System.ComponentModel;
using MIProgram.Core;
using MIProgram.Core.ProductStores;
using MIProgram.Model;

namespace MetalImpactApp
{
    public abstract class IOperationManager<T> where T:Product
    {
        public bool IsWorking { get; set; }
        public string Infos { get; set; }

        protected readonly ReviewProcessor<T> _reviewsProcessor;
        private readonly IList<Operation> _operationsToProcess;
        protected abstract IDictionary<OperationType, IOperationProcessor<T>> OperationsDefinition { get; }
        public abstract IProductRepository<T> ProductRepository { get; }

        protected IOperationManager(ReviewProcessor<T> reviewsProcessor, IList<Operation> operationsToProcess)
        {
            _reviewsProcessor = reviewsProcessor;
            _operationsToProcess = operationsToProcess;
        }

        public void EndWork()
        {
            IsWorking = false;
            _reviewsProcessor.FinalizeWork();
        }

        public long Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            var asyncWorkerWrapper = new AsyncWorkerWrapper(worker, e);

            //Explode, process & post process reviews
            _reviewsProcessor.Process(asyncWorkerWrapper, ProductRepository);
            /*
            _reviewsExtractorProcessor.Process(worker, e, this);
            */
            foreach (var operation in _operationsToProcess)
            {
                OperationsDefinition[operation.OperationType].Process(asyncWorkerWrapper, ProductRepository);
            }

            return ProductRepository.Products.Count;
        }
    }
}