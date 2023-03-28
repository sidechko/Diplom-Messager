package s1pepega.diplom.CorpMessagerServer.entities;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.Accessors;

@Getter
@Setter
@Accessors(chain = true)
@Entity
@Table(name = "userchannels")
@JsonIgnoreProperties({"hibernateLazyInitializer"})
public class UserChannelLink {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name="userid", nullable = false, foreignKey = @ForeignKey(name = "fk_userId_accountId"))
    private User user;
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name="availablechannel", nullable = false, foreignKey = @ForeignKey(name = "fk_availableChannel_channelId"))
    private Channel channel;

    @Override
    public String toString() {
        return "UserChannelLink{" +
                "id=" + id +
                ", user=" + user +
                ", channel=" + channel +
                '}';
    }
}
