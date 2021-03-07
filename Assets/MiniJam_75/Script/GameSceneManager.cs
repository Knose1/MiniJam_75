﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75
{
	public class GameSceneManager : MonoBehaviour
	{
		[SerializeField] public int titlecardSceneIndex = 1;
		[SerializeField] public int level1Index = 2;

		private static GameSceneManager instance;

		/// <summary>
		/// Unique instance 
		/// </summary>
		public static GameSceneManager Instance
		{
			get
			{
				return instance = instance ?? FindObjectOfType<GameSceneManager>();
			}
		}

		/// <summary>
		/// Load titlecard
		/// </summary>
		/// <param name="levelNumber">1 for "Level1", 2 for "Level2" etc...</param>
		public void LoadTitleCard()
		{
			SceneManager.LoadSceneAsync(titlecardSceneIndex);
		}

		/// <summary>
		/// Load a level
		/// </summary>
		/// <param name="levelNumber">1 for "Level1", 2 for "Level2" etc...</param>
		public void LoadLevel(int levelNumber)
		{
			SceneManager.LoadSceneAsync(level1Index + levelNumber - 1);
		}

		public void ReloadCurrentScene()
		{
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}

		private void Awake()
		{
			if (!instance) instance = this;

			DontDestroyOnLoad(gameObject);
		}

		private void OnDestroy()
		{
			instance = null;
		}
	}
}