using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByNorth.Core
{
    public class SceneChanger: MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

            SceneManager.LoadSceneAsync(1);
        }
    }
}