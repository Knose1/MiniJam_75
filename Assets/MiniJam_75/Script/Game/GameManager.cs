using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.GitHub.Knose1.Common.AnimationUtils;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private PlayerPhysic playerPhysic = null;

		[Header("Game End")]
		[SerializeField] private CanvasGroup gameEnd = null;
		[SerializeField] private Button restartBtn = null;
		[SerializeField] private Text titleTxt = null;
		[SerializeField] private string textWin = "Victory";
		[SerializeField] private Text restartTxt = null;
		[SerializeField] private string restartText = "Restart";
		[SerializeField] private Color colorWin = Color.green;
		[SerializeField] private Animator gameEndAnimator = null;
		[SerializeField] private AnimatorParameter gameEndTrigger = default;

		// Start is called before the first frame update
		void Awake()
		{
			playerPhysic.OnEnd += PlayerPhysic_OnEnd;
			Time.timeScale = PlayerSettings.Instance.timeScale;
		}

		private void PlayerPhysic_OnEnd(bool isWin)
		{
			if (!isWin && PlayerSettings.Instance.autoRestart)
			{
				OnRestart();
				return;
			}

			playerPhysic.OnEnd -= PlayerPhysic_OnEnd;

			Time.timeScale = 0;
			gameEnd.alpha = 1;
			gameEnd.blocksRaycasts = true;

			if (isWin)
			{
				restartTxt.text = restartText;
				titleTxt.text = textWin;
				titleTxt.color = colorWin;
			}

			gameEndTrigger.Call(gameEndAnimator);

			restartBtn.onClick.AddListener(OnRestart);
		}

		private void OnRestart()
		{
			restartBtn.onClick.RemoveAllListeners();
			GameSceneManager.Instance.ReloadCurrentScene();
		}
	}
}
