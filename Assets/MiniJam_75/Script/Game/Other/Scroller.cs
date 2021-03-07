using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75
{
	public class Scroller : MonoBehaviour
	{
		public enum ScrollContext
		{
			Global,Local
		}

		public Vector3 scrollOffset;
		public Vector3 scrollBounds;
		public Vector3 scrollSpeed;
		public ScrollContext scrollContext;

		private Vector3 scroll;

		// Update is called once per frame
		void Update()
		{
			scroll += scrollSpeed * Time.deltaTime;

			scroll.x %= scrollBounds.x;
			scroll.y %= scrollBounds.y;
			scroll.z %= scrollBounds.z;

			if (float.IsNaN(scroll.x)) scroll.x = 0;
			if (float.IsNaN(scroll.y)) scroll.y = 0;
			if (float.IsNaN(scroll.z)) scroll.z = 0;

			Vector3 pos = scrollOffset + scroll;
			switch (scrollContext)
			{
				case ScrollContext.Global:
					transform.position = pos;
					break;
				case ScrollContext.Local:
					transform.localPosition = pos;
					break;
				default:
					break;
			}
		}
	}

}