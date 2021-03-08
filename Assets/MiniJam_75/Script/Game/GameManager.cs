using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.GitHub.Knose1.Common.AnimationUtils;
using Com.GitHub.Knose1.MiniJam75.UI;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager instance;

		/// <summary>
		/// Unique instance 
		/// </summary>
		public static GameManager Instance
		{
			get
			{
				return instance = instance ?? FindObjectOfType<GameManager>();
			}
		}

		[SerializeField] private PlayerPhysic playerPhysic = null;

		// Start is called before the first frame update
		void Awake()
		{
			if (!instance) instance = this;

			playerPhysic.OnEnd += PlayerPhysic_OnEnd;
			AllowPlay();
		}

		public void AllowPlay() => Time.timeScale = PlayerSettings.Instance.timeScale;
		public void DisablePlay() => Time.timeScale = 0;

		private void PlayerPhysic_OnEnd(bool isWin)
		{
			if (!isWin && PlayerSettings.Instance.autoRestart)
			{
				OnRestart();
				return;
			}

			playerPhysic.OnEnd -= PlayerPhysic_OnEnd;

			Time.timeScale = 0;

			GameEnd gameEnd = FindObjectOfType<GameEnd>();
			gameEnd.OnRestart += GameEnd_OnRestart;
			StartCoroutine(gameEnd.Show(isWin));

		}

		private void GameEnd_OnRestart(GameEnd gameEnd)
		{
			gameEnd.OnRestart -= GameEnd_OnRestart;
			OnRestart();
		}

		private void OnRestart()
		{
			GameSceneManager.Instance.ReloadCurrentLevel();
		}

		private void OnDestroy()
		{
			if (instance == this) instance = null;
		}
	}
}
