using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	[ExecuteAlways]
	public class SetTrackedBodyOffestByRatio : MonoBehaviour
	{
		[SerializeField] new Camera camera = null;
		[SerializeField] CinemachineVirtualCamera virtualCamera = null;
		[SerializeField] float offset = 0;
		[SerializeField] float minOffset = -1000;
		[SerializeField] float offsetY = 2;
		[SerializeField] PlayerPhysic playerPhysic = null;

		CinemachineFramingTransposer transposer;

		// Update is called once per frame
		void Update()
		{
			if (!virtualCamera || !camera) return;
			if (!transposer) transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
			if (!transposer) return;

			transposer.m_TrackedObjectOffset.x = Mathf.Max(virtualCamera.m_Lens.OrthographicSize * camera.aspect + offset, minOffset);

			if (playerPhysic)
				transposer.m_TrackedObjectOffset.y = playerPhysic.GetGravityDirection() * offsetY;
		}
	}
}
