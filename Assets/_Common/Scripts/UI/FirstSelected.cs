using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.GitHub.Knose1.Common.UI
{
	public class FirstSelected : MonoBehaviour
	{
		bool lastConditionValue;

		// Update is called once per frame
		void Update()
		{
			bool conditionValue = isActiveAndEnabled && GetComponentInParent<CanvasGroup>().blocksRaycasts;

			if (conditionValue && conditionValue != lastConditionValue)
			{
				EventSystem.current.SetSelectedGameObject(gameObject);
			}

			lastConditionValue = conditionValue;
		}
	}
}
