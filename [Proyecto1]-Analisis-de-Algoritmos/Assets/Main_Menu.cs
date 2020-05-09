using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    // Start is called before the first frame update

    private static string NonoGramPath = string.Empty;


    public void PlayGame() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void selectNono() {

        var fileContent = string.Empty;

        using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Get the path of specified file
                setNonoGramPath(openFileDialog.FileName);
                UnityEngine.Debug.Log(NonoGramPath);
            }
        }
    }

    public void setNonoGramPath(string path)
    {
        NonoGramPath = path;

    }

    public string getNonoGramPath()
    {
        return NonoGramPath;

    }

}
