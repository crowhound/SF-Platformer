using System;

using SF.Characters;

using UnityEngine;

namespace SF.SpawnModule
{
    public class CharacterSpawner : MonoBehaviour
    {
        /// <summary>
        /// This is the id of the character data inside of the database.
        /// </summary>
        public int SpawnedCharacterID = 0;

        public CharacterDB CharacterDB;

        public CharacterData SpawnedCharacterData;
        public void Start()
        {
            if(CharacterDB == null)
                return;

            SpawnedCharacterData = CharacterDB.GetCharacterDataByID(SpawnedCharacterID);

            if(SpawnedCharacterData != null)
                SpawnedCharacter();
        }

        private void SpawnedCharacter()
        {
            InstantiateAsync<CharacterData>(SpawnedCharacterData,transform.position,Quaternion.identity);
        }
    }
}
