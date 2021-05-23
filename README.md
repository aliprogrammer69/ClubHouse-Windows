# Clubhouse Windows

This is an **unofficial Clubhouse app** for Windows which has been developed in C# and WPF. It is inspired by [this](https://github.com/stypr/clubhouse-py).

# Getting Started
You can download installer and install it or build source code.

## Installation
You can download and install [lastest release](https://github.com/aliprogrammer69/ClubHouse-Windows/releases). It downloads requirments and install app in your PC.

## Build Source Code
For building the code you need following requirments

* .Net Core 5
* Visual C++ Runtime 14
* VSCode or Visual Studio

Then build `ClubHouse.UI.DesktopApp` and run the program.

# Screenshot

<img src="https://github.com/aliprogrammer69/ClubHouse-Windows/blob/main/screenshots/1.jpg"
     alt="Clubhouse-windows" />

# Features
## Authentication
| Service        | Implemented |
| ------------- |:-------------:|
| StartPhoneNumberAuth | ✅
| CallPhoneNumberAuth | ⬜️
| ResendPhoneNumberAuth | ✅
| CompletePhoneNumberAuth | ✅
| CheckForUpdate |  ⬜️
| Logout |  ✅
## ChannelService
| Service | Implemented |
| ------- |:-----------:|
| GetWelcome | ⬜️
| RejectWelcome | ⬜️
| Get | ✅
| Get(channel) | ✅
| Hide | ✅
| GetCreateChannelTargets | ✅
| Create | ✅
| Join | ✅
| ActivePing | ✅
| Leave | ✅
| End | ⬜️
| RaiseHands | ✅
| SetRaiseHandSetting | ⬜️
| UpdateSkintone | ⬜️
| AcceptSpeakerInvite | ✅
| RejectSpeakerInvite | ✅
| InviteSpeaker | ✅
| UninviteSpeaker | ✅
| MuteSpeaker | ✅
| GetSuggestedSpeakers | ⬜️
| AddModerator | ✅
| InviteToJoin | ✅
| InviteToNewChannel | ⬜️
| AcceptInviteToNewChannel | ⬜️
| RejectInviteToNewChannel | ⬜️
| CancelInviteToNewChannel | ⬜️
| MakePublic | ⬜️
| MakeSocial | ⬜️
| BlockUser | ⬜️
| UpdateFlag | ⬜️
## UserService
| Service | Implemented |
| ------- |:-----------:|
| Follow | ✅
| Bulk Follow | ⬜️
| Unfollow | ✅
| Block | ⬜️
| Unblock | ⬜️
| UpdateNotificationFrequency | ⬜️
| GetNotifications | ✅
| SimilarUsers | ⬜️
| IgnoreInSuggestion | ⬜️
| Get | ✅
| GetFollowing | ✅
| GetFollowers | ✅
| GetMutualFollows | ✅
| Search | ✅
## ClubService
| Service | Implemented |
| ------- |:-----------:|
| Follow | ✅
| Unfollow | ✅
| SuggestedUsers | ⬜️
| Get | ✅
| GetMembers | ⬜️
| Search | ✅
| Get | ⬜️
| AddAdmin | ⬜️
| RemoveAdmin | ⬜️
| RemoveMember | ⬜️
| AcceptMemberInvite | ⬜️
| AddMember | ⬜️
| GetNominations | ⬜️
| ApproveNomination | ⬜️
| RejectNomination | ⬜️
| AddTopic | ⬜️
| RemoveTopic | ⬜️
| UpdateFollowAllowed | ⬜️
| UpdateMembershipPrivate | ⬜️
| UpdateCommunity | ⬜️
| UpdateDescription | ⬜️
| UpdateRules | ⬜️
| UpdateTopics | ⬜️
## EventService
| Service | Implemented |
| ------- |:-----------:|
| Get | ⬜️
| GetList | ✅
| GetToStart | ⬜️
| GetForUser | ⬜️
| Create | ⬜️
| Edit | ⬜️
| Delete | ⬜️
## TopicService
| Service | Implemented |
| ------- |:-----------:|
| Get |  ⬜️
| Get(topic) |  ⬜️
| GetClubs | ⬜️
| GetUsers | ⬜️
## ProfileService
| Service | Implemented |
| ------- |:-----------:|
| GetReleaseNotes |  ⬜️
| CheckWaitlistStatus | ✅
| AddEmail |  ⬜️
| UpdatePhoto | ✅
| UpdateUsername | ✅
| UpdateName | ✅
| UpdateDisplayname | ⬜️
| UpdateBio | ✅
| UpdateTwitterUsername | ⬜️
| UpdateInstagramUsername | ⬜️
| SuggestedUsers | ⬜️
| GetSettings | ⬜️
| GetInfo | ✅
| GetNotifications | ✅
| GetActionableNotifications | ⬜️
| IgnoreActionableNotifications | ⬜️
| GetOnlineFriends | ⬜️
| AddTopicInterest | ⬜️
| AddClubInterest | ⬜️
| RemoveTopicInterest | ⬜️
| RemoveClubInterest | ⬜️
| GetSuggestedInvites | ⬜️
| InviteToApp | ✅
| InviteFromWaitlist | ⬜️
| RefreshToken | ⬜️
| UserLog | ⬜️
| ReportIncident | ⬜️

# Contributors
This program has been writen by [Ali Jalali](https://github.com/aliprogrammer69) and [Amin Mirzaee](https://github.com/hifeamin). 

If you need features that is not implemented - feel free to implement and create PRs!

# Licence
Distributed under the MIT License. See [LICENSE](LICENSE.txt) for more information.
