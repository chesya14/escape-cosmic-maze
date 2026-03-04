using UnityEngine;
using UnityEngine.SceneManagement; // Ini untuk pindah ke labirin nanti

public class MenuManager : MonoBehaviour
{
	public void KeluarGame()
	{
		Debug.Log("Pemain menekan tombol YES. Game Keluar!");

		// Perintah untuk keluar kalau sudah jadi aplikasi (.exe/.apk)
		Application.Quit();

		// Perintah KHUSUS untuk menghentikan mode PLAY di Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
	}

	public void MainkanGame()
	{
		// Ganti nama di dalam tanda petik sesuai nama Scene Labirinmu!
		SceneManager.LoadScene("Scene_MediumMaze");
	}
}