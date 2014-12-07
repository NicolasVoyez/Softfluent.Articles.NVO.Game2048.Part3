using Softfluent.Articles.NVO.Game2048.AI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softfluent.Articles.NVO.Game2048.AI.ModifiedVersions;

namespace Softfluent.Articles.NVO.DNASpecializer
{
    public class DNALab
    {
        private const int MAX_COEFFICIENT = 10;
        public int PopulationSize {get;set;} 
        private Random _rand = new Random();
        private Type[] _Types;
        private ObservableCollection<AIWrapper> _Population = new ObservableCollection<AIWrapper>();
        public ObservableCollection<AIWrapper> Population { get { return _Population; } }

        public event EventHandler<EventArgs<string>> EventOccurred;

        public DNALab(params Type[] IACLasses)
        {
            if (IACLasses.Length == 0)
                throw new ArgumentNullException("IACLasses");

            _Types = IACLasses;
        }

public void PopulateAIs(int populationSize = 100)
{
    PopulationSize = populationSize;

    RaiseEvent("Populating IA for DNA Lab");

    for (int i = 0; i < PopulationSize; i++)
    {
        List<AIRule> rules = new List<AIRule>();
        for (int j = 0; j < _Types.Length; j++)
        {
            if (RollDice(j % 5 + 1))
                rules.Add(CreateNewIAIRule(_Types[j], _rand.NextDouble() * MAX_COEFFICIENT));

        }
        if (rules.Count == 0)
            i--;
        else
            _Population.Add(new AIWrapper(new AI(rules)));

        RaiseEvent(i + 1 + " rules genes created");

    }
}

        private void RaiseEvent(string text, params object[] elements)
        {
            if (EventOccurred != null)
            {
                if (elements == null || elements.Length == 0)
                    EventOccurred(this, new EventArgs<string>(text));
                else
                    EventOccurred(this, new EventArgs<string>(String.Format(text,elements)));
            }
        }

private bool RollDice(int faces = 2)
{
    return _rand.Next(0, faces) % faces == 0;
}

        private List<AIWrapper> TournamentSelection(int roundSize = 10, int winners = 3)
        {
            List<AIWrapper> init = new List<AIWrapper>(_Population);
            List<AIWrapper> kept = new List<AIWrapper>();

            while (init.Count != 0)
            {
                List<AIWrapper> currentTournament = new List<AIWrapper>();
                for (int i = 0; i < roundSize; i++)
                {
                    var challenger = init[_rand.Next(0, init.Count)];
                    init.Remove(challenger);
                    currentTournament.Add(challenger);
                }
                kept.AddRange(currentTournament.OrderByDescending(x => x).Take(winners));
            }
            return kept;
        }

        private List<AIWrapper> CrossGenes(List<AIWrapper> toCross, int minSizeToLive = 1)
        {
            List<AIWrapper> init = new List<AIWrapper>(toCross);
            List<AIWrapper> children = new List<AIWrapper>();

            while (init.Count > 1)
            {
                var mother = init[_rand.Next(0, init.Count)];
                var motherRules = new List<AIRule>(mother.Intelligence.Rules);
                init.Remove(mother);
                var father = GetRandomFarthestGenes(motherRules, init);
                var fatherRules = new List<AIRule>(father.Intelligence.Rules);
                init.Remove(father);

                List<AIRule> childRules = new List<AIRule>();
                List<Type> childTypes = new List<Type>();
                CrossParent(motherRules, fatherRules, childRules, childTypes);
                CrossParent(fatherRules, motherRules, childRules, childTypes);

                if (childRules.Count < minSizeToLive) // not enough Genes to live on.
                    continue;

                children.Add(new AIWrapper(new AI(childRules)));
            }

            return children;
        }

        private AIWrapper GetRandomFarthestGenes(IEnumerable<AIRule> motherRules, List<AIWrapper> init, int randomBetweenMaxNumber = 5)
        {
            var distanceOrderedRules = init.OrderByDescending(x => DistanceFrom(motherRules, x.Intelligence.Rules));
            return
                distanceOrderedRules.ElementAt(
                _rand.Next(0, init.Count >= randomBetweenMaxNumber ? randomBetweenMaxNumber : init.Count));
        }

        private double DistanceFrom(IEnumerable<AIRule> motherRules, List<AIRule> fatherRules)
        {
            double distance = 0;
            foreach (var rule in motherRules)
            {
                double ruleDistance = 0;
                AIRule sameRule;
                if (TryGetRuleOfType(fatherRules, rule.GetType(), out sameRule))
                {
                    ruleDistance += Math.Abs(rule.Coefficient - sameRule.Coefficient);
                    IDoubleBasedEvolution evolvingRule = rule as IDoubleBasedEvolution;
                    if (evolvingRule != null)
                    {
                        var coeff = Math.Abs(evolvingRule.CurrentEvolutionState -
                                             ((IDoubleBasedEvolution)sameRule).CurrentEvolutionState);
                        if (coeff > double.Epsilon && ruleDistance < double.Epsilon)
                            ruleDistance = 1;

                        ruleDistance *= coeff;
                    }
                }
                else
                    ruleDistance += 5;

                distance += ruleDistance;
            }
            return distance;
        }

        private bool TryGetRuleOfType(IEnumerable<AIRule> rules, Type type, out AIRule rule)
        {
            rule = rules.FirstOrDefault(x => x.GetType() == type);
            return (rule != null);

        }

        private void CrossParent(List<AIRule> motherRules, List<AIRule> fatherRules, List<AIRule> childRules, List<Type> childTypes)
        {
            for (int i = 0; i < motherRules.Count; i++)
            {
                if (RollDice()) continue;

                var rule = motherRules[i];
                var ruleType = rule.GetType();
                if (childTypes.Contains(ruleType))
                    continue;

                var fatherSameRule = fatherRules.FirstOrDefault(x => x.GetType() == ruleType);
                if (fatherSameRule != null)
                    childRules.Add(CrossCommonGene(rule, fatherSameRule, ruleType));
                else
                    childRules.Add(rule);

                childTypes.Add(ruleType);
            }
        }

        private AIRule CrossCommonGene(AIRule rule, AIRule rule2, Type ruleType)
        {
            var avgCoeff = (rule.Coefficient + rule2.Coefficient) / 2;
            var diffCoeff = Math.Abs(rule.Coefficient - rule2.Coefficient);
            var testCoeff = avgCoeff + (_rand.NextDouble() - 0.5) * diffCoeff;

            double evolutionCoeff = 0;
            double avgECoeff = 0;
            #region Double evolution management Evolution

            IDoubleBasedEvolution optimizable = rule as IDoubleBasedEvolution;
            IDoubleBasedEvolution optimizable2 = rule2 as IDoubleBasedEvolution;
            if (optimizable != null && optimizable2 != null)
            {
                avgECoeff = (optimizable.CurrentEvolutionState + optimizable2.CurrentEvolutionState) / 2;
                var diffECoeff = Math.Abs(optimizable.CurrentEvolutionState - optimizable2.CurrentEvolutionState);
                evolutionCoeff = avgCoeff + (_rand.NextDouble() - 0.5) * diffCoeff;
            }
            #endregion

            return CreateNewIAIRule(ruleType, testCoeff > 0 ? testCoeff : avgCoeff, evolutionCoeff > 0 ? evolutionCoeff : avgECoeff);
        }

        private AIWrapper Mutate(AIWrapper ruleMutating, int mutations = 3, int minSizeToLive = 1)
        {
            List<AIRule> mutatedRules = new List<AIRule>(ruleMutating.Intelligence.Rules.Select(x => CreateNewIAIRule(x.GetType(), x.Coefficient)));
            for (int i = 0; i < mutations; i++)
            {
                Type geneType = _Types[_rand.Next(0, _Types.Length)];
                var existingGene = mutatedRules.FirstOrDefault(x => x.GetType() == geneType);
                if (existingGene != null)
                {
                    if (mutatedRules.Count > minSizeToLive && RollDice())
                        mutatedRules.Remove(existingGene);
                    else
                    {
                        existingGene.Coefficient = _rand.NextDouble()*MAX_COEFFICIENT;

                        TryEvolve(existingGene);
                    }
                }
                else
                    mutatedRules.Add(CreateNewIAIRule(geneType, _rand.NextDouble() * MAX_COEFFICIENT));
            }
            return new AIWrapper(new AI(mutatedRules));

        }

        private static void TryEvolve(AIRule existingGene, double optimizationDouble = 0.0d)
        {
            IDoubleBasedEvolution optimizable = existingGene as IDoubleBasedEvolution;
            if (optimizable != null)
                optimizable.Evolve(optimizationDouble);
        }

        private AIRule CreateNewIAIRule(Type ruleType, double coeff, double optimizationDouble = 0.0d)
        {
            if (ruleType.GetCustomAttributes(true).Any(x => x is NotOptimizableAttribute))
                coeff = 1;
            var rule = (AIRule)ruleType.GetConstructor(new Type[] { typeof(double) }).Invoke(new object[] { coeff });

            TryEvolve(rule,optimizationDouble);

            return rule;
        }

        public void Live(Action<IEnumerable<AIWrapper>> onGenerationEnd, Predicate<AIWrapper> endCondition, double mutationOccurence = 0.003d)
        {
            RaiseEvent("DNA samples start living");

            AIWrapper bestWrapper = null;
            do
            {
                var ToReproduce = TournamentSelection();

                RaiseEvent("Tournament ended, {0} genes selected", ToReproduce.Count);

                var children = CrossGenes(ToReproduce);

                List<AIWrapper> mutated = new List<AIWrapper>();
                foreach (var element in _Population)
                {
                    if (_rand.NextDouble() < mutationOccurence)
                        mutated.Add(Mutate(element));
                }
                RaiseEvent("Mutation ended, {0} genes selected", mutated.Count);

                var orderedPopulation = new List<AIWrapper>((from element in _Population.Union(children).Union(mutated)
                                                             orderby element descending
                                                             select element).Take(PopulationSize));

                _Population.Clear();
                foreach (var element in orderedPopulation)
                    _Population.Add(element);

                bestWrapper = _Population.First();

                RaiseEvent("Population resynchronized, best gene was " + bestWrapper);

                // avoid exceptionnal lucky results in best scores.
                if (bestWrapper != null)
                    bestWrapper.GenerateScore();

                RaiseEvent("Best gene score recalculated, now he is " + bestWrapper);

                if (onGenerationEnd != null)
                    onGenerationEnd(_Population);
            }
            while (!endCondition(bestWrapper) && !_stop);
            _stop = false;

            RaiseEvent("DNA samples pause their lives");
        }

        private bool _stop = false;

        public void Stop()
        {
            if (!_stop)
                _stop = true;
        }
    }
}
