package s1pepega.diplom.CorpMessagerServer.models;

import lombok.Data;
import lombok.experimental.Accessors;
import s1pepega.diplom.CorpMessagerServer.entities.User;

@Data
@Accessors(chain = true)
public class LoginResponse {
    User user;
    Integer sessionId;
}
