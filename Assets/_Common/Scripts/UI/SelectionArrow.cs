using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.GitHub.Knose1.Common.UI
{
	public class SelectionArrow : UIBehaviour
	{
		public List<Image> images;

		bool selected;

		protected override void Start()
		{
			OnDeselect();
		}

		private void Update()
		{
			if (!GetComponentInParent<CanvasGroup>().blocksRaycasts)
			{
				if (selected) OnDeselect();
				return;
			}

			if (EventSystem.current.currentSelectedGameObject == GetComponentInParent<Selectable>().gameObject)
			{
				if (!selected) OnSelect();
			}
			else
			{
				if (selected) OnDeselect();
			}

		}

		public void OnSelect()
		{
			selected = true;
			foreach (var item in images)
			{
				item.enabled = true;
			}
		}

		public void OnDeselect()
		{
			selected = false;
			foreach (var item in images)
			{
				item.enabled = false;
			}
		}
	}

}