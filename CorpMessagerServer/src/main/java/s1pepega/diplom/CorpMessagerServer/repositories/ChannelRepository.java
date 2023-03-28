package s1pepega.diplom.CorpMessagerServer.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;

@Repository
public interface ChannelRepository extends JpaRepository<Channel, Integer> {
}
