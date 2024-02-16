using Assets.Common.Utils;
using System;
using System.IO;

namespace Ivankarez.AIFR.TrainingAlgorithm
{
    public class Individual
    {
        public long Id { get; }
        public float[] EmbeddingWeights { get; }
        public float[] DrivingNetworkWeights { get; }
        public long Generation { get; }
        public DateTime CreatedAt { get; }

        public float? Fitness { get; set; } = null;
        public float? TimeAlive { get; set; } = null;

        private Individual(long id, float[] embeddingWeights, float[] drivingNetworkWeights, long generation, DateTime createdAt, float? fitness, float? timeAlive)
        {
            Id = id;
            EmbeddingWeights = embeddingWeights;
            DrivingNetworkWeights = drivingNetworkWeights;
            Generation = generation;
            CreatedAt = createdAt;
            Fitness = fitness;
            TimeAlive = timeAlive;
        }

        public Individual(long id, float[] embeddingWeights, float[] drivingNetworkWeights, long generation, DateTime createdAt) : this(id, embeddingWeights, drivingNetworkWeights, generation, createdAt, null, null)
        {
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(EmbeddingWeights);
            writer.Write(DrivingNetworkWeights);
            writer.Write(Generation);
            writer.Write(CreatedAt.ToBinary());
            writer.Write(Fitness);
            writer.Write(TimeAlive);
        }

        public static Individual Read(BinaryReader reader)
        {
            var id = reader.ReadInt64();
            var embeddingWeights = reader.ReadFloatArray();
            var drivingNetworkWeights = reader.ReadFloatArray();
            var generation = reader.ReadInt64();
            var createdAt = DateTime.FromBinary(reader.ReadInt64());
            var fitness = reader.ReadNullableFloat();
            var timeAlive = reader.ReadNullableFloat();

            return new Individual(id, embeddingWeights, drivingNetworkWeights, generation, createdAt, fitness, timeAlive);
        }
    }
}
