package s1pepega.diplom.CorpMessagerServer.models;

import lombok.Data;
import lombok.experimental.Accessors;

@Data
@Accessors(chain = true)
public class ExceptionResponse {
    private String message;
}
