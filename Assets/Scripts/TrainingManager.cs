using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainingManager : MonoBehaviour
{
	public GameObject prefab;
	public Vector3 startingPosition;
	public int populationSize;
	public int generation;
	public float trainingTime;
	public float elapsedTime;
	public List<BaseEnemy> population = new List<BaseEnemy>();
		   
    void Start()
    {
		for (int i = 0; i < populationSize; i++)
		{
			print(i);
			BaseEnemy e = Instantiate(prefab,startingPosition,Quaternion.identity).GetComponent<BaseEnemy>();
			e.brain.Initialize(e);
			population.Add(e);
		}
    }

    
    void Update()
    {
        elapsedTime += Time.deltaTime;
		if (elapsedTime > trainingTime)
		{
			BreedPopulation();
			elapsedTime = 0;
		}
    }

	BaseEnemy Breed(BaseEnemy parent1,BaseEnemy parent2)
	{
		BaseEnemy offspring = Instantiate(prefab,startingPosition,Quaternion.identity).GetComponent<BaseEnemy>();
		offspring.brain.Initialize(offspring);
		offspring.brain.dna.CombineGenes(parent1.brain.dna,parent2.brain.dna);
		return offspring;
	}

	void BreedPopulation()
	{
		List<BaseEnemy> sortedList = population.OrderBy(x => x.brain.timeAlive).ToList();
		population.Clear();

		for (int i = sortedList.Count - 1; i > ((float)sortedList.Count * 0.7f) -1;i--)
		{
			population.Add(Breed(sortedList[i],sortedList[i-1]));
			population.Add(Breed(sortedList[i-1],sortedList[i]));
			population.Add(Breed(sortedList[i],sortedList[i-1]));
		}

		foreach (BaseEnemy enemy in sortedList)
		{
			Destroy(enemy.gameObject);
		}
		generation++;
	}

}
