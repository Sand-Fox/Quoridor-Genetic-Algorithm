using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticWithPonderation : Genetic
{
    private int winnerChance = 80;

    protected override List<Vector4> CreateNewGeneration()
    {
        List<Vector4> newGen = new();

        while (newGen.Count != _nbIndividuals)
        {
            float randomNumber1 = Random.Range(0, 100);
            float randomNumber2 = Random.Range(0, 100);

            Vector4 father;
            if (randomNumber1 < winnerChance)
            {
                father = _winners.GetRandom();
                _winners.Remove(father);
            }
            else
            {
                father = _loosers.GetRandom();
                _loosers.Remove(father);
            }

            Vector4 mother;
            if (randomNumber2 < winnerChance)
            {
                mother = _winners.GetRandom();
                _winners.Remove(mother);
            }
            else
            {
                mother = _loosers.GetRandom();
                _loosers.Remove(mother);
            }

            Vector4 child1 = Reproduce(father, mother);
            Vector4 child2 = Reproduce(father, mother);

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
}