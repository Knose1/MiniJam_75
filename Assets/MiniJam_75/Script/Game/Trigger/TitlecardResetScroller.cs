using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75.Game.Trigger
{
	public class TitlecardResetScroller : MonoBehaviour, IPlayerInteraction
	{
		[SerializeField] Scroller scroller;

		public void Interact(ref InteractionModifier interactionModifier)
		{
			scroller.scroll = Vector3.zero;
		}
	}
}
