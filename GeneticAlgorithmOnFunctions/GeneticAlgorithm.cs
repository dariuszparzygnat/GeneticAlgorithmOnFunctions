using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GeneticAlgorithmOnFunctions
{
    public static class GeneticAlgorithm
    {

        [FunctionName("GeneticAlgorithm")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var populationAndSettings = context.GetInput<PopulationAndSettings>();
            var population = populationAndSettings.Population;

            var tasks = new Task<double>[population.Individuals.Count];
            for (int i = 0; i < population.Individuals.Count; i++)
            {
                tasks[i] = context.CallActivityAsync<double>("EvaluatePopulation", population);
            }
            await Task.WhenAll(tasks);
            var evaluationResults = tasks.Select(e => e.Result).ToList();
            BestIndividual bestIndividual = await context.CallActivityAsync<BestIndividual>("GetBestIndividual", population, evaluationResults);
            populationAndSettings.GenerationId = await context.CallActivityAsync<int>("SaveBestResult", bestIndividual,
                populationAndSettings.InstanceId, populationAndSettings.GenerationId);

            population = await context.CallActivityAsync<Population>("Selection", population, evaluationResults);
            population = await context.CallActivityAsync<Population>("Crossover", population);
            populationAndSettings.Population = await context.CallActivityAsync<Population>("Mutation", population);
            context.ContinueAsNew(populationAndSettings);
        }

        [FunctionName("GeneticAlgorithm_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            TraceWriter log)
        {
            string jsonContent = await req.Content.ReadAsStringAsync();
            GeneticAlgorithmParameters geneticAlgorithmParameters = JsonConvert.DeserializeObject<GeneticAlgorithmParameters>(jsonContent);
            Population startPopulation = GenerateFirstPopulation(geneticAlgorithmParameters.IndividualsInPopulation);
            var populationAndSettings = new PopulationAndSettings()
            {
                GenerationId = 0,
                GeneticAlgorithmParameters = geneticAlgorithmParameters,
                InstanceId = Guid.NewGuid().ToString(),
                Population = startPopulation
            };
            string instanceId = await starter.StartNewAsync("GeneticAlgorithm", populationAndSettings);

            log.Info($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        private static Population GenerateFirstPopulation(int individualsInPopulation)
        {
            throw new NotImplementedException();
        }
    }
}