using static VacdmApp.Data.Data;

namespace VacdmApp.Data
{
    internal partial class DataHandler
    {
        internal static CancellationTokenSource CancellationTokenSource = new();

        internal static async Task RunAsync()
        {
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                var dataTask = GetVatsimData.GetVatsimDataAsync();
                var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();
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

        internal static bool Cancel()
        {
            try
            {
                CancellationTokenSource.Cancel();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}