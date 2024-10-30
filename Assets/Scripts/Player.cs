using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Monkey currentPlaceBuffer;
    private Camera cam;
    private bool isPlacing;

    private Monkey selectedMonkey;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = -30;
            RaycastHit2D hit = Physics2D.Raycast(mousepos, Vector3.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                Monkey m = hit.collider.gameObject.GetComponent<Monkey>();
                if (m != null)
                {
                    selectMonkey(hit.collider.gameObject.GetComponent<Monkey>());
                    return;
                }

                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Monkey"))
                    unselectMoneky();
            }
        }
    }

    private void unselectMoneky()
    {
        if (selectedMonkey != null)
        {
            UImanager.instance.deselectMonkey();
            selectedMonkey.selected(false);
            selectedMonkey = null;
        }
    }

    private void selectMonkey(Monkey m)
    {
        
        selectedMonkey = m;
        UImanager.instance.selectMonkey(m);
        selectedMonkey.selected(true);
    }
    

    private void placeMonkey(Vector3 pos)
    {
        pos.z = -5;
        Instantiate(currentPlaceBuffer.gameObject, pos, quaternion.identity);

    }

    public void StartPlace(Monkey m)
    {
        if (isPlacing) return;

        currentPlaceBuffer = m;
        if (GameManager.instance.money < currentPlaceBuffer.cost) return;
        isPlacing = true;
        StartCoroutine(place());
    }


    IEnumerator place()
    {
        GameObject ghost = Instantiate(currentPlaceBuffer.mesh, Vector3.zero, quaternion.identity);
        GameObject radius = Instantiate(currentPlaceBuffer.radiusDisplay, Vector3.zero, quaternion.identity);
        radius.transform.localScale = new Vector2(currentPlaceBuffer.radius * 2, currentPlaceBuffer.radius * 2);
        
        while (isPlacing)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            ghost.transform.position = mousePos;
            radius.transform.position = mousePos;

            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.instance.money >= currentPlaceBuffer.cost)
                {
                    placeMonkey(mousePos);

                    GameManager.instance.money -= currentPlaceBuffer.cost;
                    currentPlaceBuffer = null;
                    isPlacing = false;
                    
                    
                    Destroy(ghost);
                    Destroy(radius);
                }
                else
                {
                    currentPlaceBuffer = null;
                    isPlacing = false;
                    Destroy(ghost);
                    Destroy(radius);
                    break;
                }
            }

            yield return null;
        }
    }
}
