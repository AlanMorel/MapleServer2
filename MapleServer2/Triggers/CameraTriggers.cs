using MapleServer2.Packets;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void CameraReset(float interpolationTime)
        {
            Field.BroadcastPacket(SetCameraPacket.Set(interpolationTime));
        }

        public void CameraSelect(int cameraId, bool enable)
        {
            Field.State.TriggerCameras[cameraId].IsEnabled = enable;
            Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerCameras[cameraId]));
        }

        public void CameraSelectPath(int[] pathIds, bool returnView)
        {
            Field.BroadcastPacket(TriggerPacket.Camera(pathIds, returnView));
        }

        public void SetLocalCamera(int cameraId, bool enable)
        {
            if (!enable)
            {
                Field.BroadcastPacket(LocalCameraPacket.Camera(cameraId, 0));
            }
        }
    }
}
