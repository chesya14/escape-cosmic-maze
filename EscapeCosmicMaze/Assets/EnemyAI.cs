using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections; // WAJIB ADA BUAT BIKIN JEDA WAKTU (TIMER)

public class EnemyAI : MonoBehaviour
{
	[Header("Target Utama")]
	public Transform player;

	[Header("Sistem Patroli")]
	public Transform[] points;
	private int destPoint = 0;
	public float patrolSpeed = 1.5f;
	public float detectRange = 3.5f;

	[Header("Fuzzy Logic Info")]
	public float currentSpeed;
	public float distanceToPlayer;

	[Header("UI Game Over")]
	public GameObject teksGameOver; // 👈 INI YANG BIKIN KOTAKNYA MUNCUL DI UNITY

	private NavMeshAgent agent;

	private float speedLambat = 1.0f;
	private float speedSedang = 2.0f;
	private float speedCepat = 3.5f;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

		// Pastikan tulisan game over ngumpet pas game mulai
		if (teksGameOver != null)
		{
			teksGameOver.SetActive(false);
		}
	}

	void Update()
	{
		if (player == null) return;

		distanceToPlayer = Vector2.Distance(transform.position, player.position);

		if (distanceToPlayer < detectRange)
		{
			ChasePlayer();
		}
		else
		{
			Patroling();
		}

		FaceTarget();
	}

	void Patroling()
	{
		if (points.Length == 0) return;
		agent.speed = patrolSpeed;
		agent.SetDestination(points[destPoint].position);

		if (!agent.pathPending && agent.remainingDistance < 0.5f)
		{
			destPoint = (destPoint + 1) % points.Length;
		}
	}

	void ChasePlayer()
	{
		agent.SetDestination(player.position);
		currentSpeed = CalculateFuzzySpeed(distanceToPlayer);
		agent.speed = currentSpeed;
	}

	void FaceTarget()
	{
		if (agent.velocity.x > 0.1f)
		{
			transform.localScale = new Vector3(0.2f, 0.2f, 1);
		}
		else if (agent.velocity.x < -0.1f)
		{
			transform.localScale = new Vector3(-0.2f, 0.2f, 1);
		}
	}

	// --- RUMUS FUZZY LOGIC TSK ---
	float CalculateFuzzySpeed(float dist)
	{
		float mu_Dekat = FuzzyTriangle(dist, -1, 0, 4);
		float mu_Sedang = FuzzyTriangle(dist, 2, 5, 8);
		float mu_Jauh = FuzzyTrapezoid(dist, 6, 10, 100, 100);

		float totalWeight = mu_Dekat + mu_Sedang + mu_Jauh;
		if (totalWeight <= 0) return speedLambat;

		return ((mu_Dekat * speedCepat) + (mu_Sedang * speedSedang) + (mu_Jauh * speedLambat)) / totalWeight;
	}

	float FuzzyTriangle(float x, float a, float b, float c)
	{
		if (x <= a || x >= c) return 0;
		if (x == b) return 1;
		if (x < b) return (x - a) / (b - a);
		return (c - x) / (c - b);
	}

	float FuzzyTrapezoid(float x, float a, float b, float c, float d)
	{
		if (x <= a || x >= d) return 0;
		if (x >= b && x <= c) return 1;
		if (x < b) return (x - a) / (b - a);
		return (d - x) / (d - c);
	}

	// --- FITUR GAME OVER (TABRAKAN) ---
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("HAP! Astronot Ketangkep!");

			// 1. Munculkan tulisan Game Over
			if (teksGameOver != null)
			{
				teksGameOver.SetActive(true);
			}

			// 2. Bekukan waktu biar Alien & Astronot berhenti gerak
			Time.timeScale = 0f;

			// 3. Mulai timer untuk restart
			StartCoroutine(TungguLaluRestart());
		}
	}

	// Fungsi Timer Game Over
	IEnumerator TungguLaluRestart()
	{
		// Tunggu 2 detik
		yield return new WaitForSecondsRealtime(2f);

		// Cairkan waktu dan Restart Scene
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}