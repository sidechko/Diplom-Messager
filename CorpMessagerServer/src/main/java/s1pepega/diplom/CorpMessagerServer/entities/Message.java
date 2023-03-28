package s1pepega.diplom.CorpMessagerServer.entities;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.Accessors;

import java.sql.Timestamp;
import java.util.Date;
import java.util.Objects;

@Getter
@Setter
@Accessors(chain = true)
@Entity
@Table(name = "messages")
@JsonIgnoreProperties({"hibernateLazyInitializer"})
public class Message {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer Id;
    @Column(name = "content", nullable = false)
    private String content;
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "channel", nullable = false, foreignKey = @ForeignKey(name = "fk_message_channel_id"))
    private Channel channel;
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name="sender", nullable = false, foreignKey = @ForeignKey(name = "fk_message_sender_id"))
    private User sender;
    @JsonFormat(shape = JsonFormat.Shape.STRING)
    @Column(name = "sendtime")
    private Date sendTime;
    @JsonFormat(shape = JsonFormat.Shape.STRING)
    @Column(name = "updatetime")
    private Date updateTime;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Message message = (Message) o;
        return Objects.equals(Id, message.Id) && Objects.equals(content, message.content) && Objects.equals(channel, message.channel) && Objects.equals(sender, message.sender) && Objects.equals(sendTime, message.sendTime) && Objects.equals(updateTime, message.updateTime);
    }

    @Override
    public int hashCode() {
        return Objects.hash(Id, content, channel, sender, sendTime, updateTime);
    }

    @Override
    public String toString() {
        return "Message{" +
                "Id=" + Id +
                ", content='" + content + '\'' +
                ", channel=" + channel +
                ", sender=" + sender +
                ", sendTime=" + sendTime +
                ", updateTime=" + updateTime +
                '}';
    }
}
