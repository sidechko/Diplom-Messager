package s1pepega.diplom.CorpMessagerServer.controllers;

import jakarta.persistence.EntityNotFoundException;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import s1pepega.diplom.CorpMessagerServer.exceptions.IllegalSessionIdException;
import s1pepega.diplom.CorpMessagerServer.exceptions.WIPException;
import s1pepega.diplom.CorpMessagerServer.models.ExceptionResponse;

@RestControllerAdvice
public class ExceptionController {

    @ResponseStatus(HttpStatus.NOT_FOUND)
    @ExceptionHandler(EntityNotFoundException.class)
    private ExceptionResponse notFound(EntityNotFoundException ex) {
        ExceptionResponse response = new ExceptionResponse();
        response.setMessage(ex.getMessage());
        return response;
    }

    @ResponseStatus(HttpStatus.NOT_IMPLEMENTED)
    @ExceptionHandler(WIPException.class)
    private ExceptionResponse notIMPL(WIPException ex){
        ExceptionResponse response = new ExceptionResponse();
        response.setMessage(ex.getMessage());
        return response;
    }

    @ResponseStatus(HttpStatus.UNAUTHORIZED)
    @ExceptionHandler(IllegalSessionIdException.class)
    private ExceptionResponse secure(IllegalSessionIdException ex){
        ExceptionResponse response = new ExceptionResponse();
        response.setMessage(ex.getMessage());
        return response;
    }

    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    @ExceptionHandler(RuntimeException.class)
    private ExceptionResponse error(RuntimeException ex) {
        ExceptionResponse response = new ExceptionResponse();
        response.setMessage(ex.getMessage());
        return response;
    }
}
