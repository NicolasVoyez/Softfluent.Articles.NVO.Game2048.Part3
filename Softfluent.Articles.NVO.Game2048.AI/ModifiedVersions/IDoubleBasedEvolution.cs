namespace Softfluent.Articles.NVO.Game2048.AI.ModifiedVersions
{
    public interface IDoubleBasedEvolution
    {
        double CurrentEvolutionState { get;  }
        int EvolutionMaxRange { get;  }
        int EvolutionMinRange { get;  }
        void Evolve(double newEvolutionValue = 0);
    }
}
