using System.Collections.Generic;
using UnityEngine;

namespace SF.CommandModule
{
    public class CommandController : MonoBehaviour
    {
        [SerializeField] private bool DoStart = false;
        [SerializeReference] public List<CommandNode> Commands = new List<CommandNode>();

        public void Awake()
        {
        }

        public void Start()
        {
            if (DoStart)
                StartCommands();
        }

        public async void StartCommands()
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                await Commands[i].Use();
            }
        }
    }
}
