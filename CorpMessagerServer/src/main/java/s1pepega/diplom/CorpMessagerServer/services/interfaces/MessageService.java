package s1pepega.diplom.CorpMessagerServer.services.interfaces;

import org.springframework.lang.NonNull;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.Message;

import java.util.List;

public interface MessageService {

    @NonNull
    List<Message> findAll();

    @NonNull
    Message findById(@NonNull Integer id);

    @NonNull
    List<Message> getMessagesInChannel(@NonNull Channel channel, Integer start, Integer count);

    @NonNull
    List<Message> getMessagesAllInChannel(@NonNull Channel channel);

    @NonNull
    Message editMessage(@NonNull Message message);

    @NonNull
    Message sendMessage(@NonNull Message message);

    void delete(@NonNull Integer id);
}
