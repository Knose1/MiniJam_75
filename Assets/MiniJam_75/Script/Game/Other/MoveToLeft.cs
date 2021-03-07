using System;
using UnityEngine;

namespace Com.GitHub.Knose1.MiniJam75
{
	public class MoveToLeft : MonoBehaviour
	{
		[SerializeField] public float speed = 5f;
		[SerializeField] public float destroyPosition = -20;
		[SerializeField] public GameObject destroyGameobject = null;

		public event Action<GameObject, MoveToLeft> OnDispose;

		// Update is called once per frame
		void Update()
		{
			if (transform.position.x <= destroyPosition)
			{
				GameObject toDestroy = destroyGameobject ?? gameObject;
				OnDispose?.Invoke(toDestroy, this);
				Destroy(toDestroy);
			}

			transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
		}

		private void OnDestroy()
		{
			OnDispose = null;
		}
	}
}
