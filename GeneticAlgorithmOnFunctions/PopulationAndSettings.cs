using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithmOnFunctions
{
    public class PopulationAndSettings
    {
        public Population Population { get; set; }
        public string InstanceId { get; set; }
        public int GenerationId { get; set; }
        public GeneticAlgorithmParameters GeneticAlgorithmParameters { get; set; }
    }
}
