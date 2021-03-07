using Com.GitHub.Knose1.Common.Utils;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75.Game.Trigger
{
	public class Bounce : MonoBehaviour, IPlayerInteraction
	{
		[SerializeField] private Transform location = null;
		[SerializeField] private GameObject particleSpawn = null;
		[SerializeField] private float bounce = 1;
		[SerializeField] private float preventSpawnTime = 0.001f;

		public void Interact(ref InteractionModifier interactionModifier)
		{
			float projected = transform.up.y;
			interactionModifier.ySpeed = bounce * projected;
			
			interactionModifier.preventGroundCollision = preventSpawnTime;
			interactionModifier.preventParticleSpawn = preventSpawnTime;

			Transform tr = Instantiate(particleSpawn).transform;
			tr.position = location.position;
			tr.rotation = location.rotation;
		}
	}
}
