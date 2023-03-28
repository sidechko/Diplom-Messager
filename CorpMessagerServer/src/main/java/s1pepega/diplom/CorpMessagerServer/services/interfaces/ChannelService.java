package s1pepega.diplom.CorpMessagerServer.services.interfaces;

import org.springframework.lang.NonNull;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;

import java.util.List;

public interface ChannelService {

    @NonNull
    List<Channel> findAll();

    @NonNull
    Channel findById(@NonNull Integer id);

    @NonNull
    Channel create(@NonNull Channel createChannel);

    @NonNull
    Channel update(@NonNull Channel createChannel);

    void delete(@NonNull Integer id);
}
