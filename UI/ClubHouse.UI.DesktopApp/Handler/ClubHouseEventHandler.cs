using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using agorartc;

using ClubHouse.UI.DesktopApp.Models;

namespace ClubHouse.UI.DesktopApp.Handler {
    public class ClubHouseEventHandler : IRtcEngineEventHandlerBase {

        public ClubHouseEventHandler(RoomUserCollections userLists) {
            OnlineUsers = userLists;
        }

        public RoomUserCollections OnlineUsers { get; }

        public override void OnUserJoined(uint uid, int elapsed) {
            base.OnUserJoined(uid, elapsed);
        }

        public override void OnUserOffline(uint uid, USER_OFFLINE_REASON_TYPE reason) {
            //Debug.WriteLine($"User offline: {uid}, {reason}");
            base.OnUserOffline(uid, reason);
        }

        /// <summary>
        /// This method is deprecated from v3.0.0, use the onRemoteAudioStateChanged callback instead.
        /// This callback does not work properly when the number of users (in the COMMUNICATION profile) or hosts (in the LIVE_BROADCASTING profile) in the channel exceeds 17.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="muted"></param>
        public override void OnUserMuteAudio(uint uid, bool muted) {
            //Debug.WriteLine($"User mute: {uid}, {muted}");
            var userInfo = OnlineUsers.ToList().FirstOrDefault(u => u.User_id == uid);
            if (userInfo != null) {
                ThreadManagerUtil.RunInUI(() => {
                    userInfo.Is_muted = muted;
                });
            }
            else {
                Debug.WriteLine($"User muted: not found {uid}, {muted}");
            }
        }

        /// <summary>
        /// After a successful call of enableAudioVolumeIndication, the SDK continuously detects which remote user has the loudest volume. During the current period, the remote user, who is detected as the loudest for the most times, is the most active user.
        /// </summary>
        /// <param name="uid"></param>
        public override void OnActiveSpeaker(uint uid) {
            base.OnActiveSpeaker(uid);
            Debug.WriteLine($"User speak {uid}");
        }

        public override void OnAudioPublishStateChanged(string channel, STREAM_PUBLISH_STATE oldState, STREAM_PUBLISH_STATE newState, int elapseSinceLastState) {
            base.OnAudioPublishStateChanged(channel, oldState, newState, elapseSinceLastState);
            //Debug.WriteLine($"Publish state changed {channel},\t {oldState},\t {newState},\t {elapseSinceLastState}");
        }

        /// <summary>
        /// DEPRECATED Reports the statistics of the audio stream from each remote user/host.
        /// The SDK triggers this callback once every two seconds
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="quality"></param>
        /// <param name="delay"></param>
        /// <param name="lost"></param>
        public override void OnAudioQuality(uint uid, int quality, ushort delay, ushort lost) {
            //Deprecated as of v2.3.2. Use the onRemoteAudioStats callback instead.
            base.OnAudioQuality(uid, quality, delay, lost);
            //Debug.WriteLine($"Audio Quality {uid},\t {quality},\t {delay},\t {lost}");
        }

        /// <summary>
        /// https://docs.agora.io/en/Voice/API%20Reference/cpp/namespaceagora_1_1rtc.html#ac405a073458682fd6464c82d49fece3c
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="uid"></param>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <param name="elapseSinceLastState"></param>
        public override void OnAudioSubscribeStateChanged(string channel, uint uid, STREAM_SUBSCRIBE_STATE oldState, STREAM_SUBSCRIBE_STATE newState, int elapseSinceLastState) {
            base.OnAudioSubscribeStateChanged(channel, uid, oldState, newState, elapseSinceLastState);
            //Debug.WriteLine($"Audio Subscribe Changed {channel},\t {uid},\t {oldState},\t {newState},\t {elapseSinceLastState}");
        }

        public override void OnClientRoleChanged(CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole) {
            base.OnClientRoleChanged(oldRole, newRole);
            Debug.WriteLine($"Client role changed {oldRole},\t {newRole}");
        }

        /// <summary>
        /// Reports the last mile network quality of each user in the channel once every two seconds.
        /// Last mile refers to the connection between the local device and Agora's edge server.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="txQuality"></param>
        /// <param name="rxQuality"></param>
        public override void OnNetworkQuality(uint uid, int txQuality, int rxQuality) {
            base.OnNetworkQuality(uid, txQuality, rxQuality);
            //Debug.WriteLine($"Network quality: {uid},\t {txQuality},\t {rxQuality}");
        }

        public override void OnRemoteAudioStats(RemoteAudioStats stats) {
            base.OnRemoteAudioStats(stats);
            var userInfo = OnlineUsers.ToList().FirstOrDefault(u => u.User_id == stats.uid);
            if(userInfo!= null) {
                ThreadManagerUtil.RunInUI(() => { userInfo.Is_speaking = (stats.receivedBitrate/stats.numChannels) > 10; });
            }
            //Debug.WriteLine($"Audio Stats {stats.uid},\t {stats.quality},\t {stats.receivedBitrate},\t {stats.receivedSampleRate},\t {stats.numChannels},\t {stats.totalActiveTime}");
        }

        public override void OnLocalAudioStats(LocalAudioStats stats) {
            //Debug.WriteLine($"Audio stats -1,\t {stats.sentBitrate},\t {stats.sentSampleRate},\t {stats.txPacketLossRate}\t");
        }

        public override void OnLocalAudioStateChanged(LOCAL_AUDIO_STREAM_STATE state, LOCAL_AUDIO_STREAM_ERROR error) {
            base.OnLocalAudioStateChanged(state, error);
            //Debug.WriteLine($"State changed -1, {state},\t {error}");
        }

        public override void OnRemoteAudioStateChanged(uint uid, REMOTE_AUDIO_STATE state, REMOTE_AUDIO_STATE_REASON reason, int elapsed) {
            base.OnRemoteAudioStateChanged(uid, state, reason, elapsed);
            //Debug.WriteLine($"State changed {uid},\t {state},\t {reason}");
        }

        public override void OnLocalUserRegistered(uint uid, string userAccount) {
            base.OnLocalUserRegistered(uid, userAccount);
            Debug.WriteLine($"Local user registred {uid},\t {userAccount}");
        }

        public override void OnStreamInjectedStatus(string url, uint uid, int status) {
            base.OnStreamInjectedStatus(url, uid, status);
            Debug.WriteLine($"Stream injected {url},\t {uid},\t {status}");
        }

        public override void OnUserInfoUpdated(uint uid, UserInfo info) {
            base.OnUserInfoUpdated(uid, info);
            Debug.WriteLine($"User info updated: {uid},\t {info}");
        }

        public override void OnStreamMessage(uint uid, int streamId, byte[] data, uint length) {
            base.OnStreamMessage(uid, streamId, data, length);
            Debug.WriteLine($"Stream message: {uid}, {streamId}, {length}");
        }

        public override void OnJoinChannelSuccess(string channel, uint uid, int elapsed) {
            base.OnJoinChannelSuccess(channel, uid, elapsed);
            Debug.WriteLine($"Join success: {channel}, {uid}, {elapsed}");
        }

        public override void OnReJoinChannelSuccess(string channel, uint uid, int elapsed) {
            base.OnReJoinChannelSuccess(channel, uid, elapsed);
            Debug.WriteLine($"ReJoin success: {channel}, {uid}, {elapsed}");
        }
    }
}
