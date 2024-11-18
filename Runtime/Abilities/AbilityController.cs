using System.Collections.Generic;
using System.Linq;
using SF.Characters.Controllers;
using UnityEngine;

namespace SF.AbilityModule
{
    public class AbilityController : MonoBehaviour
    {
        //The gameobject the abilities will control.
        public GameObject AbilityOwner;
        public List<AbilityCore> Abilities = new List<AbilityCore>();

        private Controller2D _controller2D;
        private void Awake()
        {
            Abilities = GetComponents<AbilityCore>().ToList();

            _controller2D = AbilityOwner != null ? AbilityOwner.GetComponent<Controller2D>() : GetComponent<Controller2D>();
        }
        private void Start()
        {
            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].Initialize(_controller2D);
            }
        }
    }
}
