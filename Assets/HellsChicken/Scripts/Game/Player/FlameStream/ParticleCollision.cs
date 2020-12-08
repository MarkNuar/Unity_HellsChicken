﻿using System;
using System.Collections.Generic;
using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.FlameStream {
    public class ParticleCollision : MonoBehaviour {

        [SerializeField] private CheckEnemies listScript;

        private void OnParticleCollision(GameObject other) {
            
            Transform shield = other.transform.Find("Shield");
            
            if (shield != null) {
                listScript.GOList.Add(other.gameObject.name);
                shield.gameObject.GetComponent<Destruction>().Destroyer();
            }

            if (other.gameObject.layer == 12 && !listScript.GOList.Contains(other.gameObject.name)) {
                Destruction destr = other.GetComponent<Destruction>();
                
                if(destr == null)    
                    destr = other.gameObject.transform.parent.GetComponent<Destruction>();
                
                destr.Destroyer();
            }
        }
    }
}