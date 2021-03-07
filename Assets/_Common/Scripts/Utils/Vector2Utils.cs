using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.GitHub.Knose1.Common.Utils
{
	public static class Vector2Utils
	{
		/// <summary>
		/// Project a vector onto a normal
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="normal"></param>
		/// <returns></returns>
		public static float Project(Vector2 vector, Vector2 normal) 
			=> Vector2.Dot(normal, vector) / normal.magnitude;

	}
}
