using Com.GitHub.Knose1.Common.AnimationUtils;
using Com.GitHub.Knose1.MiniJam75.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Com.GitHub.Knose1.MiniJam75.UI
{
	public class PauseGame : MonoBehaviour
	{
		[SerializeField] private Button resumeBtn = null;
		[SerializeField] private Button restartBtn = null;
		[SerializeField] private Button menuBtn = null;
		[SerializeField] private Toggle autoRestartTogg = null;
		[SerializeField] private Animator animator = null;
		[SerializeField] private AnimatorParameter showTrigger = default;
		[SerializeField] private float showTime = default;
		[SerializeField] private AnimatorParameter hideTrigger = default;
		[SerializeField] private float hideTime = default;

		void Start()
		{
			resumeBtn.onClick.AddListener(ResumeBtnClick);
			restartBtn.onClick.AddListener(RestartBtnClick);
			menuBtn.onClick.AddListener(MenuBtnClick);
			autoRestartTogg.onValueChanged.AddListener(AutoRestartToggValueChanged);
		}

		private void ResumeBtnClick()
		{
			StartCoroutine(Hide());
		}

		private void RestartBtnClick()
		{
			StartCoroutine(Hide());
			GameSceneManager.Instance.ReloadCurrentLevel();
		}

		private void MenuBtnClick()
		{
			StartCoroutine(Hide());
			
			GameSceneManager.Instance.LoadTitleCard();
			//OnMenu?.Invoke(this);
		}

		private void AutoRestartToggValueChanged(bool arg0)
		{
			PlayerSettings.Instance.autoRestart = arg0;
		}

		public IEnumerator Show()
		{
			isShown = true;
			autoRestartTogg.SetIsOnWithoutNotify(PlayerSettings.Instance.autoRestart);

			showTrigger.Call(animator);
			GameManager.Instance.DisablePlay();
			yield return new WaitForSecondsRealtime(showTime);
		}

		public IEnumerator Hide()
		{
			isShown = false;
			hideTrigger.Call(animator);
			yield return new WaitForSecondsRealtime(hideTime);
			GameManager.Instance.AllowPlay();
		}

		bool lastPressedValue;
		bool isShown = false;

		/// <summary>
		/// Unity Input Event
		/// </summary>
		/// <param name="value"></param>
		private void OnSwitchPause(InputValue value)
		{
			if (value.isPressed && value.isPressed != lastPressedValue)
			{
				if (!isShown)
				StartCoroutine(Show());
				else
				StartCoroutine(Hide());
			}

			lastPressedValue = value.isPressed;

		}
	}
}
