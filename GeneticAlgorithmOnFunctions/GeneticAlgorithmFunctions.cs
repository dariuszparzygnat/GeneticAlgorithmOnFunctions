using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace GeneticAlgorithmOnFunctions
{
    public static class GeneticAlgorithmFunctions
    {
        [FunctionName("EvaluatePopulation")]
        public static double EvaluatePopulation([ActivityTrigger] Population population, TraceWriter log)
        {
            return 0;
        }

        [FunctionName("GetBestIndividual")]
        public static BestIndividual GetBestIndividual(Population population, IList<double> evaluationResults, TraceWriter log)
        {
            return null;
        }

        [FunctionName("SaveBestResult")]
        public static int SaveBestResult([ActivityTrigger] BestIndividual individual, string instanceId, int generationId, TraceWriter log)
        {

            return generationId++;
        }

        [FunctionName("Selection")]
        public static Population Selection(Population population, IList<double> evaluationResults, TraceWriter log)
        {
            return null;
        }

        [FunctionName("Crossover")]
        public static Population Crossover(Population population, TraceWriter log)
        {
            return null;
        }

        [FunctionName("Mutation")]
        public static Population Mutation(Population population, TraceWriter log)
        {
            return null;
        }
    }
}
