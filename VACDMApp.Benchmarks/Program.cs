//using BenchmarkDotNet.Attributes;
//using BenchmarkDotNet.Configs;
//using BenchmarkDotNet.Jobs;
//using BenchmarkDotNet.Running;
//using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
//using VACDMApp.Data;
//using VACDMApp.VACDMData;
//using VACDMApp.Data.Renderer;

//namespace VACDMApp.Benchmarks
//{
//    public class Program
//    {
//        public static void Main(string[] args) 
//        {
//            var config = DefaultConfig.Instance.AddJob(
//               Job.MediumRun.WithLaunchCount(1).WithToolchain(InProcessNoEmitToolchain.Instance)
//           );

//            _ = BenchmarkRunner.Run<Benchmarks>(config);
//        }
//    }

//    [MemoryDiagnoser]
//    public class Benchmarks
//    {
//        [Benchmark]
//        public async Task VatsimDataBenchmark() => await GetVatsimData.GetVatsimDataAsync();

//        [Benchmark]
//        public async Task VACDMDataBenchmark() => await VACDMPilotsData.GetVACDMPilotsAsync();

//        [Benchmark]
//        public async Task AirlinesBenchmark() => await AirlinesData.GetAirlinesAsync();

//        [Benchmark]
//        public async Task FlowMeasuresBenchmark() => await FlowMeasuresData.GetFlowMeasuresAsync();

//        [Benchmark]
//        public async Task DataSourcesBenchmark() => await VaccDataSources.GetDataSourcesAsync();

//        [Benchmark]
//        public async Task SettingsBenchmark() => await SettingsData.ReadSettingsAsync();

//        [Benchmark]
//        public void RenderPilotsBenchmark() => BenchmarkPilots.Render();
//    }
//}
