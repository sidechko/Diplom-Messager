package s1pepega.diplom.CorpMessagerServer.services.interfaces;

import org.springframework.lang.NonNull;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.User;
import s1pepega.diplom.CorpMessagerServer.entities.UserChannelLink;

import java.util.List;

public interface UserChannelService {
    @NonNull
    List<UserChannelLink> findAll();

    @NonNull
    List<Channel> getUserChannels(@NonNull Integer userId);
    @NonNull
    List<UserChannelLink> getUserChannelsLink(@NonNull Integer userId);

    @NonNull
    List<User> getChannelUsers(@NonNull Integer channelId);
    @NonNull
    List<UserChannelLink> getChannelUsersLink(@NonNull Integer channelId);

    @NonNull
    UserChannelLink create(@NonNull UserChannelLink link);

    void delete(@NonNull UserChannelLink link);
}
