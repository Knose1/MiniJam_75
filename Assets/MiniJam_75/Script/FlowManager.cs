using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75
{
	public class FlowManager : MonoBehaviour
	{
		[SerializeField] private bool startWithLevel1 = false;

		private static FlowManager instance;

		/// <summary>
		/// Unique instance 
		/// </summary>
		public static FlowManager Instance
		{
			get
			{
				return instance = instance ?? FindObjectOfType<FlowManager>();
			}
		}

		private void Awake()
		{
			if (!instance) instance = this;
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			if (startWithLevel1) GameSceneManager.Instance.LoadLevel(1);
			else GameSceneManager.Instance.LoadTitleCard();
		}

		private void OnDestroy()
		{
			instance = null;
		}
	}
}