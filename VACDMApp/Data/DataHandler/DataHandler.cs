using static VacdmApp.Data.Data;

namespace VacdmApp.Data
{
    internal partial class DataHandler
    {
        internal static CancellationTokenSource CancellationTokenSource = new();

        private static bool _isInitialized = false;

        internal static async Task RunAsync()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException();
            }

            _isInitialized = true;

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                var dataTask = GetVatsimData.GetVatsimDataAsync();
                var vacdmTask = VacdmPilotsData.GetVacdmPilotsAsync();
                var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();

                var taskList = new List<Task>()
                {
                    dataTask,
                    vacdmTask,
                    measuresTask
                };

                var runTasks = Task.WhenAll(taskList);

                await runTasks;

                if (!runTasks.IsCompletedSuccessfully)
                {
                    CancellationTokenSource.Cancel();

                    //TODO show Exception
                    throw runTasks.Exception;
                }

                VatsimPilots = dataTask.Result.pilots.ToList();
                VacdmPilots = vacdmTask.Result;
                FlowMeasures = measuresTask.Result.FlowMeasures;
                FlowMeasureFirs = measuresTask.Result.Firs;

                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }

        internal static void Cancel()
        {
            _isInitialized = false;
            CancellationTokenSource.Cancel();

            //Reset the token
            CancellationTokenSource = new CancellationTokenSource();
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