using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SubnauticaSplitter
{
    public class Splitter : MonoBehaviour
    {
        private TcpConnection _connection = new TcpConnection();

        public float Time = 0f;

        private bool _runStarted = false;
        private bool _runEnded = false;
        private bool _destroying = false;

        /// <summary>
        /// Update is being called every frame, this is where our logic is
        /// </summary>
        void Update()
        {
            try
            {
                //If the gameobject is being destroyed, or the run has ended: Stop executing
                if (_destroying || _runEnded)
                    return;

                //Get the game time from the day and night cycle
                DayNightCycle cycle = DayNightCycle.main;
                if (cycle != null)
                {
                    //Count the timer up by using unity's Deltatime property
                    if (cycle.dayNightSpeed > 0.2f)
                        Time += UnityEngine.Time.deltaTime;

                    //Send the ingame time to LiveSplit Server
                    _connection.SendGametime(Time);
                }

                //Check if the rocket launch has begun, if so; end the run
                if (LaunchRocket.isLaunching && !_runEnded)
                {
                    _runEnded = true;
                    _connection.Split();
                    return;
                }

                //Check when the pod intro cinematic has STARTED and is not currently ACTIVE, that means we have wen't past it
                EscapePod pod = EscapePod.main;
                if (pod != null && pod.startedIntroCinematic && !pod.introCinematic.cinematicModeActive && !_runStarted)
                {
                    _runStarted = true;
                    _connection.StartRun();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[SubnauticaSplitter] ERROR: " + e);
            }
        }

        /// <summary>
        /// Called multiple times every frame, this is where we draw things
        /// </summary>
        void OnGUI()
        {
            if (_destroying)
                return;
            GUI.Label(new Rect(10, Screen.height - 20, 400, 20), "SubnauticaSplitter - " + (_connection.Connected ? "Connected" : "Not Connected"));
        }
        
        /// <summary>
        /// OnDestroy is called when an object is being destroid, this function respawns itself so it wouldn't die
        /// </summary>
        void OnDestroy()
        {
            _destroying = true;
            //Respawn our timer if subnautica tries to kill us
            Loader.Load(Time);
        }
    }
}