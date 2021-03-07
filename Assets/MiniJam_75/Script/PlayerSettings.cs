﻿using System;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75
{

	/// <summary>
	/// 
	/// </summary>
	public class PlayerSettings : MonoBehaviour
	{
		[SerializeField] public float timeScale = 1;


		private static PlayerSettings instance;

		/// <summary>
		/// Unique instance 
		/// </summary>
		public static PlayerSettings Instance
		{
			get
			{
				return instance = instance ?? FindObjectOfType<PlayerSettings>();
			}
		}

		private void Awake()
		{
			if (!instance) instance = this;

			DontDestroyOnLoad(gameObject);
		}

		public void OnDestroy()
		{
			instance = null;
		}
	}
}