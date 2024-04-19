using static VacdmApp.Data.Data;

namespace VacdmApp.Data
{
    //TODO Singleton?
    internal static partial class DataHandler
    {
        private static CancellationTokenSource _cancellationTokenSource = new();

        private static bool _isInitialized = false;

        internal static readonly bool IsStopped = _isInitialized;

        internal static async Task RunAsync()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException();
            }

            _isInitialized = true;

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var dataTask = GetVatsimData.GetVatsimDataAsync();
                var vacdmTask = VacdmPilotsData.GetVacdmPilotsAsync();
                var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();

                var taskList = new List<Task>();

                var settings = Data.Settings!;

                if (settings.UpdateVatsimDataAutomatically)
                {
                    taskList.Add(dataTask);
                }

                if (settings.UpdateVacdmDataAutomatically)
                {
                    taskList.Add(vacdmTask);
                }

                if (settings.UpdateEcfmpDataAutomatically)
                {
                    taskList.Add(measuresTask);
                }

                var runTasks = Task.WhenAll(taskList);
                await runTasks;

                if (!runTasks.IsCompletedSuccessfully)
                {
                    _cancellationTokenSource.Cancel();

                    //TODO show Exception
                    throw runTasks.Exception;
                }

                VatsimPilots = dataTask.Result.pilots.ToList();
                VacdmPilots = vacdmTask.Result;
                FlowMeasures = measuresTask.Result.FlowMeasures;
                FlowMeasureFirs = measuresTask.Result.Firs;

                var interval = settings.UpdateInterval;
                await Task.Delay(TimeSpan.FromSeconds(interval));
            }
        }

        internal static void Cancel()
        {
            _isInitialized = false;
            _cancellationTokenSource.Cancel();

            //Reset the token
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        internal static async Task ResumeAsync()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException();
            }

            await RunAsync();
        }
    }
}