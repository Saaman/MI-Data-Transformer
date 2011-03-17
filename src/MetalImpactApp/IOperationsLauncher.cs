using System.ComponentModel;

namespace MetalImpactApp
{
    public interface IOperationsLauncher
    {
        long Process(BackgroundWorker worker, DoWorkEventArgs e);
        void EndWork();
        bool IsWorking { get; }
        string Infos { get; }
    }
}