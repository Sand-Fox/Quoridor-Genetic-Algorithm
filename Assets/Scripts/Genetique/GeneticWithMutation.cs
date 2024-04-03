using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticWithMutation : Genetic
{
    [SerializeField, Range(0, 1)] private float _mutateProbability = 0.1f;

    protected override List<Vector4> CreateNewGeneration()
    {
        List<Vector4> newGen = new();

        while (_winners.Count > 0)
        {
            Vector4 father = _winners.GetRandom();
            Vector4 mother = _winners.GetRandom();

            _winners.Remove(father);
            _winners.Remove(mother);

            Vector4 child1 = Reproduce(father, mother);
            Vector4 child2 = Reproduce(father, mother);

            if (Random.value < _mutateProbability) child1 = Mutate(child1);
            if (Random.value < _mutateProbability) child2 = Mutate(child2);

            newGen.Add(father);
            newGen.Add(mother);
            newGen.Add(child1);
            newGen.Add(child2);
        }

        return newGen;
    }

    private Vector4 Reproduce(Vector4 father, Vector4 mother)
    {
        Vector4 son = new(
            (Random.value < 0.5) ? father.x : mother.x,
            (Random.value < 0.5) ? father.y : mother.y,
            (Random.value < 0.5) ? father.z : mother.z,
            (Random.value < 0.5) ? father.w : mother.w);
        return son;
    }

    private Vector4 Mutate(Vector4 original)
    {
        Vector4 son = original;
        int geneIndex = Random.Range(0, 4);
        son[geneIndex] = Random.value;
        return son;
    }
}
