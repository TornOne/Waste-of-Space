using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public Image rotationTutorial;
    public Image cameraTutorial;
    public Image deleteTutorial;
    public Image destroyTutorial;

    public Image eShieldTutorial;
    public Image reinforcedTutorial;
    public Image reactorTutorial;
    public Image laserTutorial;
    public Image engineTutorial;

    private bool reinforced = false;
    private bool reactor = false;
    private bool eShield = false;
    private bool laser = false;
    private bool engine = false;


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
      yield return WaitForMouse();
      rotationTutorial.enabled = false;

      yield return new WaitForSeconds(1);

      deleteTutorial.enabled = true;
      yield return WaitForMouse();
      deleteTutorial.enabled = false;

      yield return new WaitForSeconds(1);

      destroyTutorial.enabled = true;
      yield return WaitForMouse();
      destroyTutorial.enabled = false;

      yield return new WaitForSeconds(3);
      cameraTutorial.enabled = true;
      yield return new WaitForSeconds(3);
      cameraTutorial.enabled = false;
      yield return new WaitForSeconds(1);

      while(true){
        while(GridManager.instance.tileHeld == null) 
          yield return null;
        Type held = GridManager.instance.tileHeld.GetType();

        if(reinforced && laser && reactor && reinforced && eShield) 
          break;

        if (!engine && held == typeof(Engine)){
            engineTutorial.enabled = true;
            yield return WaitForMouse();
            engineTutorial.enabled = false;
            engine = true;
        }else if(!reactor && held == typeof(Reactor)){
            reactorTutorial.enabled = true;
            yield return WaitForMouse();
            reactorTutorial.enabled = false;
            reactor = true;
        }else if(!laser && held == typeof(Turret)){
            laserTutorial.enabled = true;
            yield return WaitForMouse();
            laserTutorial.enabled = false;
            laser = true;
        }else if(!eShield && held == typeof(EnergyShield)){
            eShieldTutorial.enabled = true;
            yield return WaitForMouse();
            eShieldTutorial.enabled = false;
            eShield = true;
        }else if(!reinforced && held == typeof(Reinforced)){
            reinforcedTutorial.enabled = true;
            yield return WaitForMouse();
            reinforcedTutorial.enabled = false;
            reinforced = true;
        }
        yield return new WaitForSeconds(1);
      }
      Debug.Log("Tutorial finished");
    }

    private static IEnumerator WaitForMouse(){
        while(!Input.GetMouseButtonDown(0) && 
              !Input.GetMouseButtonDown(1)){
            yield return null;
        }
    }

}


