using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public Image rotationTutorial;
    public Image cameraTutorial;
    public Image deleteTutorial;
    public Image destroyTutorial;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TutorialCoroutine(){
      yield return new WaitForSeconds(1);
      rotationTutorial.enabled = true;
      while(!Input.GetMouseButtonDown(0) && 
            !Input.GetMouseButtonDown(1)){
        yield return null;
      }
      rotationTutorial.enabled = false;
      yield return new WaitForSeconds(1);
      deleteTutorial.enabled = true;
      while(!Input.GetMouseButtonDown(0) && 
            !Input.GetMouseButtonDown(1)){
        yield return null;
      }
      deleteTutorial.enabled = false;
      yield return new WaitForSeconds(3);
      cameraTutorial.enabled = true;
      yield return new WaitForSeconds(3);
      cameraTutorial.enabled = false;
    }
}
