using Com.GitHub.Knose1.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.GitHub.Knose1.MiniJam75.Game
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerPhysic : MonoBehaviour
	{
		private Rigidbody2D rb2D;

		[Header("Raycast")]
		[SerializeField] private float circleRadius = 0.5f;
		[SerializeField] private float maxCastDistance = 1000f;
		[SerializeField] private float distanceIsGrounded = 0.1f;
		[SerializeField] private LayerMask raycastLayerMask = default;

		[Header("Physic")]
		[SerializeField] private float forwardSpeed = 1;
		[SerializeField] private float impulseOnGround = 2;
		[SerializeField, Tooltip("The impulse units you lose by seconds")] public float gravity = 1;
		[SerializeField] public float weight = 1;
		[SerializeField] private float maxSpeed = 100;

		[Header("Juicy")]
		[SerializeField] private GameObject groundParticlePrefab = null;

		[Header("GameOver")]
		[SerializeField] private float distanceDeathThreshold = 0.05f;
		[SerializeField] private float distanceDeahHit = 0.03f;
		[SerializeField] private float deathCoyoteeTime = 0.01f;
		[SerializeField] private LayerMask gameOverRaycastLayerMask = default;
		[SerializeField] private bool debugPauseOnDeath = false;

		[Header("Physic Power Up")]
		[SerializeField] private LayerMask physicPowerUpLayerMask = default;
		[SerializeField] private float timeBetweenEffects = 0.1f;

		[Header("Game End transition")]
		[SerializeField] private PlayerInput input = null;
		[SerializeField] private Rigidbody2D myRb = null;
		[SerializeField] private Scroller backgroundScroller = null;
		[SerializeField] private SpriteRenderer playerVisual = null;
		[SerializeField] private GameObject trailParticle = null;
		[SerializeField] private GameObject gameOverParticle = null;
		[SerializeField] private float gameOverTransitionTime = 0.5f;
		[SerializeField] private float winTransitionTime = 3f;

		private bool enableMove = true;
		private float ySpeed;
		private float xPos;
		private float yPos;
		private float preventParticleSpawn;
		private float preventGroundCollision;
		public event Action<bool> OnEnd;

		private void Awake()
		{
			rb2D = GetComponent<Rigidbody2D>();
			xPos = transform.position.x;
			yPos = transform.position.y;
		}

		private void FixedUpdate()
		{
			if (!enableMove) return; 

			float fixedDeltaTime = Time.fixedDeltaTime;
			
			HandleMove(fixedDeltaTime);
			HandleHit(fixedDeltaTime);

		}

		/// <summary>
		/// Handle :<br/>
		/// - Location<br/>
		/// - Collision checking (ground and roof)<br/>
		/// - Bounce on collision<br/>
		/// - Particle spawning on Bounce<br/>
		/// </summary>
		/// <param name="fixedDeltaTime"></param>
		private void HandleMove(float fixedDeltaTime)
		{

			float gravity = fixedDeltaTime * this.gravity * weight;
			ySpeed = Mathf.Clamp(ySpeed - gravity, -maxSpeed, maxSpeed);

			int direction = GetGravityDirection(gravity);

			//*/////////////////*//
			//* Ground Checking *//
			//*/////////////////*//
			bool isGrounded = false;
			RaycastHit2D hitGround = CastCircle(Vector2.down * direction, raycastLayerMask, out bool isCollisionGround);
			if (isCollisionGround)
			{
				isGrounded = hitGround.distance <= (distanceIsGrounded * transform.localScale.y);

				if (isGrounded)
				{
					SetYSpeedAndSpawnParticle(fixedDeltaTime, direction, hitGround);
				}
			}

			if (!isGrounded)
			{
				//*/////////////////*//
				//*  Roof Checking  *//
				//*/////////////////*//
				RaycastHit2D hitRoof = CastCircle(Vector2.up * direction, raycastLayerMask, out bool isCollisionRoof);
				if (isCollisionRoof)
				{
					if (hitRoof.distance <= (distanceIsGrounded * transform.localScale.y))
					{
						SetYSpeedAndSpawnParticle(fixedDeltaTime, -direction, hitRoof);
					}
				}
			}

			//xPos
			xPos += forwardSpeed * fixedDeltaTime;

			//yPos
			yPos += ySpeed * fixedDeltaTime;

			transform.position = rb2D.position = new Vector2(
				x: xPos,
				y: yPos
			);
		}

		private void SetYSpeedAndSpawnParticle(float fixedDeltaTime, int direction, RaycastHit2D hitGround)
		{
			if (preventGroundCollision > 0)
			{
				preventGroundCollision -= fixedDeltaTime;
				if (preventParticleSpawn > 0)
				{
					preventParticleSpawn -= fixedDeltaTime;
				}
				return;
			}

			preventGroundCollision = 0;

			ySpeed = impulseOnGround * direction;

			if (preventParticleSpawn > 0)
			{
				preventParticleSpawn -= fixedDeltaTime;
				return;
			}

			preventParticleSpawn = 0;

			Transform particleSystem = Instantiate(groundParticlePrefab).transform;
			particleSystem.position = hitGround.point;

			if (direction == -1)
				particleSystem.rotation = Quaternion.AngleAxis(180, Vector3.forward) * Quaternion.AngleAxis(180, Vector3.up);
		}

		public int GetGravityDirection() => GetGravityDirection(gravity * weight);
		public int GetGravityDirection(float gravity)
		{
			int direction = 1;
			if (gravity < 0)
				direction = -1;
			return direction;
		}

		private RaycastHit2D CastCircle(Vector2 direction, LayerMask layerMask, out bool isCollision)
		{
			var hit = Physics2D.CircleCast(transform.position, circleRadius * transform.localScale.y, direction, maxCastDistance, layerMask);
			if (isCollision = hit.transform != null)
			{
				Debug.DrawLine(transform.position, hit.point, Color.red);

				//Check if the hitPoint is in the same direction as the vector "direction"
				//
				// Example : If direction is Vector2.up, then the hit position must be at upper than the position
				Vector2 meToPoint = hit.point - (Vector2)transform.position;
				float projection = Vector2Utils.Project(meToPoint, direction);

				//If the vector "meToPoint" doesn't have the same direction as "direction"
				if (projection < 0) isCollision = false;
			}

			return hit;
		}

		private void ColliderBorderThreshold(RaycastHit2D hit, Vector2 direction, ref bool isCollision, float threshold)
		{
			if (!isCollision) return;

			Vector2 normalA = Quaternion.AngleAxis(90, Vector3.forward) * direction;
			Vector2 normalB = Quaternion.AngleAxis(-90, Vector3.forward) * direction;

			//I'm making a collider threshold
			//
			//So that when the player touch the top of a hazard, (s)he doesn't get dead


			Bounds bounds = hit.collider.bounds;
			Vector2 position = hit.transform.position;
			Vector2 hitInLocalColliderPos = (Vector2)transform.position - position;

			Vector2 colliderDirectionA = (Vector2)bounds.ClosestPoint(position + normalA * 50) - position;
			Vector2 colliderDirectionB = (Vector2)bounds.ClosestPoint(position + normalB * 50) - position;

			float projectionOnNormalA = Vector2.Dot(colliderDirectionA, hitInLocalColliderPos) / colliderDirectionA.magnitude;
			float projectionOnNormalB = Vector2.Dot(colliderDirectionB, hitInLocalColliderPos) / colliderDirectionB.magnitude;

			float distanceA = Mathf.Abs(colliderDirectionA.magnitude - projectionOnNormalA);
			float distanceB = Mathf.Abs(colliderDirectionB.magnitude - projectionOnNormalB);
			float distance = Mathf.Min(distanceA, distanceB);

			if (distance < threshold)
				isCollision = false;

			Debug.DrawRay(position, hitInLocalColliderPos, Color.black);
			Debug.DrawRay(position, colliderDirectionA, Color.black);
			Debug.DrawRay(position, colliderDirectionB, Color.blue);
		}

		private Vector2 deathHitLocation;
		private float timeSinceDeathHit = 0;

		private void HandleHit(float fixedDeltaTime)
		{
			Vector2 direction = Vector2.right;
			RaycastHit2D hit = CastCircle(direction, gameOverRaycastLayerMask, out bool isCollision);

			if (isCollision && timeSinceDeathHit == 0)
			{
				deathHitLocation = hit.point;
			}

			isCollision = isCollision && deathHitLocation.x > transform.position.x;
			ColliderBorderThreshold(hit, direction, ref isCollision, distanceDeathThreshold);

			if (isCollision && hit.distance <= (distanceDeahHit * transform.localScale.y))
			{
				if (timeSinceDeathHit >= deathCoyoteeTime)
				{
					if (debugPauseOnDeath) Debug.Break();
					BeforeSendOnEnd(false);

					//transform.position = deathHitLocation;

					timeSinceDeathHit = 0;
				}
				else
					timeSinceDeathHit += fixedDeltaTime;
			}
			else
			{
				timeSinceDeathHit = 0;
			}
		}

		//*///////////////////////////////////////////*/
		//*                On Game End                */
		//*///////////////////////////////////////////*/

		private void BeforeSendOnEnd(bool isWin)
		{
			Destroy(input);
			StartCoroutine(isWin ? WinCoroutine() : DeathCoroutine());
		}

		private IEnumerator DeathCoroutine()
		{
			enableMove = false;
			myRb.isKinematic = true;
			backgroundScroller.enabled = false;

			MoveToLeftManager.Instance.speed = 0;

			//Destroy the visual
			Destroy(playerVisual);
			Destroy(trailParticle);

			//Spawn particles
			Transform tr = Instantiate(gameOverParticle).transform;
			tr.position = transform.position;

			yield return new WaitForSeconds(gameOverTransitionTime);
			OnEnd?.Invoke(false);
		}

		private IEnumerator WinCoroutine()
		{
			float startTimeScale = Time.timeScale;
			float time = winTransitionTime;
			while (time > 0)
			{
				Time.timeScale = Mathf.Lerp(0, startTimeScale, Mathf.InverseLerp(0, winTransitionTime, time));
				yield return new WaitForEndOfFrame();
				time -= Time.unscaledDeltaTime;
			}

			OnEnd?.Invoke(true);
		}


		private void OnDestroy()
		{
			OnEnd = null;
		}

		//*///////////////////////////////////////////*/
		//*            Interaction Handler            */
		//*///////////////////////////////////////////*/

		private float lastTimeSinceCollision = 0;
		private void OnTriggerEnter2D(Collider2D collision)
		{
			float time = Time.time;

			if ((time - lastTimeSinceCollision) < timeBetweenEffects)
				return;

			GameObject otherGameObject = collision.gameObject;
			int layer = otherGameObject.layer;
			if (physicPowerUpLayerMask == (physicPowerUpLayerMask | (1 << layer)))
			{
				//If the other game object is on the layer

				IPlayerInteraction playerInteraction = otherGameObject.GetComponent<IPlayerInteraction>();
				if (playerInteraction is null) return;
				
				//Avoid double collisions
				lastTimeSinceCollision = time;

				var modifier = GetInteractionModifier();
				playerInteraction.Interact(ref modifier);
				ApplyInteractionModifier(modifier);
			}

		}

		//*///////////////////////////////////////////*/
		//*            InteractionModifier            */
		//*///////////////////////////////////////////*/

		private InteractionModifier GetInteractionModifier() => new InteractionModifier(weight: weight)
		{
			gravity = gravity,
			ySpeed = ySpeed,
			preventParticleSpawn = preventParticleSpawn
		};

		private void ApplyInteractionModifier(InteractionModifier interactionModifier)
		{
			gravity = interactionModifier.gravity;
			ySpeed = interactionModifier.ySpeed;
			preventParticleSpawn = interactionModifier.preventParticleSpawn;
			preventGroundCollision = interactionModifier.preventGroundCollision;

			if (interactionModifier.isEnd)
			{
				BeforeSendOnEnd(interactionModifier.endStatus);
			}
		}
	}


	public struct InteractionModifier
	{
		public float ySpeed;
		public float gravity;
		public float preventGroundCollision;
		public float preventParticleSpawn;

		public bool isEnd;

		/// <summary>
		/// This bool is triggered if <see cref="isEnd"/> is true <br/>
		/// True = win<br/>
		/// False = death
		/// </summary>
		public bool endStatus;
		
		public readonly float weight;

		public InteractionModifier(float weight) : this() => this.weight = weight;
	}

	public interface IPlayerInteraction
	{
		void Interact(ref InteractionModifier interactionModifier);
	}

}
