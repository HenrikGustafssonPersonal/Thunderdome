using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DamagedDirection : MonoBehaviour
{

    public Transform damagedDirectionImagePos;
    private Transform playerPosition;
    public GameObject damgedDirImage;

    public void DrawDamageIndicator(Vector3 enemyPosition)
    {
        //Draw damage indicator on UI
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 tempTargetPosition = new Vector3(enemyPosition.x, 0, enemyPosition.z);
        Vector3 tempPlayerPosition = new Vector3(playerPosition.position.x, 0, playerPosition.position.z);
        Vector3 targetDir = new Vector3(tempTargetPosition.x - tempPlayerPosition.x, 0, tempPlayerPosition.z - tempTargetPosition.z);

        float angle = Vector3.Angle(targetDir, transform.forward);
        Vector3 cross = Vector3.Cross(targetDir, transform.forward);
        if (cross.y < 0)
        {
            angle = 360 - angle; // Adjust the angle to cover the full 360 degrees
        }
        damagedDirectionImagePos.rotation = Quaternion.Euler(damagedDirectionImagePos.rotation.x, damagedDirectionImagePos.rotation.y, playerPosition.rotation.eulerAngles.y - angle + 180 + (90));

        Vector3 finalRot = new Vector3(0, 0, playerPosition.rotation.eulerAngles.y - angle + 180 + (90));

        GameObject damagedArrow = Instantiate(damgedDirImage, this.gameObject.transform.position, Quaternion.Euler(finalRot));
        damagedArrow.transform.SetParent(this.gameObject.transform);
        //Instatiate damage indicator on the UI



    }

    public void DrawWarningIndicator(Vector3 enemyPosition)
    {

        //Draw warning indicator for ranger enemies when charging, remove indicator when destoryed or finished charge

        //Instatiate damage indicator on the Warning

    }

}
