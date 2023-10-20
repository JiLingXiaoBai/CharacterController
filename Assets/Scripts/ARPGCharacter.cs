using ARPG.Camera;
using QFramework;

public class ARPGCharacter : Architecture<ARPGCharacter>
{
    protected override void Init()
    {
        #region Register Model
        RegisterModel<ICameraModel>(new CameraModel());
        #endregion
        
        #region Register System
        //
        #endregion
        
        #region Register Utility
        //
        #endregion
        
    }
}