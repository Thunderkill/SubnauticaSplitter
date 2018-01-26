using UnityEngine;

namespace SubnauticaSplitter
{
    public class Loader
    {
        public static GameObject LoadObject;

        /// <summary>
        /// Loads the Splitter class to the game as a new gameobject
        /// </summary>
        public static Splitter Load()
        {
            //Add a new GameObject to the game
            LoadObject = new GameObject();
            //Add the splitter as a MonoBehaviour componenet
            Splitter splitter = LoadObject.AddComponent<Splitter>();
            //Prevent the game from destoying our Splitter component during scene switches
            Object.DontDestroyOnLoad(LoadObject);
            return splitter;
        }

        /// <summary>
        /// Load the Splitter class with a defined time
        /// </summary>
        public static Splitter Load(float time)
        {
            Splitter splitter = Load();
            splitter.Time = time;
            return splitter;
        }

        /// <summary>
        /// Destroy the Splitter gameobject from the game
        /// </summary>
        public static void Unload()
        {
            if (LoadObject == null)
                return;
            Object.Destroy(LoadObject);
        }
    }
}