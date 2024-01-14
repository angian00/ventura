using System.Collections.Generic;
using UnityEngine;
using Ventura.Generators;
using Ventura.Util;

namespace Ventura.Test.VillageSimulator
{
    public class Person
    {
        public string name;
        public string job;


        public void PrintState()
        {
            DebugUtils.Log($"{name}: {job}");
        }
    }


    public class Village
    {
        public string name;
        public Vector2Int pos;
        public int nPersons;

        public List<Person> persons;

        public Village()
        {
            nPersons = Random.Range(8, 20);
            persons = new();
            for (int i = 0; i < nPersons; i++)
            {
                Person person = new Person();
                person.name = FileStringGenerator.FirstNames.GenerateString();
                person.job = FileStringGenerator.Jobs.GenerateString();

                persons.Add(person);
            }
        }


        public void PrintState()
        {
            DebugUtils.Log($"[{name}]");
            DebugUtils.Log("--- persons ---");
            foreach (var person in persons)
            {
                person.PrintState();
            }
        }
    }


    public class VillageSimulator
    {
        public const int WORLD_WIDTH = 40;
        public const int WORLD_HEIGHT = 30;

        public const int nVillages = 10;

        private List<Village> _villages;


        public VillageSimulator()
        {
            _villages = new();
            for (int i = 0; i < nVillages; i++)
            {
                Village village = new Village();
                village.name = FileStringGenerator.Sites.GenerateString();
                //TODO: choose village.pos

                _villages.Add(village);
            }
        }


        public void PrintState()
        {
            DebugUtils.Log("------ villages ------");
            foreach (var village in _villages)
            {
                village.PrintState();
            }
        }

    }
}
