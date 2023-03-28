package s1pepega.diplom.CorpMessagerServer.entities;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.Accessors;

import java.util.Objects;

@Getter
@Setter
@Accessors(chain = true)
@Entity
@Table(name = "channels")
@JsonIgnoreProperties({"hibernateLazyInitializer"})
public class Channel {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer Id;
    @Column(name = "channelname", nullable = false)
    private String name;
    @Column(name = "description")
    private String desc;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Channel channel = (Channel) o;
        return Objects.equals(Id, channel.Id) && Objects.equals(name, channel.name) && Objects.equals(desc, channel.desc);
    }

    @Override
    public int hashCode() {
        return Objects.hash(Id, name, desc);
    }

    @Override
    public String toString() {
        return "Channel{" +
                "Id=" + Id +
                ", name='" + name + '\'' +
                ", desc='" + desc + '\'' +
                '}';
    }
}
