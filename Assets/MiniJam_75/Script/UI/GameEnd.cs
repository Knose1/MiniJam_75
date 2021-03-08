using Com.GitHub.Knose1.Common.AnimationUtils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.GitHub.Knose1.MiniJam75.UI
{
	public class GameEnd : MonoBehaviour
	{
		[SerializeField] private CanvasGroup gameEnd = null;
		[SerializeField] private Button restartBtn = null;
		[SerializeField] private Button menuBtn = null;
		[SerializeField] private Text titleTxt = null;
		[SerializeField] private Text restartTxt = null;
		[SerializeField] private Animator gameEndAnimator = null;
		[SerializeField] private AnimatorParameter showTrigger = default;
		[SerializeField] private float showTime = default;
		[SerializeField] private AnimatorParameter hideTrigger = default;
		[SerializeField] private float hideTime = default;

		[Header("Game Over")]
		[SerializeField] private string textGameOver = "Game Over";
		[SerializeField] private string retryText = "Retry";
		[SerializeField] private Color colorGameOver = Color.white;
		[Header("Win")]
		[SerializeField] private string textWin = "Victory";
		[SerializeField] private string restartText = "Restart";
		[SerializeField] private Color colorWin = Color.green;

		public event Action<GameEnd> OnRestart;
		//public event Action<GameEnd> OnMenu;

		// Start is called before the first frame update
		void Start()
		{
			restartBtn.onClick.AddListener(RestartBtnClick);
			menuBtn.onClick.AddListener(MenuBtnClick);
		}

		private void RestartBtnClick()
		{
			StartCoroutine(Hide());
			OnRestart?.Invoke(this);
		}

		private void MenuBtnClick()
		{
			StartCoroutine(Hide());
			GameSceneManager.Instance.LoadTitleCard();
			//OnMenu?.Invoke(this);
		}

		public IEnumerator Show(bool isWin)
		{
			gameEnd.alpha = 1;
			//gameEnd.blocksRaycasts = true;

			if (isWin)
			{
				restartTxt.text = restartText;
				titleTxt.text = textWin;
				titleTxt.color = colorWin;
			}
			else
			{
				restartTxt.text = retryText;
				titleTxt.text = textGameOver;
				titleTxt.color = colorGameOver;
			}

			showTrigger.Call(gameEndAnimator);
			yield return new WaitForSecondsRealtime(showTime);
		}

		public IEnumerator Hide()
		{
			gameEnd.alpha = 0;
			gameEnd.blocksRaycasts = false;

			hideTrigger.Call(gameEndAnimator);
			yield return new WaitForSecondsRealtime(hideTime);
		}
	}
}
