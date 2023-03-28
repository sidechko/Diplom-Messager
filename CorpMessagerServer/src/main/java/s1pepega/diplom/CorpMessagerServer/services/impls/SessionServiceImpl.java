package s1pepega.diplom.CorpMessagerServer.services.impls;

import org.springframework.stereotype.Service;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.SessionService;

import java.util.HashMap;

@Service("SessionServiceImpl")
public class SessionServiceImpl implements SessionService {

    private static final HashMap<Integer, Integer> sessions = new HashMap<>();

    @Override
    public Integer getUserIdAsSession(Integer sessionId) {
        if(!sessions.containsKey(sessionId))
            throw new RuntimeException("unknown session, user not found");
        return sessions.get(sessionId);
    }

    @Override
    public void appendNewSession(Integer sessionId, Integer userId) {
        sessions.putIfAbsent(sessionId,userId);
    }

    @Override
    public Boolean canSendRequest(Integer sessionId, Integer userId) {
        if(userId == null)
            throw new RuntimeException("userId not present");
        if(!sessions.containsKey(sessionId))
            throw new RuntimeException("unknown session, send request canceled");
        return sessions.get(sessionId).equals(userId);
    }

    @Override
    public void closeSession(Integer sessionId) {
        sessions.remove(sessionId);
    }

    @Override
    public Integer getNewSessionId() {
        return (int)System.currentTimeMillis();
    }

    @Override
    public HashMap<Integer, Integer> getSessions() {
        return sessions;
    }
}
