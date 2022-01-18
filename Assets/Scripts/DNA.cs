using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DNA
{
	public List<int> genes = new List<int>();
	public List<int> parent1Genes;
	public List<int> parent2Genes;

	public int maxGeneNumber = 360;
	public int geneQuantity = 3;

    public DNA(int maxGeneNumber,int geneQuantity)
	{
		this.maxGeneNumber = maxGeneNumber;
		this.geneQuantity = geneQuantity;
	}

	public void SetGeneValues()
	{
		genes.Clear();

		for (int i = 0; i < geneQuantity; i++)
		{
			genes.Add(Random.Range(0,maxGeneNumber + 1));
		}
	}

	public void CombineGenes(DNA parent1, DNA parent2)
	{
		parent1Genes = parent1.genes;
		parent2Genes = parent2.genes;

		for (int i = 0; i < maxGeneNumber; i++)
		{
			if (Random.Range(0,10) > 5)
			{
				genes[i] = parent1.genes[i];
			}
			else
			{
				genes[i] = parent2.genes[i];
			}
		}
	}

	public void Mutate()
	{
		genes[Random.Range(0,geneQuantity)] = Random.Range(0,maxGeneNumber);
	}	

	public int GetGene(int index)
	{
		return genes[index];
	}
}
