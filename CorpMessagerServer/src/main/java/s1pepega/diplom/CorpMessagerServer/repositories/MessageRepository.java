package s1pepega.diplom.CorpMessagerServer.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import s1pepega.diplom.CorpMessagerServer.entities.Message;

import java.util.List;

@Repository
public interface MessageRepository extends JpaRepository<Message, Integer> {

    @Query(value = "SELECT * FROM Messages m WHERE m.Channel = :channelId LIMIT :skip, :count", nativeQuery = true)
    List<Message> getMessageInChannelWithStartAndCount(
            @Param("channelId")Integer channelId,
            @Param("skip")Integer skip,
            @Param("count")Integer count);

    @Query(value = "SELECT * FROM Messages m WHERE m.Channel = :channelId", nativeQuery = true)
    List<Message> getMessageAllInChannel(
            @Param("channelId")Integer channelId);
}
