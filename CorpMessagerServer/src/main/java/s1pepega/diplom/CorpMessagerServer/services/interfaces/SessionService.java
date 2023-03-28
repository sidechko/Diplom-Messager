package s1pepega.diplom.CorpMessagerServer.services.interfaces;

import java.util.HashMap;

public interface SessionService {

    public Integer getUserIdAsSession(Integer sessionId);

    public void appendNewSession(Integer sessionId, Integer userId);

    public Boolean canSendRequest(Integer sessionId, Integer userId);

    public void closeSession(Integer sessionId);

    public Integer getNewSessionId();

    public HashMap<Integer,Integer> getSessions();
}
