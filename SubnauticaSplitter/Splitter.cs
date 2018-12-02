using System;
using UnityEngine;

namespace SubnauticaSplitter
{
    public class Splitter : MonoBehaviour
    {
        private TcpConnection _connection = new TcpConnection();

        private float nextReconnect;

        public float Time;

        private bool _runStarted;
        private bool _runEnded;
        private bool _destroying;

        /// <summary>
        /// Update is being called every frame, this is where our logic is
        /// </summary>
        private void Update()
        {
            //If we are not connected to the client, then try to reconnect every 5 seconds
            if (!_connection.Connected && UnityEngine.Time.time > nextReconnect)
            {
                _connection.Connect();
                nextReconnect = UnityEngine.Time.time + 5f;
            }

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
                    //Reset the current game time
                    Time = 0;
                    _connection.StartRun();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SubnauticaSplitter] ERROR: {e}");
            }
        }

        /// <summary>
        /// Called multiple times every frame, this is where we draw things
        /// </summary>
        private void OnGUI()
        {
            if (_destroying)
                return;
            GUI.Label(new Rect(10, Screen.height - 20, 400, 20), $"SubnauticaSplitter - {(_connection.Connected ? "Connected" : "Not Connected")}");
        }
        
        /// <summary>
        /// OnDestroy is called when an object is being destroid, this function respawns itself so it wouldn't die
        /// </summary>
        private void OnDestroy()
        {
            _destroying = true;
            //Respawn our timer if subnautica tries to kill us
            Loader.Load(Time);
        }
    }
}