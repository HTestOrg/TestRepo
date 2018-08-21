using BusinessObjects;

namespace Synoptek
{
    public class GetSessionValues
    {
        #region Fill Common module
        public static Common FillCommonObject(Common _ObjCommon)
        {
            _ObjCommon.UserId = SessionController.UserSession.UserId;
            _ObjCommon.UserName = SessionController.UserSession.UserName;
            _ObjCommon.EmailAddress = SessionController.UserSession.EmailAddress;
            _ObjCommon.RoleType = SessionController.UserSession.RoleType;
            _ObjCommon.IsSubscribed = SessionController.UserSession.IsSubscribed;
            return _ObjCommon;
        }
        #endregion
    }
}