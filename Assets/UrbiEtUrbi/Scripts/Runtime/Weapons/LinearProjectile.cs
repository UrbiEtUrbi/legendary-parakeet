using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile
{


  

    
    private void FixedUpdate()
    {
        transform.position += Direction * Speed * Time.fixedDeltaTime;
    }


    

   
}
