using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private List<SpawnDifficulty> difficulties = new List<SpawnDifficulty>();
		[SerializeField] private int currentDifficultyIndex = 0;

		private SpawnDifficulty currentDifficulty;

		// Start is called before the first frame update
		void Start()
		{
			currentDifficulty = difficulties[currentDifficultyIndex];
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}

	public class SpawnDifficulty : ScriptableObject
	{
		[SerializeField] private List<GameObject> patern = new List<GameObject>();
		[SerializeField] private int _paternsCountBeforeNextDifficulty = 5;
		public int PaternsCountBeforeNextDifficulty => _paternsCountBeforeNextDifficulty;

		private GameObject lastPatern = null;
		private List<GameObject> randomPaternList;
		
		public void Init()
		{
			randomPaternList = new List<GameObject>(patern);
		}

		public GameObject GetRandomPatern()
		{
			int index = Random.Range(0, randomPaternList.Count);
			GameObject toReturn = randomPaternList[index];

			randomPaternList.RemoveAt(index);

			//If lastPatern != null
			if (lastPatern)
				randomPaternList.Add(lastPatern);

			lastPatern = toReturn;

			return Instantiate(toReturn);
		}
	}
}
