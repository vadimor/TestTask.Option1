﻿namespace TestTask.Option1.Data.Entities
{
    public class ExperimentValue
    {
        public int Id { get; set; }

        public string Value { get; set; } = null!;

        public int ExperimentId { get; set; }

        public Experiment Experiment { get; set; } = null!;
    }
}