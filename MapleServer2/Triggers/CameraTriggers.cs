using MapleServer2.Packets;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void CameraReset(float interpolationTime)
        {
            Field.BroadcastPacket(SetCameraPacket.Set(interpolationTime));
        }

        public void CameraSelect(int arg1, bool arg2)
        {
        }

        public void CameraSelectPath(int[] pathIds, bool returnView)
        {
            Field.BroadcastPacket(TriggerPacket.Camera(pathIds, returnView));
        }

        public void SetLocalCamera(int cameraId, bool enable)
        {
        }
    }
}
