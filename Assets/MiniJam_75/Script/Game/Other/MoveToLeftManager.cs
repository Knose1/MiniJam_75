using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	public class MoveToLeftManager : MonoBehaviour
	{
		[SerializeField] public float speed = 5f;
		
		private static MoveToLeftManager instance;

		/// <summary>
		/// Unique instance 
		/// </summary>
		public static MoveToLeftManager Instance
		{
			get
			{
				return instance = instance ?? FindObjectOfType<MoveToLeftManager>();
			}
		}

		private void Awake()
		{
			if (!instance) instance = this;

		}

		private void OnDestroy()
		{
			if (instance == this)
				instance = null;
		}
	}
}