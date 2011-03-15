using System.ComponentModel;

namespace MIProgram.Core
{
    public class AsyncWorkerWrapper
    {
        public BackgroundWorker BackgroudWorker { get; set; }
        public DoWorkEventArgs WorkEventArgs { get; set; }
        public string Infos { get; set; }
        public bool IsWorking { get; set; }

        public AsyncWorkerWrapper(BackgroundWorker worker, DoWorkEventArgs e)
        {
            BackgroudWorker = worker;
            WorkEventArgs = e;
            IsWorking = true;
        }
    }
}