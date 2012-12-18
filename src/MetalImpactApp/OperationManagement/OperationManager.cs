using System.Collections.Generic;
using System.ComponentModel;
using MIProgram.Core;
using MIProgram.Core.Model;
using MIProgram.Core.ProductRepositories;

namespace MetalImpactApp.OperationManagement
{
    public abstract class OperationManager<T> : IOperationsLauncher where T : Product
    {
        protected readonly ReviewProcessor<T> ReviewsProcessor;
        private readonly IList<Operation> _operationsToProcess;
        private AsyncWorkerWrapper _asyncWorkerWrapper;
        protected abstract IDictionary<OperationType, IOperationProcessor<T>> OperationsDefinition { get; }
        public abstract ProductRepository<T> ProductRepository { get; }

        protected OperationManager(ReviewProcessor<T> reviewsProcessor, IList<Operation> operationsToProcess)
        {
            ReviewsProcessor = reviewsProcessor;
            _operationsToProcess = operationsToProcess;
        }

        public void EndWork()
        {
            _asyncWorkerWrapper.IsWorking = false;
            ReviewsProcessor.FinalizeWork();
        }

        public bool IsWorking
        {
            get { return (_asyncWorkerWrapper == null) ? false : _asyncWorkerWrapper.IsWorking; }
        }

        public string Infos
        {
            get { return (_asyncWorkerWrapper == null) ? string.Empty : _asyncWorkerWrapper.Infos; }
        }

        public long Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            _asyncWorkerWrapper = new AsyncWorkerWrapper(worker, e);

            //Explode, process & post process reviews
            ReviewsProcessor.Process(_asyncWorkerWrapper, ProductRepository);

            _asyncWorkerWrapper.BackgroudWorker.ReportProgress(0);
            foreach (var operation in _operationsToProcess)
            {
                var operationToPerform = OperationsDefinition[operation.OperationType];
                _asyncWorkerWrapper.Infos = operationToPerform.ProcessDescription;
                operationToPerform.Process(ProductRepository);
                _asyncWorkerWrapper.BackgroudWorker.ReportProgress((_operationsToProcess.IndexOf(operation) + 1) * 100 / _operationsToProcess.Count);
            }

            return ProductRepository.Products.Count;
        }
    }
}