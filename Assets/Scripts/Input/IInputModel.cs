using UnityEngine;

namespace ARPG.Input
{
    public interface IInputModel : IModel
    {
        public Vector2 Movement { get; }
        
        public Vector2 CameraLook { get; }
        
        public bool Jump { get; }
        
        public bool LockOn { get; }
        
    }
}