using Com.GitHub.Knose1.Common.AnimationUtils;
using Com.GitHub.Knose1.MiniJam75.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Com.GitHub.Knose1.MiniJam75.UI
{
	public class Titlecard : MonoBehaviour
	{
		[SerializeField] private Button normalDifficultyBtn = null;
		[SerializeField] private Button hardDifficultyBtn = null;
		[SerializeField] private Button exitBtn = null;

		void Start()
		{
			Time.timeScale = 1;

			normalDifficultyBtn.onClick.AddListener(NormalDifficultyBtnClick);
			hardDifficultyBtn.onClick.AddListener(HardDifficutlyBtnClick);
			exitBtn.onClick.AddListener(ExitBtnClick);
		}

		private void NormalDifficultyBtnClick()
		{
			PlayerSettings.Instance.timeScale = 1;
			GameSceneManager.Instance.LoadLevel(1);
		}

		private void HardDifficutlyBtnClick()
		{
			PlayerSettings.Instance.timeScale = 2;
			GameSceneManager.Instance.LoadLevel(1);
		}

		private void ExitBtnClick()
		{
			Application.Quit();
		}

	}
}
