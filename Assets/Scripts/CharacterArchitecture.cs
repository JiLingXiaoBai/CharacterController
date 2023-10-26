using ARPG.Camera;
using ARPG.Character.Player;
using QFramework;

public class CharacterArchitecture : Architecture<CharacterArchitecture>
{
    protected override void Init()
    {
        #region Register Model
        RegisterModel<ICameraModel>(new CameraModel());
        RegisterModel<IPlayerModel>(new PlayerModel());
        #endregion
        
        #region Register System
        //
        #endregion
        
        #region Register Utility
        //
        #endregion
        
    }
}