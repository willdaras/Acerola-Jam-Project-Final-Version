using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class CheckpointManager : Common.MonoSingleton<CheckpointManager>
    {
        public CheckPoint currentCheckpoint => _currentCheckpoint;
        [SerializeField] private CheckPoint _currentCheckpoint;

        public void Reset()
        {
            currentCheckpoint.ResetFromCheckpoint();
        }

        public void SetCheckpoint(CheckPoint checkpoint)
        {
            _currentCheckpoint = checkpoint;
        }
    }
}
