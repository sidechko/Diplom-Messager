package s1pepega.diplom.CorpMessagerServer.exceptions;

public class IllegalSessionIdException extends RuntimeException{

    public Integer sessionId = -1;
    public IllegalSessionIdException(String message) {
        super(message);
    }

    public  IllegalSessionIdException(String message, Integer id){
        this(message);
        this.sessionId = id;
    }
}
