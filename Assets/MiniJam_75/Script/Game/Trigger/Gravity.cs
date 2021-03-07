using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75.Game.Trigger
{
	public class Gravity : MonoBehaviour, IPlayerInteraction
	{
		[SerializeField] private Transform location = null;
		[SerializeField] private GameObject particleSpawn = null;
		[SerializeField] private float preventSpawnTime = 0.01f;

		public void Interact(ref InteractionModifier interactionModifier)
		{
			interactionModifier.gravity *= -1;

			interactionModifier.preventParticleSpawn = preventSpawnTime;

			Transform tr = Instantiate(particleSpawn).transform;
			tr.position = location.position;
			tr.rotation = location.rotation;
		}
	}
}
