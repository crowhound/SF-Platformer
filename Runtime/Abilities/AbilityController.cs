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

            if (AbilityOwner != null)
                _controller2D = AbilityOwner.GetComponent<Controller2D>();
            else
                _controller2D = GetComponent<Controller2D>();
        }
        private void Start()
        {
            for (int i = 0; i < Abilities.Count; i++)
            {
                if(Abilities[i].InitOnStart)
                {
                    Abilities[i].Initialize(_controller2D);
                }
            }
        }

        private void Update()
        {
            for (int i = 0; i < Abilities.Count; i++)
            {
                // If the Ability is not controlled by some form of input it runs on update.
                if(Abilities[i] is not IInputAbility)
                    Abilities[i].UpdateAbility();
            }
        }
    }
}
