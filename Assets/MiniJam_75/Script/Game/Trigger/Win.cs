using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75.Game.Trigger
{
	public class Win : MonoBehaviour, IPlayerInteraction
	{
		public void Interact(ref InteractionModifier interactionModifier)
		{
			interactionModifier.isEnd = true;
			interactionModifier.endStatus = true;
		}
	}
}
