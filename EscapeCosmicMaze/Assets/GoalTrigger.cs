using UnityEngine;
using TMPro; // Pastikan ini ada karena kita pakai TextMeshPro
using UnityEngine.SceneManagement;
using System.Collections;

public class GoalTrigger : MonoBehaviour
{
	[Header("UI & Skor")]
	public TextMeshProUGUI teksAngkaSkor; // Tarik objek ScoreText ke sini
	public GameObject teksEscaped;

	[Header("Aturan Main")]
	public int skorMinimum = 150; // Target skor yang kamu mau

	private int skorTotal = 0;

	void Start()
	{
		UpdateTampilan();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		// 1. Jika menyentuh Bintang
		if (other.CompareTag("Bintang"))
		{
			skorTotal += 10; // Tambah 10 poin
			UpdateTampilan(); // Ganti angka di layar
			Destroy(other.gameObject); // Hilangkan bintang
			Debug.Log("Skor sekarang: " + skorTotal);
		}

		// 2. Jika menyentuh Portal
		if (other.CompareTag("Portal"))
		{
			// Cek apakah skor sudah mencapai 150
			if (skorTotal >= skorMinimum)
			{
				teksEscaped.SetActive(true);
				Time.timeScale = 0f;
				StartCoroutine(SelesaiGame());
			}
			else
			{
				// Muncul di Console kalau belum cukup (buat debug)
				Debug.Log("Skor belum cukup! Kamu butuh " + (skorMinimum - skorTotal) + " poin lagi.");
			}
		}
	}

	void UpdateTampilan()
	{
		if (teksAngkaSkor != null)
		{
			teksAngkaSkor.text = skorTotal.ToString();

			// BIAR KEREN: Kalau sudah 150, angka berubah jadi hijau (portal siap!)
			if (skorTotal >= skorMinimum)
			{
				teksAngkaSkor.color = Color.green;
			}
			else
			{
				teksAngkaSkor.color = Color.white; // Tetap putih kalau belum cukup
			}
		}
	}

	IEnumerator SelesaiGame()
	{
		yield return new WaitForSecondsRealtime(3f);
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}