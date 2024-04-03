using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticWithMutation : Genetic
{
    protected override List<Vector4> CreateNewGeneration()
    {
        List<Vector4> newGen = new();

        while (_winners.Count > 0)
        {
            Vector4 father = _winners.GetRandom();
            _winners.Remove(father);

            Vector4 mother = _winners.GetRandom();
            _winners.Remove(mother);

            newGen.Add(father);
            newGen.Add(mother);

            Vector4 child1 = Reproduce(father, mother);
            Vector4 child2 = Reproduce(father, mother);

            if (Random.value < 0.01) child1 = Mutation(child1);
            if (Random.value < 0.01) child2 = Mutation(child2);

            newGen.Add(child1);
            newGen.Add(child2);
        }

        return newGen;
    }

    private Vector4 Reproduce(Vector4 father, Vector4 mother)
    {
        Vector4 son = new Vector4(
            (Random.value < 0.5) ? father.x : mother.x,
            (Random.value < 0.5) ? father.y : mother.y,
            (Random.value < 0.5) ? father.z : mother.z,
            (Random.value < 0.5) ? father.w : mother.w);
        return son;
    }

    private Vector4 Modify(Vector4 father, int geneIndex)
    {
        if (geneIndex == 0) return new Vector4(father.x * Random.Range(0.9f, 1.1f), father.y, father.z, father.w);
        if (geneIndex == 1) return new Vector4(father.x, father.y * Random.Range(0.9f, 1.1f), father.z, father.w);
        if (geneIndex == 2) return new Vector4(father.x, father.y, father.z * Random.Range(0.9f, 1.1f), father.w);
        if (geneIndex == 3) return new Vector4(father.x, father.y, father.z, father.w * Random.Range(0.9f, 1.1f));
        Debug.LogWarning("n is not in range of Vector4");
        return default;
    }

    private Vector4 Mutation(Vector4 father)
    {
        Vector4 son = new Vector4();
        int nbGenes = Random.Range(1, 5);

        for (int i = 0; i < nbGenes; i++)
        {
            int geneIndex = Random.Range(0, 4);
            son = Modify(father, geneIndex);
        }
        return son;
    }
}
