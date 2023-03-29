package s1pepega.diplom.CorpMessagerServer.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import s1pepega.diplom.CorpMessagerServer.entities.UserChannelLink;

import java.util.List;

public interface UserChannelRepository extends JpaRepository<UserChannelLink, Integer> {

    @Query(value = "SELECT DISTINCT * FROM userchannels uc WHERE uc.userId = :id", nativeQuery = true)
    public List<UserChannelLink> getUserChannels(@Param("id") Integer id);

    @Query(value = "SELECT DISTINCT * FROM userchannels uc WHERE uc.availableChannel = :id", nativeQuery = true)
    public List<UserChannelLink> getChannelUsers(@Param("id") Integer id);
}
