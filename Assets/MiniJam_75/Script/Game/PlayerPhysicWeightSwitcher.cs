using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	[RequireComponent(typeof(PlayerPhysic))]
	public class PlayerPhysicWeightSwitcher : MonoBehaviour
	{
		[System.Serializable]
		private struct WeightSettings
		{
#pragma warning disable CS0649 // Warning Unity "never assigned"
			[SerializeField] public float weight;
			[SerializeField] public float size;
			[SerializeField] public Color color;
			[SerializeField] public Sprite sprite;
#pragma warning restore CS0649 // Warning Unity "never assigned"
		}

		[SerializeField] private SpriteRenderer spriteRend = null;
		[SerializeField] private int defaultWeight = 0;
		[SerializeField] private List<WeightSettings> weights = new List<WeightSettings>();

		PlayerPhysic playerPhysic;

		int _currentWeight;
		protected int CurrentWeight
		{
			get => _currentWeight;
			set {
				int count = weights.Count;
				int clampedValue = (value + count) % count;
				if (clampedValue < 0) clampedValue = 0;
				_currentWeight = clampedValue;

				WeightSettings settings = weights[clampedValue];

				playerPhysic.weight = settings.weight;

				spriteRend.sprite = settings.sprite;
				spriteRend.color = settings.color;

				transform.localScale = Vector3.one * settings.size;
			}
		}

		private void OnValidate()
		{
			if (weights is null) weights = new List<WeightSettings>();

			if (weights.Count == 0)
			{
				weights.Add(new WeightSettings());
			}
			defaultWeight = Mathf.Clamp(defaultWeight, 0, weights.Count - 1);
		}

		private void Awake()
		{
			playerPhysic = GetComponent<PlayerPhysic>();
		}

		private void Start()
		{
			CurrentWeight = defaultWeight;
		}


		int lastWeightValue = 0;

		/// <summary>
		/// Unity Input Event
		/// </summary>
		/// <param name="value"></param>
		protected void OnWeight(InputValue value)
		{
			int weightValue = (int)value.Get<float>();

			if (weightValue == lastWeightValue) return;

			lastWeightValue = weightValue;

			if (weightValue == 0) return;

			CurrentWeight += lastWeightValue;
		}
	}
}
