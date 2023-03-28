package s1pepega.diplom.CorpMessagerServer.controllers.rest;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.User;
import s1pepega.diplom.CorpMessagerServer.models.LoginResponse;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.SessionService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserChannelService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserService;

import java.util.HashMap;
import java.util.List;

import static org.springframework.util.MimeTypeUtils.APPLICATION_JSON_VALUE;

@RestController
@RequestMapping("/api/users")
@RequiredArgsConstructor
public class UserControllerREST {
    @Autowired
    private UserService userService;
    @Autowired
    private UserChannelService ucService;
    @Autowired
    private SessionService sessionService;

    @GetMapping(produces = APPLICATION_JSON_VALUE)
    public List<User> findAll() {
        return userService.findAll();
    }

    @GetMapping(value = "/{userId}", produces = APPLICATION_JSON_VALUE)
    public User findById(@PathVariable Integer userId, @RequestHeader("sessionId") Integer sessionId) {
        return userService.findById(userId);
    }

    @GetMapping(value = "/@{userName}", produces = APPLICATION_JSON_VALUE)
    public User findByName(@PathVariable String userName, @RequestHeader("sessionId") Integer sessionId) {
        return userService.findByName(userName);
    }

    @GetMapping(value = "/{userId}/getChannels", produces = APPLICATION_JSON_VALUE)
    public List<Channel> getChannels(@PathVariable Integer userId, @RequestHeader("sessionId") Integer sessionId) {
//        checkSession(userId,sessionId);
        return ucService.getUserChannels(userId);
    }

    @PostMapping(value = "/create", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public LoginResponse create(@RequestBody User request) {
        User newUser = userService.createUser(request);
        Integer sessionId = sessionService.getNewSessionId();
        sessionService.appendNewSession(sessionId,newUser.getId());
        return new LoginResponse().setUser(newUser).setSessionId(sessionId);
    }

    @PostMapping(value = "/login", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public LoginResponse login(@RequestBody User request) {
        User user = userService.login(request);
        Integer sessionId = sessionService.getNewSessionId();
        sessionService.appendNewSession(sessionId,user.getId());
        return new LoginResponse().setUser(user).setSessionId(sessionId);
    }

    @PatchMapping(value = "/update", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public User update(@RequestBody User request, @RequestHeader("sessionId") Integer sessionId) {
        checkSession(request.getId(), sessionId);
        return userService.update(request);
    }

    @DeleteMapping(value = "/delete/{id}", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public User delete(@PathVariable Integer id, @RequestHeader("sessionId") Integer sessionId) {
        checkSession(id,sessionId);
        User user = userService.findById(id);
        userService.delete(id);
        return user;
    }

    private void checkSession(Integer userId, Integer sessionId){
        if(!sessionService.canSendRequest(sessionId, userId))
            throw new RuntimeException("access denied");
    }
}
